using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{
	public class UI : MonoBehaviour
	{
		public delegate void GuiUpdateHandler();
		public event GuiUpdateHandler UpdateGui;

		[SerializeField] private GameObject uiCanvas;
		[SerializeField] private GameObject pauseCanvas;
		[SerializeField] private Text bulletCount;
		[SerializeField] private Text messageOnScreen;
		[SerializeField] private Text score;
		[SerializeField] private Text roundIndex;

		private AudioSource sfx;
		[SerializeField]
		private AudioClip pauseSfx;

		private GameState gameState;

		public Text BulletCount
		{
			get { return bulletCount; }
			set { bulletCount = value; }
		}

		public Text MessageOnScreen
		{
			get { return messageOnScreen; }
			set { messageOnScreen = value; }
		}

		public Text Score
		{
			get { return score; }
			set { score = value; }
		}

		public Text RoundIndex
		{
			get { return roundIndex; }
			set { roundIndex = value; }
		}

		public static UI Create(
				GameObject uiCanvas, GameObject pauseCanvas,
				Text bulletCount, Text messageOnScreen,
				Text score, Text roundIndex,
				AudioSource sfx, AudioClip pauseSfx,
				GameState gameState)
		{
			var o = new GameObject(nameof(UI)).AddComponent<UI>();

			o.uiCanvas = uiCanvas;
			o.pauseCanvas = pauseCanvas;
			o.bulletCount = bulletCount;
			o.MessageOnScreen = messageOnScreen;
			o.Score = score;
			o.RoundIndex = roundIndex;
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
				RoundIndex.text = "Round: " + gameState.Level;
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