using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShootAR.Menu
{
	/// <remarks>
	/// Most functions of this class are assigned as events on buttons
	/// through the Inspector in the Editor.
	/// </remarks>
	public class MenuManager : MonoBehaviour
	{

		private AudioSource sfx;

		[SerializeField] private GameObject menu;
		[SerializeField] private GameObject subMenu;
		[SerializeField] private GameObject roundMenu;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject game;
        [SerializeField] private GameObject gameui;
        [SerializeField] private GameObject clouds;
        [SerializeField] private AudioClip select;
		[SerializeField] private AudioClip back;
        private bool toggleBool = false;

        void Start()
        {
            sfx = gameObject.AddComponent<AudioSource>();
            Screen.lockCursor = true;
        }

		public void StartGame() { 
            menu.SetActive(false);
            gameui.SetActive(true);
            Screen.lockCursor = false;
            game.SetActive(true);
        }

		public void ToRoundSelect() {
			mainMenu.SetActive(false);
			subMenu.SetActive(true);
			roundMenu.SetActive(true);

			sfx.PlayOneShot(select, 1.2F);
		}


		public void ToMainMenu() {
            roundMenu.SetActive(false);
			subMenu.SetActive(false);
            mainMenu.SetActive(true);
            sfx.PlayOneShot(back, 1.5F);
		}

        public void ActivateClouds()
        {
            toggleBool = !toggleBool;
            clouds.SetActive(toggleBool);
            Debug.LogError("There is the secret button");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }                
	}
}
