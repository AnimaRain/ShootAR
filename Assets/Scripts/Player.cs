using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	[Range(0, 6)]
	private int Health;

	private GameController gameController;

	private void Awake()
	{
		gameController = GetComponent<GameController>();
	}

	/// <summary>
	/// Player's health decreases by the designated amount.
	/// If heatlh reaches zero, the game is over. Negative value restores.
	/// </summary>
	public void LoseHealth(int damage)
	{
		Health -= damage;

		if (Health <= 0)
		{
			gameController.gameOver = true;
		}
	}
}
