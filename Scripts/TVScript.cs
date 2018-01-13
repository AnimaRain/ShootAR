using UnityEngine;

public class TVScript : MonoBehaviour {

    #region Definitions
    public Material BlackScreen;
    public Material StaticScreen;
    public int TVRefreshTime;
    public bool tvon;
    private static GameController gameController;
    #endregion

    private void Awake()
    {
        if (gameController == null)
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        Invoke("StartTV", TVRefreshTime);
    }

    public void CloseTV()
    {
        GetComponent<Renderer>().material = BlackScreen;
        CancelInvoke("StartTV");
        if (!gameController.gameOver)
        Invoke("StartTV", TVRefreshTime);
        tvon = false;
    }

    public void StartTV()
    {
        GetComponent<Renderer>().material = StaticScreen;
        CancelInvoke("StartTV");
        tvon = true;
    }
}
