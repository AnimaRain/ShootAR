/* TODO: Check if roundWon and gameOver conditions are used in the correct
 * places. */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private const float ShotCooldown = 0.35f;

	public delegate void GameOver();
	public event GameOver OnGameOver;

	[SerializeField] private Button fireButton;
	[SerializeField] private Bullet bullet;
	[SerializeField] private Button pauseButton, resumeButton;
	[HideInInspector] public int level;
	[SerializeField] private AudioClip winSfx;
	private Dictionary<string, Spawner> spawner;
	private int score;
	private TVScript tvScreen;

	[HideInInspector] public bool gameOver, roundWon;
	private bool exitTap;
	private Player player;
	private AudioSource sfx;
	private float nextFire;

	private UIManager ui;
	private WebCamTexture cam;


	private void Awake()
	{
		ui = GameObject.Find("UIManager").GetComponent<UIManager>();

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

		//TODO: Add if cam == null code here

		cam = FindObjectOfType<Background>().cam;

		/* Create a dictionary of all spawners by setting the name of
         * their assigned ObjectToSpawn as a key and the spawner itself
         * as the value.*/
		spawner = new Dictionary<string, Spawner>();
		Spawner[] spawnerParent = GameObject.Find("Spawners").GetComponentsInChildren<Spawner>();
		if (spawnerParent == null)
		{
			Debug.LogError("Could not find Object \"Spawners\" or no Spawner scripts where attached to it.");
		}
		else
		{
			foreach (Spawner spawner in spawnerParent)
			{
				string type = spawner.objectToSpawn.name;
				this.spawner.Add(type, spawner);
			}
		}

		player = GameObject.Find("Player").GetComponent<Player>();
		if (player == null)
			Debug.LogError("Player object not found");

		sfx = gameObject.AddComponent<AudioSource>();
		Bullet.count = 10;
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
		int roundToPlay = FindObjectOfType<MenuManager>().RoundToPlay;
		if (roundToPlay > 0)
		{
			level = roundToPlay - 1;
		}

		if (!exitTap)
		{
			AdvanceLevel();
		}

		System.GC.Collect();
	}

	private void Update()
	{
		if (!gameOver)
		{
			//Round Won
			if (spawner.ContainsKey(nameof(Crasher)) ? !spawner["Crasher"].isSpawning : true && Enemy.activeCount == 0)
			{
				roundWon = true;
				ui.centerText.text = "Round Clear!";
				sfx.PlayOneShot(winSfx, 0.7f);
				tvScreen.CloseTV();
				ClearScene();
				ui.buttonText.text = "Tap to continue";
			}

			//Defeat
			else if (player.Health == 0 || (Bullet.ActiveCount == 0 && Bullet.count == 0 && Enemy.activeCount > 0))
			{
				ui.centerText.text = "Rounds Survived : " + (level - 1);
				ClearScene();
				ui.buttonText.text = "Tap to continue";
			}
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
				if (Bullet.count > 0 && Time.time > nextFire)
				{
					nextFire = Time.time + ShotCooldown;
					Instantiate(bullet, Vector3.zero, Camera.main.transform.rotation);
				}
			}

			//Tap To Continue
			if (gameOver)
			{
				//Defeat, tap to restart
				if (Bullet.count == 0 || player.Health <= 0)
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
					Bullet.count += 6;
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
		foreach (var spawner in spawner)
		{
			// Spawn Patterns
			switch (spawner.Key)
			{
				case "Crasher":
					spawner.Value.StartSpawning(4 * level + 8);
					break;
				case "Drone":
					spawner.Value.StartSpawning(3 * level + 6);
					break;
				case "Capsule":
					spawner.Value.StartSpawning(level + 2);
					break;
			}
		}
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

		foreach (Spawner spawner in spawner.Values)
		{
			spawner.StopSpawning();
		}

		SpawnableObject[] objects = FindObjectsOfType<SpawnableObject>();
		foreach (SpawnableObject o in objects)
		{
			Destroy(o.gameObject);
		}
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