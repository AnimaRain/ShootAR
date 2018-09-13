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
		public delegate void GameOver();
		public event GameOver OnGameOver;

		[HideInInspector] public int level;
		[SerializeField] private readonly AudioClip winSfx;
		private Dictionary<Type, Spawner> enemySpawner;
		private Spawner capsuleSpawner;
		private int score;
		[HideInInspector] public bool gameOver, roundWon;
		private bool exitTap;
		private AudioSource sfx;

		#region Dependencies
		[SerializeField] private readonly Button fireButton;
		[SerializeField] private readonly Button pauseButton, resumeButton;
		[SerializeField] private readonly UIManager ui;
		[SerializeField] private readonly WebCamTexture cam;
		[SerializeField] private readonly Player player;
		private TVScript tvScreen;
		#endregion


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

			enemySpawner = new Dictionary<Type, Spawner>();
			Spawner[] enemySpawners = FindObjectsOfType<Spawner>();
			if (enemySpawners == null)
			{
				Debug.LogError("Could not find \"Enemy\" spawners.");
			}
			else
			{
				foreach (Spawner spawner in enemySpawners)
				{
					Type type = spawner.ObjectToSpawn.GetType();
					enemySpawner.Add(type, spawner);
#if DEBUG
					Debug.Log($"Found spawner of type \"{type}\"");
#endif
				}
			}
			capsuleSpawner = FindObjectOfType<Spawner>();
			if (capsuleSpawner == null)
			{
				Debug.LogError("Could not find \"Capsule\" spawners.");
			}

			if (player == null)
				Debug.LogError("Player object not found");

			sfx = gameObject.AddComponent<AudioSource>();
			gameOver = false;
		}

		private void Start()
		{
			if (tvScreen == null) tvScreen = GameObject.Find("TVScreen").GetComponent<TVScript>();
			ui.buttonText.text = "";
			ui.centerText.text = "";
			ui.bulletCountText.text = "";
			fireButton.onClick.AddListener(OnButtonDown);

			//if player chose to start from a higher level, assign that level
			int roundToPlay = FindObjectOfType<RoundSelectMenu>().RoundToPlay;
			if (roundToPlay > 0)
			{
				level = roundToPlay - 1;
			}

			if (!exitTap)
			{
				AdvanceLevel();
			}

			GC.Collect();
		}

		private void Update()
		{
			if (!gameOver)
			{
				#region Round Won
				//TO DO: Check following condition. Looks weird... Is the literal true really needed?
				if (enemySpawner.ContainsKey(typeof(Crasher)) ? !enemySpawner[typeof(Crasher)].IsSpawning : true && Enemy.ActiveCount == 0)
				{
					roundWon = true;
					ui.centerText.text = "Round Clear!";
					sfx.PlayOneShot(winSfx, 0.7f);
					tvScreen.CloseTV();
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
		}

		public void OnApplicationQuit()
		{
			gameOver = true;
			ClearScene();

#if UNITY_EDITOR_WIN
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}


		public void OnButtonDown()
		{
			if (exitTap == false)
			{
				//Fire Bullet
				if (!gameOver)
				{
					player.Shoot();
				}

				//Tap To Continue
				if (gameOver)
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
						gameOver = false;
						ui.centerText.text = "";
						ui.buttonText.text = "";
						player.Ammo += 6;
						AdvanceLevel();
					}
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
			tvScreen.Invoke("StartTV", 10);

			// Spawn Patterns
			foreach (var spawner in enemySpawner)
			{
				switch (spawner.Key.ToString())
				{
					case nameof(Crasher):
						spawner.Value.StartSpawning(4 * level + 8);
						break;
					case nameof(Drone):
						spawner.Value.StartSpawning(3 * level + 6);
						break;
				}
			}
			capsuleSpawner.StartSpawning(level + 2);

			roundWon = false;
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
			gameOver = true;

			capsuleSpawner.StopSpawning();
			foreach (Spawner spawner in enemySpawner.Values)
				spawner.StopSpawning();

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
			GUILayout.Label($"Game Over: {gameOver}\nRound Over: {roundWon}");
		}
#endif
	}
}