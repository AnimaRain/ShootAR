using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameObject uiCanvas;
		[SerializeField] private GameObject pauseCanvas;
		[SerializeField] private Text bulletCount;
		[SerializeField] private Text bulletPlus;
		[SerializeField] private Text messageOnScreen;
		[SerializeField] private Text score;
		[SerializeField] private Text roundIndex;
		[SerializeField] private GameState gameState;
		private AudioSource sfx;
		[SerializeField] private AudioClip pauseSfx;
#pragma warning disable CS0649
		[SerializeField] private Button pauseToMenuButton;
#pragma warning restore CS0649

		public Text BulletCount {
			get { return bulletCount; }
			set { bulletCount = value; }
		}

		public Text BulletPlus {
			get { return bulletPlus; }
			set { bulletPlus = value; }
		}

		public Text MessageOnScreen {
			get { return messageOnScreen; }
			set { messageOnScreen = value; }
		}

		public Text Score {
			get { return score; }
			set { score = value; }
		}

		public Text RoundIndex {
			get { return roundIndex; }
			set { roundIndex = value; }
		}

		[SerializeField] private NameAsker nameAsker;

		public static UIManager Create(
				GameObject uiCanvas, GameObject pauseCanvas,
				Text bulletCount, Text bulletPlus,
				Text messageOnScreen,
				Text score, Text roundIndex,
				AudioSource sfx, AudioClip pauseSfx,
				GameState gameState) {
			var o = new GameObject(nameof(UIManager)).AddComponent<UIManager>();

			o.uiCanvas = uiCanvas;
			o.pauseCanvas = pauseCanvas;
			o.bulletCount = bulletCount;
			o.bulletPlus = bulletPlus;
			o.MessageOnScreen = messageOnScreen;
			o.Score = score;
			o.RoundIndex = roundIndex;
			o.sfx = sfx;
			o.pauseSfx = pauseSfx;
			o.gameState = gameState;

			return o;
		}

		public void Start() {

			if (pauseSfx != null) {
				sfx = gameObject.AddComponent<AudioSource>();
				sfx.clip = pauseSfx;
				sfx.volume = 1f;
			}
			else Debug.LogWarning("Pause audio-clip has not been assigned.");

			pauseToMenuButton?.onClick.AddListener(() => {
				gameState.Paused = false;
				UnityEngine.SceneManagement.SceneManager
					.LoadScene(0);
			});
		}

		public void TogglePauseMenu() {
			sfx.PlayOneShot(pauseSfx, 1f);

			if (!pauseCanvas.gameObject.activeSelf) {
				RoundIndex.text = "Round: " + gameState.Level;
				uiCanvas.SetActive(false);
				pauseCanvas.SetActive(true);
				gameState.Paused = true;
			}
			else {
				uiCanvas.SetActive(true);
				pauseCanvas.SetActive(false);
				gameState.Paused = false;
			}
#if DEBUG
			Debug.Log("UIMANAGER:: TimeScale: " + Time.timeScale);
#endif
		}

		/// <summary>
		/// Returns player's input through a callback.
		///
		/// Waits until player inputs a name through the ui,
		/// and uses that name inside <paramref name="nameReturn"/>.
		/// </summary>
		/// <param name="nameReturn">
		/// the callback inside where the name input is used
		/// </param>
		///
		/// <example>
		/// Ask a name from the player (in this case, the passed callback
		/// function just assigns the returned name to out local variable):
		/// <code>
		/// string playerName = "";
		/// StartCoroutine(ui.AskName(name => playerName = name));
		/// </code>
		/// We can now use the returned name however we want:
		/// <code>
		/// highscores.AddScore(playerName, score);
		/// </code>
		/// </example>
		public IEnumerator AskName(Action<string> nameReturn) {
			nameAsker.gameObject.SetActive(true); // set nameAsker in motion

			yield return new WaitWhile(() => nameAsker.PendingQuery);

			nameReturn(nameAsker.InputName);
		}
	}
}
