using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootAR.Menu
{
	/// <remarks>
	/// Most functions of this class are assigned as events on buttons
	/// through the Inspector in the Editor.
	/// </remarks>
	public class MenuManager : MonoBehaviour
	{

		private AudioSource sfx;

		[SerializeField] private GameObject mainMenu;
		[SerializeField] private GameObject subMenu;
		[SerializeField] private GameObject creditsMenu;
		[SerializeField] private GameObject startMenu;
		[SerializeField] private GameObject roundMenu;
		[SerializeField] private AudioClip select;
		[SerializeField] private AudioClip back;
		[SerializeField] private MuteButton muteButton;


		private void Awake() {
#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.unityLogger.logEnabled = false;
#endif
		}

		private void Start() {
			Application.runInBackground = false;

			sfx = gameObject.AddComponent<AudioSource>();
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

		public void ToRoundSelect() {
			mainMenu.SetActive(false);
			subMenu.SetActive(true);
			roundMenu.SetActive(true);

			sfx.PlayOneShot(select, 1.2F);
		}

		public void ToCredits() {
			mainMenu.SetActive(false);
			subMenu.SetActive(true);
			creditsMenu.SetActive(true);
		}

		public void ToMainMenu() {
			creditsMenu.SetActive(false);
			startMenu.SetActive(false);
			roundMenu.SetActive(false);
			subMenu.SetActive(false);
			mainMenu.SetActive(true);

			sfx.PlayOneShot(back, 1.5F);
		}

		public void QuitApp() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
			Application.Quit();
		}
	}
}
