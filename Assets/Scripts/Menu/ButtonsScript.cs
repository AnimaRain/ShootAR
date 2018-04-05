using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonsScript : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject startMenu;
    public GameObject roundMenu;
    public static int roundToPlay;

    public void ToStartMenu()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void GameStart(int round)
    {
        roundToPlay = round;
        SceneManager.LoadScene("FreakyTVGame");
    }

    public void ToRoundSelect()
    {
        mainMenu.SetActive(false);
        roundMenu.SetActive(true);
    }

    public void ToCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void ToMenu()
    {
        creditsMenu.SetActive(false);
        startMenu.SetActive(false);
        roundMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitApp()
    {
    #if UNITY_EDITOR_WIN
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
