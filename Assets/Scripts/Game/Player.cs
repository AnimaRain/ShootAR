/* TODO: Check healthIndicator if any changes are needed; I'm not quite happy with 
 * the way it is right now. Maybe it should be declared readonly. */

using UnityEngine;

namespace ShootAR
{
	public class Player : MonoBehaviour
	{
		public const sbyte HealthMax = 6;
		[SerializeField]
		private GameObject[] healthIndicator = new GameObject[HealthMax];

		[Range(0, HealthMax), SerializeField]
		private int health;

		private GameManager gameManager;

		private void Start()
		{
			gameManager = FindObjectOfType<GameManager>();

			UpdateHealthUI();
		}

		/// <summary>
		/// Player's health. (range: 0-6)
		/// If heatlh drops to zero, the game is over.
		/// </summary>
		public int Health
		{

			get
			{
				return health;
			}

			set
			{
				health = Mathf.Clamp(health + value, 0, 6);
				if (health == 0) gameManager.gameOver = true;
				UpdateHealthUI();
			}
		}

		public void UpdateHealthUI()
		{
			for (int i = 0; i < healthIndicator.Length; i++)
			{
				/* TODO: Take note -> the if-statement was changed to the boolean
				 * parameter. Got rid of the if-else and replaced it with a single line.
				 */

				healthIndicator[i].SetActive(i < health);
			}
		}
	}
}