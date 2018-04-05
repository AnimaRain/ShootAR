using UnityEngine;

public class Player : MonoBehaviour
{
	public const byte HealthMax = 6;

	[Range(0, HealthMax)]
	public int health;
	[SerializeField]
	private readonly GameObject[] healthIndicator = new GameObject[HealthMax];

	private GameManager gameManager;

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager>();

		UpdateHealth();
	}

	/// <summary>
	/// Player's health decreases by the designated amount.
	/// If heatlh reaches zero, the game is over. Negative value restores.
	/// </summary>
	public void LoseHealth(int damage)
	{
		health -= damage;
		UpdateHealth();

		if (health <= 0)
		{
			gameManager.gameOver = true;
		}
	}

	public void UpdateHealth()
	{
		for (int i = 0; i < healthIndicator.Length; i++)
		{
			/*TODO: Take note -> the if-statement was changed to the boolean parameter.
			 * got rid of the if-else and replaced it with a single line.
			 */

			healthIndicator[i].SetActive(i < health);
		}
	}
}
