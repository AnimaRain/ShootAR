using ShootAR.Enemies;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace ShootAR
{

	[RequireComponent(typeof(AudioSource))]
	public class GameManager : MonoBehaviour
	{
		private const int   CAPSULE_BONUS_POINTS = 50,
							ROUND_AMMO_REWARD = 6;

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
		private Stack<Spawner> stashedSpawners;

		private bool readyToRestart = false;

		public static GameManager Create(
			Player player, GameState gameState,
			ScoreManager scoreManager = null,
			AudioClip victoryMusic = null, AudioSource sfx = null,
			Button fireButton = null, RawImage background = null,
			UIManager ui = null
		) {
			var o = new GameObject(nameof(GameManager)).AddComponent<GameManager>();

			o.player = player;
			o.gameState = gameState;
			o.scoreManager = scoreManager;
			o.victoryMusic = victoryMusic;
			o.audioPlayer = sfx;
			o.fireButton = fireButton;
			o.backgroundTexture =
				background
				??
				new GameObject("Background").AddComponent<RawImage>();
			o.ui = ui ??
				UIManager.Create(
					uiCanvas: new GameObject("UI"),
					pauseCanvas: new GameObject("PauseScreen"),
					bulletCount: new GameObject("Ammo").AddComponent<Text>(),
					bulletPlus: new GameObject("Ammo Reward").AddComponent<Text>(),
					messageOnScreen: new GameObject("Message").AddComponent<Text>(),
					score: new GameObject("Score").AddComponent<Text>(),
					roundIndex: new GameObject("Level").AddComponent<Text>(),
					sfx: null, pauseSfx: null,
					gameState: o.gameState
				);

			return o;
		}

		private void Awake() {
#if UNITY_ANDROID && !UNITY_EDITOR
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
						if (readyToRestart) {
							SceneManager.LoadScene(1);
						}
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

			gameState.Level = 0;

			Spawnable.Pool<Bullet>.Instance.Populate(10);

			AudioListener.volume = Configuration.Instance.SoundMuted ? 0f : Configuration.Instance.Volume;

			AdvanceLevel();

			GC.Collect();
		}

		private void OnEnable() {
			if (gameState is null) return;

			gameState.OnGameOver += OnGameOver;
			gameState.OnRoundWon += OnRoundWon;
		}

		private void FixedUpdate() {
			if (gameState.RoundStarted && !gameState.GameOver) {
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
			ClearScene();
		}

		private void OnApplicationQuit() {
			if (Configuration.Instance.UnsavedChanges)
				Configuration.Instance.SaveSettings();

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}

		public void OnApplicationPause() {
			if (Configuration.Instance.UnsavedChanges)
				Configuration.Instance.SaveSettings();
		}


		/// <summary>
		/// In here lies the code that runs before each round.
		/// </summary>
		private void AdvanceLevel() {
			gameState.Level++;
#if DEBUG
			Debug.Log($"Advancing to level {gameState.Level}");
#endif

			// Configuring spawners
			Stack<Spawner.SpawnConfig>[] patterns
				= Spawner.ParseSpawnPattern(Configuration.Instance.SpawnPatternFile);

			Spawner.SpawnerFactory(patterns, 0, ref spawnerGroups, ref stashedSpawners);

			int totalEnemies = 0;
			foreach (var group in spawnerGroups) {
				group.Value.ForEach(spawner => {
					spawner.StartSpawning();

					if (group.Key.IsSubclassOf(typeof(Enemy))) {
						totalEnemies += spawner.SpawnLimit;

					}
				});
			}

			/* Player should always have enough ammo to play the next
			 * round. If they already have more than enough, they get
			 * points. */
			ulong difference = (ulong)(player.Ammo - totalEnemies);
			if (difference > 0)
				scoreManager.AddScore(difference * 10);
			else if (difference < 0) {
				/* If it is before the 1st round, give player more bullets
				 * so they are allowed to miss shots. */
				const float bonusBullets = 0.55f;
				if (gameState.Level == 1) {
					difference *=  (ulong)bonusBullets;
				}

				player.Ammo += (difference < int.MaxValue) ? -(int)difference : int.MaxValue;
			}

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
				scoreManager?.AddScore((ulong)(capsules.Length * CAPSULE_BONUS_POINTS));
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
				ui.MessageOnScreen.text =
					$"Game Over\n\n" +
					$"Rounds Survived : {gameState.Level}";
			}

			// Stop all spawners from spawning
			foreach (List<Spawner> spawners in spawnerGroups.Values) {
				spawners.ForEach(spawner => {
					spawner.StopSpawning();
				});
			}

			// Check for highscore
			ScoreList highscores = ScoreList.LoadScores();
			if (!highscores.Exists(scoreManager.Score)) {
				// make sure the game does not restart before recording the score
				readyToRestart = false;

				/* Player is asked for their name asynchronusly.
				 * When the name has been submitted, the score is added to the
				 * table and the table is saved to file. */
				StartCoroutine(
					ui.AskName(name => {
						highscores.AddScore(name, scoreManager.Score);

						using (BinaryWriter writer = new BinaryWriter(
							Configuration.Instance.Highscores.OpenWrite()
						)) {
							for (int i = 0; i < ScoreList.POSITIONS; i++) {
								(string, ulong) score = highscores[i];
								writer.Write(score.Item1 ?? ""); // write name
								writer.Write(score.Item2); // write points
							}
						}

						readyToRestart = true;
					})
				);
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
