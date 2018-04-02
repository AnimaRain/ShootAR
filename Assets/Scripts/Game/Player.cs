using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public const System.Byte HEALTH_MAX = 6;

	[Range(0, HEALTH_MAX)]
	public int Health;

	private GameController gameController;
	[SerializeField]
	private GameObject[] healthIndicator = new GameObject[HEALTH_MAX];

	private void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		UpdateHealth();
	}

	/// <summary>
	/// Player's health decreases by the designated amount.
	/// If heatlh reaches zero, the game is over. Negative value restores.
	/// </summary>
	public void LoseHealth(int damage)
	{
		Health -= damage;
		UpdateHealth();

		if (Health <= 0)
		{
			gameController.gameOver = true;
		}
	}

	public void UpdateHealth()
	{
		for (int i = 0; i < healthIndicator.Length; i++)
		{
			if (i < Health)
			{
				healthIndicator[i].SetActive(true);
			}
			else
			{
				healthIndicator[i].SetActive(false);
			}
		}
	}
}
