/* TODO: Check if roundWon and gameOver conditions are used in the correct
 * places. */
/* TODO: Write "Create" function. */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ShootAR.Enemies;
using ShootAR.Menu;

namespace ShootAR
{

	public class GameManager : MonoBehaviour
	{
		public delegate void GameOverHandler();
		public event GameOverHandler OnGameOver;
		public delegate void RoundWonHandler();
		public event RoundWonHandler OnRoundWon;

		[HideInInspector] public int level;
		[SerializeField] private AudioClip winSfx;
		private Dictionary<Type, Spawner> spawner;
		private int score;
		[HideInInspector] public bool GameOver, RoundWon;
		[Obsolete] private bool exitTap;
		private AudioSource sfx;

		#region Dependencies
		[SerializeField] private Button fireButton;
		[SerializeField] private Button pauseButton, resumeButton;
		[SerializeField] private UIManager ui;
		[SerializeField] private WebCamTexture cam;
		[SerializeField] private Player player;
		#endregion

		public static GameManager Create(
				Player player, int level = 0, int score = 0,
				AudioClip winSfx = null, AudioSource sfx = null,
				Button fireButton = null, Button pauseButton = null,
				Button resumeButton = null, UIManager ui = null,
				WebCamTexture cam = null
			)
		{
			var o = new GameObject(nameof(GameManager)).AddComponent<GameManager>();

			o.player = player;
			o.level = level;
			o.score = score;
			o.winSfx = winSfx;
			o.sfx = sfx;
			o.fireButton = fireButton;
			o.pauseButton = pauseButton;
			o.resumeButton = resumeButton;
			o.cam = cam;

			if (ui == null)
			{
				o.ui = UIManager.Create(
					uiCanvas: new GameObject(),
					pauseCanvas: new GameObject(),
					bulletCountText: new GameObject().AddComponent<Text>(),
					centerText: new GameObject().AddComponent<Text>(),
					buttonText: new GameObject().AddComponent<Text>(),
					scoreText: new GameObject().AddComponent<Text>(),
					roundText: new GameObject().AddComponent<Text>(),
					sfx: null, pauseSfx: null
				);
			}
			else o.ui = ui;

			return o;
		}

		private void Awake()
		{

#if UNITY_ANDROID

			//Check if Gyroscope is supported
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

			sfx = gameObject.AddComponent<AudioSource>();
			GameOver = false;
		}

		private void Start()
		{
			ui.buttonText.text = "";
			ui.centerText.text = "";
			ui.bulletCountText.text = "";
			fireButton?.onClick.AddListener(OnButtonDown);

			if (player == null)
				Debug.LogError("Player object not found");

			int? roundToPlay = FindObjectOfType<RoundSelectMenu>()?.RoundToPlay;
			if (roundToPlay != null && roundToPlay > 0)
			{
				level = (int)roundToPlay - 1;
			}

			AdvanceLevel();

			GC.Collect();
		}

		private void Update()
		{
			if (!GameOver)
			{
				#region Round Won
				//TO DO: Check following condition. Looks weird... Is the literal true really needed?
				if (spawner.ContainsKey(typeof(Crasher)) ? !spawner[typeof(Crasher)].IsSpawning : true && Enemy.ActiveCount == 0)
				{
					RoundWon = true;
					ui.centerText.text = "Round Clear!";
					sfx.PlayOneShot(winSfx, 0.7f);
					ClearScene();
					ui.buttonText.text = "Tap to continue";
				}
				#endregion

				#region Defeat
				else if (player.Health == 0 || (Bullet.ActiveCount == 0 && player.Ammo == 0 && Enemy.ActiveCount > 0))
				{
					ui.centerText.text = "Rounds Survived : " + (level - 1);
					ClearScene();
					ui.buttonText.text = "Tap to continue";
				}
				#endregion
			}
			else if (OnGameOver != null)
				OnGameOver();
		}

		public void OnApplicationQuit()
		{
			GameOver = true;
			ClearScene();

#if UNITY_EDITOR_WIN
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}


		public void OnButtonDown()
		{
			//Fire Bullet
			if (!GameOver)
			{
				player.Shoot();
			}

			//Tap To Continue
			if (GameOver)
			{
				//Defeat, tap to restart
				if (player.Ammo == 0 || player.Health <= 0)
				{
					cam.Stop();
					SceneManager.LoadScene(1);
				}
				//Next Round tap
				else
				{
					GameOver = false;
					ui.centerText.text = "";
					ui.buttonText.text = "";
					player.Ammo += 6;
					AdvanceLevel();
				}
			}
			else Application.Quit();
		}

		/// <summary>
		/// Prepares for the next level.
		/// </summary>
		private void AdvanceLevel()
		{
			level++;

			// Spawn Patterns
			foreach (var s in spawner)
			{
				switch (s.Key.ToString())
				{
					case nameof(Crasher):
						s.Value.StartSpawning(4 * level + 8);
						break;
					case nameof(Drone):
						s.Value.StartSpawning(3 * level + 6);
						break;
					case nameof(Capsule):
						s.Value.StartSpawning(level + 2);
						break;
				}
			}

			RoundWon = false;
		}

		/// <summary>
		/// Adds points to the score and updates the GUI.
		/// </summary>
		/// <param name="points">The amount of pointts to add.</param>
		public void AddScore(int points)
		{
			if (ui.scoreText != null)
			{
				score += points;
				ui.scoreText.text = "Score: " + score;
			}
		}

		/// <summary>
		/// Deactivates spawners, destroys all spawned objects and set game over state to true. 
		/// </summary>
		private void ClearScene()
		{
			GameOver = true;

			foreach (Spawner spawner in spawner.Values)
				spawner?.StopSpawning();

			Enemy[] enemies = FindObjectsOfType<Enemy>();
			foreach (var e in enemies) Destroy(e.gameObject);
			Capsule[] capsules = FindObjectsOfType<Capsule>();
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
			GUILayout.Label($"Game Over: {GameOver}\nRound Over: {RoundWon}");
		}
#endif
	}
}