using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{
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

		private GameState gameState;

		public static UIManager Create(
				GameObject uiCanvas, GameObject pauseCanvas,
				Text bulletCountText, Text centerText, Text buttonText,
				Text scoreText, Text roundText,
				AudioSource sfx, AudioClip pauseSfx,
				GameState gameState)
		{
			var o = new GameObject(nameof(UIManager)).AddComponent<UIManager>();

			o.uiCanvas = uiCanvas;
			o.pauseCanvas = pauseCanvas;
			o.bulletCountText = bulletCountText;
			o.centerText = centerText;
			o.buttonText = buttonText;
			o.scoreText = scoreText;
			o.roundText = roundText;
			o.sfx = sfx;
			o.pauseSfx = pauseSfx;
			o.gameState = gameState;

			return o;
		}

		public void Start()
		{

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
				roundText.text = "Round : " + gameState.Level;
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
}