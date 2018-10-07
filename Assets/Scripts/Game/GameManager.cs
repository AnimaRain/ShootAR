using ShootAR.Enemies;
using ShootAR.Menu;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShootAR
{

	public class GameManager : MonoBehaviour
	{
		[SerializeField] private AudioClip winSfx;
		private Dictionary<Type, Spawner> spawner;
		[SerializeField] private ScoreManager scoreManager;
		[Obsolete] private bool exitTap;
		private AudioSource sfx;
		[SerializeField] private GameState gameState;

		#region Dependencies
		[SerializeField] private Button fireButton;
		[SerializeField] private UI ui;
		private WebCamTexture cam;
		[SerializeField] private Player player;
		#endregion

		public static GameManager Create(
				Player player, GameState gameState, ScoreManager scoreManager = null,
				AudioClip winSfx = null, AudioSource sfx = null,
				Button fireButton = null, UI ui = null
			)
		{
			var o = new GameObject(nameof(GameManager)).AddComponent<GameManager>();

			o.player = player;
			o.gameState = gameState;
			o.scoreManager = scoreManager;
			o.winSfx = winSfx;
			o.sfx = sfx;
			o.fireButton = fireButton;

			if (ui == null)
			{
				o.ui = UI.Create(
					uiCanvas: new GameObject(),
					pauseCanvas: new GameObject(),
					bulletCount: new GameObject().AddComponent<Text>(),
					messageOnScreen: new GameObject().AddComponent<Text>(),
					score: new GameObject().AddComponent<Text>(),
					roundIndex: new GameObject().AddComponent<Text>(),
					sfx: null, pauseSfx: null, gameState: o.gameState
				);
			}
			else o.ui = ui;

			return o;
		}

		private void Awake()
		{
#if UNITY_EDITOR
			Debug.unityLogger.logEnabled = true;
#else
			Debug.unityLogger.logEnabled = false;
#endif

#if UNITY_ANDROID

			if (!SystemInfo.supportsGyroscope)
			{
				const string error = "This device does not have Gyroscope";
				Debug.LogError(error);
				ui.buttonText.text = error + "\n\nTap to exit";
				exitTap = true;
			}
			else
			{
				//Enable gyro
				Input.gyro.enabled = true;
			}
#endif
		}

		private void Start()
		{
			if (player == null)
			{
				const string error = "Player object not found";
				Debug.LogError(error);
				throw new Exception(error);
			}
			if (gameState == null)
			{
				const string error = "GameState object not found";
				Debug.LogError(error);
				throw new Exception(error);
			}

			ui.MessageOnScreen.text = "";
			ui.BulletCount.text = "";
			fireButton?.onClick.AddListener(OnTap);
			sfx = gameObject.AddComponent<AudioSource>();

			int? roundToPlay = FindObjectOfType<RoundSelectMenu>()?.RoundToPlay;
			if (roundToPlay != null && roundToPlay > 0)
			{
				gameState.Level = (int)roundToPlay - 1;
			}

			spawner = new Dictionary<Type, Spawner>();
			Spawner[] spawners = FindObjectsOfType<Spawner>();
			if (spawners == null)
			{
				Debug.LogError("Could not find spawners.");
			}
			else
			{
				foreach (Spawner s in spawners)
				{
					Type type = s.ObjectToSpawn.GetType();
					spawner.Add(type, s);
#if DEBUG
					Debug.Log($"Found spawner of type \"{type}\"");
#endif
				}
			}

			AdvanceLevel();

			GC.Collect();
		}

		private void FixedUpdate()
		{
			if (!gameState.GameOver)
			{
				#region Round Won
				bool spawnersStoped = true;
				foreach (var type in spawner.Keys)
					if (type == typeof(Enemy) && spawner[type].IsSpawning)
					{
						spawnersStoped = false;
						break;
					}
				if (spawnersStoped && Enemy.ActiveCount == 0)
				{
					Debug.Log("Round won");
					gameState.RoundWon = true;
					ui.MessageOnScreen.text = "Round Clear!";
					sfx?.PlayOneShot(winSfx, 0.7f);
					ClearScene();
				}
				#endregion

				#region Defeat
				else if (player.Health == 0 ||
					(Enemy.ActiveCount > 0 && Bullet.ActiveCount == 0 &&
					player.Ammo == 0))
				{
					Debug.Log("Player defeated");
					ui.MessageOnScreen.text =
						$"Rounds Survived : {gameState.Level - 1}";
					gameState.GameOver = true;
					ClearScene();
				}
				#endregion
			}
		}

		public void OnApplicationQuit()
		{
			gameState.GameOver = true;
			ClearScene();

#if UNITY_EDITOR_WIN
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}


		public void OnTap()
		{
			if (gameState.RoundWon)
			{
				gameState.GameOver = false;
				ui.MessageOnScreen.text = "";
				player.Ammo += 6;
				AdvanceLevel();
			}
			else if (gameState.GameOver)
			{
				// TODO: Comment why cam.Stop() is required here.
				cam.Stop();
				SceneManager.LoadScene(1);
			}
			else
				player.Shoot();
		}

		/// <summary>
		/// Prepares for the next level.
		/// </summary>
		private void AdvanceLevel()
		{
			gameState.Level++;
#if DEBUG
			Debug.Log($"Advancing to level {gameState.Level}");
#endif

			foreach (var s in spawner)
			{
				switch (s.Key.ToString())
				{
					#region Spawn Patterns
					case nameof(Crasher):
						s.Value.StartSpawning(4 * gameState.Level + 8);
						break;
					case nameof(Drone):
						s.Value.StartSpawning(3 * gameState.Level + 6);
						break;
					case nameof(Capsule):
						s.Value.StartSpawning(gameState.Level + 2);
						break;
						#endregion
				}
			}

			gameState.RoundWon = false;
		}

		/// <summary>
		/// Deactivates spawners and destroys all spawned objects. 
		/// </summary>
		private void ClearScene()
		{
			Debug.Log("Clearing scene...");

			foreach (Spawner spawner in spawner.Values)
				spawner?.StopSpawning();

			Enemy[] enemies = FindObjectsOfType<Enemy>();
			foreach (var e in enemies) Destroy(e.gameObject);
			Capsule[] capsules = FindObjectsOfType<Capsule>();
			if (gameState.RoundWon && scoreManager != null)
				scoreManager.AddScore(capsules.Length * Capsule.BONUS_POINTS);
			foreach (var c in capsules) Destroy(c.gameObject);
		}

		public void GoToMenu()
		{
			cam.Stop();
			SceneManager.LoadScene("MainMenu");
		}


#if DEBUG
		private void OnGUI()
		{
			GUILayout.Label($"Game Over: {gameState.GameOver}\nRound Over: {gameState.RoundWon}");
		}
#endif
	}
}