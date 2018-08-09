/* TODO: Check healthIndicator if any changes are needed; I'm not quite happy with 
 * the way it is right now. Maybe it should be declared readonly. */

using UnityEngine;

namespace ShootAR
{
	public class Player : MonoBehaviour
	{
		//Set here how much health the player is allowed to have.
		public const sbyte HEALTH_MAX = 3;

		private const float ShotCooldown = 0.35f;

		[SerializeField]
		private GameObject[] healthIndicator = new GameObject[HEALTH_MAX];

		[Range(0, HEALTH_MAX), SerializeField]
		private int health;
		[Range(0, Mathf.Infinity), SerializeField]
		private int ammo;
		private float nextFire;

		private GameManager gameManager;
		/// <summary>
		/// The bullet prefab that gets cloned when the player shoots.
		/// </summary>
		[SerializeField] private readonly Bullet bullet;

		/// <summary>
		/// Player's health.
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
				if (HasArmor && value < 0)
				{
					value = 0;
					HasArmor = false;
				}
				health = Mathf.Clamp(value, 0, HEALTH_MAX);
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

		public int Ammo
		{
			get { return ammo; }
			set
			{
				ammo = value;
				CanShoot = ammo <= 0;
			}
		}

		public bool HasArmor { get; set; }
		public bool CanShoot { get; set; }

		public static Player Create(int health)
		{
			var o = new GameObject().AddComponent<Player>();
			for (int i = 0; i < HEALTH_MAX; i++)
				o.healthIndicator[i] = new GameObject("HealthIndicator");
			o.Health = health;
			return o;
		}

		public Bullet Shoot()
		{
			if (Bullet.Count > 0 && Time.time > nextFire)
			{
				nextFire = Time.time + ShotCooldown;
				return Instantiate(bullet, Vector3.zero, Camera.main.transform.rotation);
			}

			return null;
		}

		private void Start()
		{
			gameManager = FindObjectOfType<GameManager>();

			UpdateHealthUI();
		}
	}
}