using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	private AudioSource sfx;

	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject subMenu;
	[SerializeField] private GameObject creditsMenu;
	[SerializeField] private GameObject startMenu;
	[SerializeField] private GameObject roundMenu;
	[SerializeField] private AudioClip select;
	[SerializeField] private AudioClip back;
	[SerializeField] private MuteButton muteButton;


	private void Start()
	{
		sfx = gameObject.AddComponent<AudioSource>();
	}

	public void ToStartMenu()
    {
        mainMenu.SetActive(false);
		subMenu.SetActive(true);
        startMenu.SetActive(true);

		sfx.PlayOneShot(select, 1.2F);
	}

    public void StartGame()
    {
		SceneManager.LoadScene(1);
    }

    public void ToRoundSelect()
    {
        mainMenu.SetActive(false);
		subMenu.SetActive(true);
		roundMenu.SetActive(true);
		DontDestroyOnLoad(gameObject);

		sfx.PlayOneShot(select, 1.2F);
	}

    public void ToCredits()
    {
        mainMenu.SetActive(false);
		subMenu.SetActive(true);
		creditsMenu.SetActive(true);
    }

    public void ToMainMenu()
    {
        creditsMenu.SetActive(false);
        startMenu.SetActive(false);
        roundMenu.SetActive(false);
		subMenu.SetActive(false);
		mainMenu.SetActive(true);

		sfx.PlayOneShot(back, 1.5F);
	}

    public void QuitApp()
    {
    #if UNITY_EDITOR_WIN
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
