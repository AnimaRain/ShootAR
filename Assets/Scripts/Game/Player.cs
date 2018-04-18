using UnityEngine;

public class Player : MonoBehaviour
{
	public const sbyte HealthMax = 6;
	[SerializeField]
	private readonly GameObject[] healthIndicator = new GameObject[HealthMax];

	[Range(0, HealthMax),SerializeField]
	private int health;

	private GameManager gameManager;

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager>();

		UpdateHealthUI();
	}

	/// <summary>
	/// Player's health. It 
	/// If heatlh reaches zero, the game is over.
	/// </summary>
	public int Health
	{

		get
		{
			return health;
		}

		set
		{
			health = value;
			if (health <= 0)
			{
				health = 0;
				gameManager.gameOver = true;
			}
			else if (health > 6)
				health = 6;

			UpdateHealthUI();
		}
	}

	public void UpdateHealthUI()
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
