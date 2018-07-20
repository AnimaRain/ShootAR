/* TODO: Check healthIndicator if any changes are needed; I'm not quite happy with 
 * the way it is right now. Maybe it should be declared readonly. */

using UnityEngine;

namespace ShootAR
{
	public class Player : MonoBehaviour
	{
		public const sbyte HEALTH_MAX = 3;
		[SerializeField]
		private GameObject[] healthIndicator = new GameObject[HEALTH_MAX];

		[Range(0, HEALTH_MAX), SerializeField]
		private int health;

		private GameManager gameManager;

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
				health = Mathf.Clamp(value, 0, 6);
				if (health == 0) gameManager.gameOver = true;
				UpdateHealthUI();
			}
		}

		private void UpdateHealthUI()
		{
			for (int i = 0; i < HEALTH_MAX; i++)
			{
				healthIndicator[i].SetActive(i < health);
			}
		}

		public static Player Create(int health)
		{
			var o = new GameObject().AddComponent<Player>();
			for (int i = 0; i < HEALTH_MAX; i++)
				o.healthIndicator[i] = new GameObject("HealthIndicator");
			o.Health = health;
			return o;
		}

		private void Start()
		{
			gameManager = FindObjectOfType<GameManager>();

			UpdateHealthUI();
		}
	}
}