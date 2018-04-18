using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public delegate void GuiUpdateHandler();
	public event GuiUpdateHandler UpdateGui;

	[SerializeField] private GameObject uiCanvas;
	[SerializeField] private GameObject pauseCanvas;
	public Text bulletCountText;
	public Text centerText;
	public Text buttonText;
	public Text scoreText;
	public Text roundText;

	private AudioSource sfx;
	[SerializeField]
	private AudioClip pauseSfx;

	private GameManager gameManager;

	public void Start()
	{

		gameManager = FindObjectOfType<GameManager>();

		//Create the audio source for pausing
		if (pauseSfx != null)
		{
			sfx = gameObject.AddComponent<AudioSource>();
			sfx.clip = pauseSfx;
			sfx.volume = 1f;
		}
		else
		{
			Debug.LogWarning("Pause audio-clip has not been assigned.");
		}

	}

	public void Update()
	{
		UpdateGui?.Invoke();
	}

	public void TogglePauseMenu()
	{
		sfx.PlayOneShot(pauseSfx, 1f);

		// not the optimal way but for the sake of readability
		if (gameObject.activeSelf)
		{
			roundText.text = "Round : " + gameManager.level;
			uiCanvas.SetActive(false);
			pauseCanvas.SetActive(true);
			Time.timeScale = 0f;
		}
		else
		{
			uiCanvas.SetActive(true);
			pauseCanvas.SetActive(false);
			Time.timeScale = 1.0f;
		}

		Debug.Log("UIMANAGER:: TimeScale: " + Time.timeScale);
	}
}