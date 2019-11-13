using ShootAR.Enemies;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShootAR
{

	[RequireComponent(typeof(AudioSource))]
	public class GameManager : MonoBehaviour
	{
		private const int   CAPSULE_BONUS_POINTS = 50,
							ROUND_AMMO_REWARD = 6;
#if DEBUG
#pragma warning disable IDE1006 // Suppress naming rule violation
		private string SPAWN_PATTERN_FILE_PATH;
#pragma warning restore IDE1006
#else
		private const string SPAWN_PATTERN_FILE_PATH = "spawnpatterns.xml";
#endif

		[SerializeField] private AudioClip victoryMusic;
		private Dictionary<Type, List<Spawner>> spawnerGroups;
		[SerializeField] private ScoreManager scoreManager;
		[Obsolete] private bool exitTap;    // Why do we need this? Should it be removed?
		private AudioSource audioPlayer;
		[SerializeField] private GameState gameState;
		[SerializeField] private Button fireButton;
		[SerializeField] private UIManager ui;
		private WebCamTexture cam;
		[SerializeField] private RawImage backgroundTexture;
		[SerializeField] private Player player;
		[SerializeField] private PrefabContainer prefabs;
		private Stack<Spawner> stashedSpawners;
		private XmlReader spawnPattern;

		public static GameManager Create(
			string spawnPattern,
			Player player, GameState gameState,
			PrefabContainer prefabs,
			ScoreManager scoreManager = null,
			AudioClip victoryMusic = null, AudioSource sfx = null,
			Button fireButton = null, RawImage background = null,
			UIManager ui = null
		) {
			var o = new GameObject(nameof(GameManager)).AddComponent<GameManager>();

			o.SPAWN_PATTERN_FILE_PATH = spawnPattern;
			o.player = player;
			o.gameState = gameState;
			o.prefabs = prefabs;
			o.scoreManager = scoreManager;
			o.victoryMusic = victoryMusic;
			o.audioPlayer = sfx;
			o.fireButton = fireButton;
			o.backgroundTexture = background ?? new GameObject("Background")
														.AddComponent<RawImage>();
			o.ui = ui ??
				UIManager.Create(
					uiCanvas: new GameObject("UI"),
					pauseCanvas: new GameObject("PauseScreen"),
					bulletCount: new GameObject("Ammo").AddComponent<Text>(),
					messageOnScreen: new GameObject("Message").AddComponent<Text>(),
					score: new GameObject("Score").AddComponent<Text>(),
					roundIndex: new GameObject("Level").AddComponent<Text>(),
					sfx: null, pauseSfx: null,
					gameState: o.gameState
				);

			return o;
		}

		private void Awake() {
#if UNITY_ANDROID
			if (!SystemInfo.supportsGyroscope) {
				exitTap = true;
				const string error = "This device does not have Gyroscope";
				if (ui != null)
					ui.MessageOnScreen.text = error;
				throw new UnityException(error);
			}
			else {
				Input.gyro.enabled = true;
			}

			//Set up the rear camera
			for (int i = 0; i < WebCamTexture.devices.Length; i++) {
				if (!WebCamTexture.devices[i].isFrontFacing) {
					cam = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
					break;
				}
			}
#endif
			/* Do not use elif here. While testing
			 * using Unity Remote 5, it does not use
			 * the camera on the phone and it has to
			 * fall back on the webcam. We need both
			 * UNITY_ANDROID and UNITY_EDITOR for that. */
#if UNITY_EDITOR
			cam = new WebCamTexture();
#endif
		}

		private void Start() {
			if (player == null)
				throw new UnityException("Player object not found");
			if (gameState == null)
				throw new UnityException("GameState object not found");
			if (prefabs is null)
				throw new UnityException("Collection of prefabs not found");
			if (cam == null) {
				const string error = "This device does not have a rear camera";
				ui.MessageOnScreen.text = error;
				throw new UnityException(error);
			}

			cam.Play();
			backgroundTexture.texture = cam;
			backgroundTexture.rectTransform
				.localEulerAngles = new Vector3(0, 0, cam.videoRotationAngle);
			float scaleY = cam.videoVerticallyMirrored ? -1.0f : 1.0f;
			float videoRatio = (float)cam.width / (float)cam.height;
			backgroundTexture.rectTransform
				// Through testing i found out that using these settings makes
				// the most optimal outcome.
				.localScale = new Vector3(scaleY, scaleY / videoRatio, 1);

			fireButton?
				.onClick.AddListener(() => {
					if (gameState.GameOver) {
						/* Because pools are static they require to be manually
						 * emptied when the scene is reloaded or else bugs will
						 * occur. Not all pools are required to be emptied, but
						 * this way it is easier to manage.*/
						Spawnable.Pool<BulletCapsule>.Empty();
						Spawnable.Pool<ArmorCapsule>.Empty();
						Spawnable.Pool<HealthCapsule>.Empty();
						Spawnable.Pool<PowerUpCapsule>.Empty();
						Spawnable.Pool<Crasher>.Empty();
						Spawnable.Pool<Drone>.Empty();
						Spawnable.Pool<Bullet>.Empty();

						SceneManager.LoadScene(1);
					}
					else if (gameState.RoundWon) {
						ui.MessageOnScreen.text = "";
						AdvanceLevel();
					}
					else
						player.Shoot();
				});

			ui.BulletCount.text = player.Ammo.ToString();

			stashedSpawners = new Stack<Spawner>(2);
			spawnerGroups = new Dictionary<Type, List<Spawner>>();

			/* The round index is assigned an initial value diminished by 1,
			 * since AdvanceLevel will add it back. */
			gameState.Level = Configuration.StartingLevel - 1;
			player.Ammo += gameState.Level * 15;    /* initial Ammo value set in
													 * Inspector */
			Spawnable.Pool<Bullet>.Populate(prefabs.Bullet, 10);
			AdvanceLevel();

			GC.Collect();
		}

		private void OnEnable() {
			if (gameState is null) return;

			gameState.OnGameOver += OnGameOver;
			gameState.OnRoundWon += OnRoundWon;
		}

		private void FixedUpdate() {
			if (!gameState.GameOver) {
				// Round Won
				bool spawnersStoped = true;
				foreach (var type in spawnerGroups.Keys) {
					foreach (var spawner in spawnerGroups[type])
						if (type.IsSubclassOf(typeof(Enemy))
							&& spawner.IsSpawning) {
							spawnersStoped = false;
							break;
						}
				}
				if (spawnersStoped && Enemy.ActiveCount == 0) {
					gameState.RoundWon = true;
				}

				// Defeat
				else if (Enemy.ActiveCount > 0 && Bullet.ActiveCount == 0
						&& player.Ammo == 0) {
					gameState.GameOver = true;
				}
			}
		}

		private void OnDisable() {
			if (gameState != null) {
				gameState.OnGameOver -= OnGameOver;
				gameState.OnRoundWon -= OnRoundWon;
			}
		}

		private void OnDestroy() {
			/* cam.Stop() is required to stop the camera so it can be
			 * restarted when the scene loads again; else, after the 
			 * scene reloads, the feedback will be blank. */
			cam.Stop();
			gameState.GameOver = true;
			ClearScene();
		}

		private void OnApplicationQuit() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}


		/// <summary>
		/// In here lies the code that runs before each round.
		/// </summary>
		private void AdvanceLevel() {
			gameState.Level++;
#if DEBUG
			Debug.Log($"Advancing to level {gameState.Level}");
#endif

			#region Spawn Pattern
			Type type = default;
			int limit = default, currentSpawnerIndex = -1,
				ammoEnemyDifference = player.Ammo,
				availableSpawners = 0, requiredSpawners = 0;
			float   rate = default, delay = default,
					maxDistance = default, minDistance = default;
			List<Spawner> spawnerGroup = null;
			bool newSpawnerRequired = false, addLimitToSum = false,
				 doneParsingForCurrentLevel = false;

			while (!doneParsingForCurrentLevel) {
				if (!(spawnPattern?.Read() ?? false)) {
					spawnPattern = XmlReader.Create(SPAWN_PATTERN_FILE_PATH);
					spawnPattern.MoveToContent();
				}

				switch (spawnPattern.NodeType) {
				case XmlNodeType.Element:
					switch (spawnPattern.Name) {
					// Find type of Spawnable and get the related spawner group.
					case nameof(Crasher):
						addLimitToSum = true;
						type = typeof(Crasher);
						if (Spawnable.Pool<Crasher>.Count == 0)
							Spawnable.Pool<Crasher>.Populate(prefabs.Crasher);
						goto case nameof(Spawnable);
					case nameof(Drone):
						addLimitToSum = true;
						type = typeof(Drone);
						if (Spawnable.Pool<Drone>.Count == 0) {
							Spawnable.Pool<Drone>.Populate(prefabs.Drone);
							Spawnable.Pool<EnemyBullet>
								.Populate(prefabs.EnemyBullet);
						}
						goto case nameof(Spawnable);
					case nameof(BulletCapsule):
						type = typeof(BulletCapsule);
						if (Spawnable.Pool<BulletCapsule>.Count == 0)
							Spawnable.Pool<BulletCapsule>
								.Populate(prefabs.BulletCapsule);
						goto case nameof(Spawnable);
					case nameof(HealthCapsule):
						type = typeof(HealthCapsule);
						if (Spawnable.Pool<HealthCapsule>.Count == 0)
							Spawnable.Pool<HealthCapsule>
								.Populate(prefabs.HealthCapsule as HealthCapsule);
						goto case nameof(Spawnable);
					case nameof(ArmorCapsule):
						type = typeof(ArmorCapsule);
						if (Spawnable.Pool<ArmorCapsule>.Count == 0)
							Spawnable.Pool<ArmorCapsule>
								.Populate(prefabs.ArmorCapsule as ArmorCapsule);
						goto case nameof(Spawnable);
					case nameof(PowerUpCapsule):
						type = typeof(PowerUpCapsule);
						if (Spawnable.Pool<PowerUpCapsule>.Count == 0)
							Spawnable.Pool<PowerUpCapsule>
								.Populate(prefabs.PowerUpCapsule as PowerUpCapsule);
						goto case nameof(Spawnable);
					case nameof(Spawnable):
						if (!spawnerGroups.ContainsKey(type))
							spawnerGroups.Add(type, new List<Spawner>());
						spawnerGroup = spawnerGroups[type];
						availableSpawners = spawnerGroup.Count;
						break;


					/* Count how many spawners of a type will be required.
					 * If there are not enough spawners in the group, take
					 * one from the stashed pile or mark that a new one should
					 * be created. */
					case "pattern":
						if (availableSpawners - ++currentSpawnerIndex <= 0)
							if (stashedSpawners.Count > 0)
								spawnerGroup.Add(stashedSpawners.Pop());
							else
								newSpawnerRequired = true;
						requiredSpawners++;
						break;

					// Get spawner configuration data.
					case nameof(limit):
						limit = spawnPattern.ReadElementContentAsInt();
						if (addLimitToSum) ammoEnemyDifference -= limit;
						addLimitToSum = false;
						break;
					case nameof(rate):
						rate = spawnPattern.ReadElementContentAsFloat();
						break;
					case nameof(delay):
						delay = spawnPattern.ReadElementContentAsFloat();
						break;
					case nameof(maxDistance):
						maxDistance = spawnPattern.ReadElementContentAsFloat();
						break;
					case nameof(minDistance):
						minDistance = spawnPattern.ReadElementContentAsFloat();
						break;
					}
					break;

				// Configure spawner using the retrieved data.
				case XmlNodeType.EndElement
				when spawnPattern.Name == "pattern":
					if (newSpawnerRequired) {
						spawnerGroup.Add(
							Spawner.Create(
								type, limit, rate, delay,
								maxDistance, minDistance
							)
						);
						spawnerGroup[currentSpawnerIndex].StartSpawning();

						newSpawnerRequired = false;
					}
					else
						spawnerGroup[currentSpawnerIndex]
							.StartSpawning(
								type, limit, rate, delay,
								maxDistance, minDistance
							);
					break;

				// When done with a type of Spawnable, stash leftover spawners.
				case XmlNodeType.EndElement
				when spawnPattern.Name == nameof(Crasher) ||
					 spawnPattern.Name == nameof(Drone) ||
					 spawnPattern.Name == nameof(BulletCapsule) ||
					 spawnPattern.Name == nameof(ArmorCapsule) ||
					 spawnPattern.Name == nameof(HealthCapsule) ||
					 spawnPattern.Name == nameof(PowerUpCapsule):
					for (int i = currentSpawnerIndex + 1;
							availableSpawners - requiredSpawners > i;
							i++) {
						stashedSpawners.Push(spawnerGroup[i]);
						spawnerGroup.RemoveAt(i);
					}
					currentSpawnerIndex = -1;
					break;

				case XmlNodeType.EndElement
				when spawnPattern.Name == "level":

					/* Player should always have enough ammo to play the next
					 * round. If they already have more than enough, they get
					 * points. */
					if (ammoEnemyDifference > 0)
						scoreManager.AddScore(ammoEnemyDifference * 10);
					else if (ammoEnemyDifference < 0)
						player.Ammo += -ammoEnemyDifference;

					doneParsingForCurrentLevel = true;
					break;
				}
			}
			//TODO: Stash entire group of spawners when that type is not used
			// in a round.
			#endregion

			gameState.RoundWon = false;
			gameState.RoundStarted = true;
		}

		/// <summary>
		/// Destroys all spawned objects. 
		/// </summary>
		private void ClearScene() {
			// Be merciful. Player deserves some points for the unused capsules.
			if (gameState.RoundWon) {
				Capsule[] capsules = FindObjectsOfType<Capsule>();
				scoreManager?.AddScore(capsules.Length * CAPSULE_BONUS_POINTS);
				foreach (var c in capsules) c.Destroy();
			}

			Spawnable[] spawnables = FindObjectsOfType<Spawnable>();
			foreach (var s in spawnables) s.Destroy();

#if DEBUG
			Debug.Log("Scene cleared.");
#endif
		}

		public void GoToMenu() {
			cam.Stop();
			SceneManager.LoadScene("MainMenu");
		}

		private void OnGameOver() {
			if (ui != null) {
				var survivedRounds = gameState.Level - Configuration.StartingLevel;
				ui.MessageOnScreen.text =
					$"Game Over\n\n" +
					$"Rounds Survived : {survivedRounds}";
			}

			ClearScene();
		}

		private void OnRoundWon() {
			ui.MessageOnScreen.text = "Round Clear!";
			audioPlayer?.PlayOneShot(victoryMusic, 0.7f);
			ClearScene();
		}


#if DEBUG
		private void OnGUI() {
			GUILayout.Label(
				$"Build {Application.version}\n" +
				$"Game Over: {gameState.GameOver}\n" +
				$"Round Over: {gameState.RoundWon}"
			);
		}
#endif
	}
}
