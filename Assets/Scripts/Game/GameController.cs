/* TODO: Check if roundWon and gameOver conditions are used in the correct
 * places. */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	#region Definitions

	public GameObject UICanvas;
	public GameObject PauseCanvas;
	[HideInInspector]
	public WebCamTexture Cam;   //Rear Camera
	[HideInInspector]
	public bool arReady;
	public Button FireButton;
	public Bullet Bullet;
	public Text CountText;
	public Text CenterText;
	public Text ButtonText;
	public Text ScoreText;
	public Text RoundText;
	[HideInInspector]
	public int Level;
	public AudioClip WinSfx, PauseSfx;
	private Dictionary<string, Spawner> Spawner;
	private int Score;
	private TVScript TVScreen;

	[HideInInspector]
	public bool roundWon;
	[HideInInspector]
	public bool gameOver;
	private bool exitTap;
	private Player player;
	private AudioSource sfx;

	private const float ShotCooldown = 0.35f;

	private float nextFire;

	#endregion


	private void Awake()
	{
#if UNITY_ANDROID

		//Check if we support both devices
		//Gyroscope
		if (!SystemInfo.supportsGyroscope)
		{
			Debug.Log("This device does not have Gyroscope");
			CenterText.text = "This device does not have Gyroscope!";
			ButtonText.text = "Tap to exit";
			exitTap = true;
		}

		//Enable gyro
		Input.gyro.enabled = true;

		//Back Camera
		for (int i = 0; i < WebCamTexture.devices.Length; i++)
		{
			if (!WebCamTexture.devices[i].isFrontFacing)
			{

				Cam = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
				break;
			}
		}

#endif

#if UNITY_EDITOR_WIN
		Cam = new WebCamTexture();
#endif

		//If we did not find a back camera,exit
		if (Cam == null)
		{
			Debug.Log("This device does not have a rear camera");
			CenterText.text = "This device does not have a rear camera!";
			ButtonText.text = "Tap to exit";
			exitTap = true;
		}

		/* Create a dictionary of all spawners by setting the name of
         * their assigned ObjectToSpawn as a key and the spawner itself
         * as the value.*/
		Spawner = new Dictionary<string, Spawner>();
		Spawner[] spawners = GameObject.Find("Spawners").GetComponents<Spawner>();
		if (spawners == null)
		{
			Debug.Log("Could not find Object \"Spawners\" or no Spawner scripts where attached to it.");
		}
		else
		{
#if DEBUG
			Debug.Log(string.Format("spawners #START#\nLength:{0}\n", spawners.Length));
			for (int i = 0; i < spawners.Length; i++)
			{
				Debug.Log(string.Format("FIELD: {2}\tKEY: {0}\tVALUE: {1}", i, spawners[i], spawners));
			}
			Debug.Log("spawners #END#");
#endif
			foreach (Spawner spawner in spawners)
			{
				string type = spawner.ObjectToSpawn.name;
#if DEBUG
				Debug.Log(string.Format("DICTIONARY \"Spawners\":\tKEY: {0}\tVALUE: {1}\nTYPE_CHECK: {2}", type, spawner, spawner.ObjectToSpawn.name));
#endif
				Spawner.Add(type, spawner);
			}
		}

		player = GameObject.Find("Player").GetComponent<Player>();
		if (player == null)
			Debug.Log("Player object not found");

		sfx = new AudioSource();
		Bullet.Count = 10;
		gameOver = false;
		arReady = true;
	}

	private void Start()
	{
		if (TVScreen == null) TVScreen = GameObject.Find("TVScreen").GetComponent<TVScript>();
		ButtonText.text = "";
		CenterText.text = "";
		CountText.text = "";
		FireButton.onClick.AddListener(OnButtonDown);
		if (ButtonsScript.RoundToPlay != 0)
		{
			Level = ButtonsScript.RoundToPlay - 1;
			ButtonsScript.RoundToPlay = 0;
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
			if (Spawner["Crasher"].SpawnCount == Spawner["Crasher"].SpawnLimit && Enemy.ActiveCount == 0)
			{
				roundWon = true;
				gameOver = true;
				CenterText.text = "Round Clear!";
				sfx.PlayOneShot(WinSfx, 0.7f);
				TVScreen.CloseTV();
				ClearLevel();
				ButtonText.text = "Tap to continue";
			}

			//Defeat
			else if (player.Health == 0 || (Bullet.ActiveCount == 0 && Bullet.Count == 0 && Enemy.ActiveCount > 0))
			{
				CenterText.text = "Rounds Survived : " + (Level - 1);
				gameOver = true;
				ClearLevel();
				ButtonText.text = "Tap to continue";
			}
		}
	}

	public void OnApplicationQuit()
	{
#if UNITY_EDITOR_WIN
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}


	private void OnButtonDown()
	{
		if (exitTap == false)
		{
			//Fire Bullet
			if (!gameOver)
			{
				if (Bullet.Count > 0 && Time.time > nextFire)
				{
					nextFire = Time.time + ShotCooldown;
					Instantiate(Bullet, Vector3.zero, Camera.main.transform.rotation);
				}
			}

			//Tap To Continue
			if (gameOver)
			{
				//Defeat, tap to restart
				if (Bullet.Count == 0 || player.Health == 0)
				{
					Cam.Stop();
					SceneManager.LoadScene("FreakyTVGame");
				}
				//Next Round tap
				else
				{
					gameOver = false;
					CenterText.text = "";
					ButtonText.text = "";
					Bullet.Count += 6;
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
		Level++;
		TVScreen.Invoke("StartTV", 10);
		foreach (var spawner in Spawner)
		{
			spawner.Value.SpawnCount = 0;
			spawner.Value.StartCoroutine("Spawn");

			// Spawn Patterns
			switch (spawner.Key.ToString())
			{
				case "Crasher":
					spawner.Value.SpawnLimit = 4 * Level + 8;
					break;
				case "Drone":
					spawner.Value.SpawnLimit = 3 * Level + 6;
					break;
				case "Capsule":
					spawner.Value.SpawnLimit = Level + 2;
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
		if (ScoreText != null)
		{
			Score += points;
			ScoreText.text = "Score: " + Score;
		}
	}

	/// <summary>
	/// Deactivates spawners, destroys all spawned objects.
	/// Mainly used when a round ends.
	/// </summary>
	private void ClearLevel()
	{
		foreach (Spawner spawner in Spawner.Values)
			spawner.StopCoroutine("Spawn");

		Object[] objects = FindObjectsOfType<SpawnableObject>();
		foreach (Object o in objects)
		{
			Destroy(GameObject.Find(o.name));
		}
	}

	public void TogglePauseMenu()
	{
		sfx.PlayOneShot(PauseSfx, 1f);

		// not the optimal way but for the sake of readability
		if (UICanvas.activeSelf)
		{
			RoundText.text = "Round : " + Level;
			UICanvas.SetActive(false);
			PauseCanvas.SetActive(true);
			Time.timeScale = 0f;
		}
		else
		{
			UICanvas.SetActive(true);
			PauseCanvas.SetActive(false);
			Time.timeScale = 1.0f;
		}

		Debug.Log("GAMEMANAGER:: TimeScale: " + Time.timeScale);
	}

	public void GoToMenu()
	{
		Cam.Stop();
		SceneManager.LoadScene("MainMenu");
	}

}