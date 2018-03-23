using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonsScript : MonoBehaviour {

    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject StartMenu;
    public GameObject RoundMenu;
    public static int RoundToPlay;

    public void ToStartMenu()
    {
        MainMenu.SetActive(false);
        StartMenu.SetActive(true);
    }

    public void GameStart(int Round)
    {
        RoundToPlay = Round;
        SceneManager.LoadScene("FreakyTVGame");
    }

    public void ToRoundSelect()
    {
        MainMenu.SetActive(false);
        RoundMenu.SetActive(true);
    }

    public void ToCredits()
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void ToMenu()
    {
        CreditsMenu.SetActive(false);
        StartMenu.SetActive(false);
        RoundMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void QuitApp()
    {
    #if UNITY_EDITOR_WIN
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
