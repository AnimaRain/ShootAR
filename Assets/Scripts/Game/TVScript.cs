using UnityEngine;

public class TvScript : MonoBehaviour
{

	[SerializeField]
	private Material blackScreen, staticScreen;
	private int tvRefreshTime;
	public bool tvOn;

	private GameManager gameManager;
	

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		Invoke("StartTV", tvRefreshTime);
	}

	private void Update()
	{
		//TODO: while not game over, if turned off, turn on.
	}

	public void CloseTv()
	{
		GetComponent<Renderer>().material = blackScreen;
		CancelInvoke("StartTV");
		if (!gameManager.gameOver)
			Invoke("StartTV", tvRefreshTime);
		tvOn = false;
	}

	public void StartTv()
	{
		GetComponent<Renderer>().material = staticScreen;
		CancelInvoke("StartTV");
		tvOn = true;
	}
}
