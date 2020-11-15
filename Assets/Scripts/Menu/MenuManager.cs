using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

namespace ShootAR.Menu
{
	/// <remarks>
	/// Most functions of this class are assigned as events on buttons
	/// through the Inspector in the Editor.
	/// </remarks>
	[RequireComponent(typeof(AudioSource))]
	public class MenuManager : MonoBehaviour
	{

		private AudioSource sfx;

		[SerializeField] private GameObject mainMenu;
		[SerializeField] private GameObject subMenu;
		[SerializeField] private GameObject creditsMenu;
		[SerializeField] private GameObject startMenu;
		[SerializeField] private GameObject waveEditorMenu;
		[SerializeField] private GameObject highscoreMenu;
		[SerializeField] private AudioClip select;
		[SerializeField] private AudioClip back;


		private void Awake() {
#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.unityLogger.logEnabled = false;
#endif
		}

		private void Start() {
			Configuration.Instance.CreateFiles();

			Application.runInBackground = false;

			AudioListener.volume = Configuration.Instance.SoundMuted ? 0f : Configuration.Instance.Volume;

			sfx = gameObject.GetComponent<AudioSource>();
		}

		public void ToStartMenu() {
			mainMenu.SetActive(false);
			subMenu.SetActive(true);
			startMenu.SetActive(true);

			sfx.PlayOneShot(select, 1.2F);
		}

		public void StartGame() {
			SceneManager.LoadScene(1);
		}

		public void ToWaveEditor() {
			mainMenu.SetActive(false);
			subMenu.SetActive(true);
			waveEditorMenu.SetActive(true);

			sfx.PlayOneShot(select, 1.2F);
		}

		public void ToCredits() {
			mainMenu.SetActive(false);
			subMenu.SetActive(true);
			creditsMenu.SetActive(true);
		}

		public void ToHighscores() {
			mainMenu.SetActive(false);
			subMenu.SetActive(true);
			highscoreMenu.SetActive(true);
		}

		public void ToMainMenu() {
			highscoreMenu.SetActive(false);
			creditsMenu.SetActive(false);
			startMenu.SetActive(false);
			waveEditorMenu.SetActive(false);
			subMenu.SetActive(false);
			mainMenu.SetActive(true);

			sfx.PlayOneShot(back, 1.5F);
		}

		public void QuitApp() {
			if (Configuration.Instance.UnsavedChanges)
				Configuration.Instance.SaveSettings();

			Application.Quit();

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
	}
}
