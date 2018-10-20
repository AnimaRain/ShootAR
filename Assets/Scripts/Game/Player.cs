using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(AudioSource))]
	public class Player : MonoBehaviour
	{
		//Set here how much health the player is allowed to have.
		public const sbyte MAXIMUM_HEALTH = 6;
		private const float SHOT_COOLDOWN = 0.50f;

		[SerializeField]
		private GameObject[] healthIndicator = new GameObject[MAXIMUM_HEALTH];

		[Range(0, MAXIMUM_HEALTH), SerializeField]
		private int health;
		[Range(0, 999), SerializeField]
		private int ammo;
		private float nextFire;

		[SerializeField] private GameState gameState;
		/// <summary>
		/// The <see cref="Bullet"/> prefab that gets instantiated when the player shoots.
		/// </summary>
		/// <seealso cref="Shoot"/>
		[SerializeField] private Bullet bullet;
		private AudioSource audioSource;
		[SerializeField] private AudioClip shotSfx;
		[SerializeField] private UnityEngine.UI.Text bulletCount;

		/// <summary>
		/// Player's health.
		/// If heatlh drops to zero, the game is over.
		/// </summary>
		public int Health
		{
			get { return health; }

			set
			{
				health = Mathf.Clamp(value, 0, MAXIMUM_HEALTH);
				if (health == 0 && gameState != null)
					gameState.GameOver = true;
				UpdateHealthUI();
			}
		}

		private void UpdateHealthUI()
		{
			if (healthIndicator[0] == null) return;

			for (int i = 0; i < MAXIMUM_HEALTH; i++)
			{
				healthIndicator[i].SetActive(i < health);
			}
		}

		/// <summary>
		/// The ammount of bullets the player has.
		/// </summary>
		/// <remarks>
		/// Setting this, also sets the bullet count on the UI.
		/// </remarks>
		public int Ammo
		{
			get { return ammo; }
			set {
				ammo = value;
				if (bulletCount != null)
					bulletCount.text = ammo.ToString();
			}
		}

		public bool HasArmor { get; set; }
		public bool CanShoot { get; set; }

		public static Player Create(
			int health = MAXIMUM_HEALTH, Camera camera = null,
			Bullet bullet = null, int ammo = 0, GameState gameState = null)
		{
			var o = new GameObject(nameof(Player)).AddComponent<Player>();

			var healthUI = new GameObject("HealthUI").transform;
			for (int i = 0; i < MAXIMUM_HEALTH; i++)
			{
				o.healthIndicator[i] = new GameObject("HealthIndicator");
				o.healthIndicator[i].transform.parent = healthUI;
			}
			o.Health = health;
			o.Ammo = ammo;
			o.bullet = bullet;
			o.gameState = gameState;
			if (camera != null) camera.tag = "MainCamera";
			else if (bullet != null)
			{
				Debug.LogWarning("No reference to main camera. Shooting" +
					" functions will raise error if used.");
			}

			return o;
		}

		/// <summary>
		/// Instantiate a bullet if able, there is enough ammo and the cooldown
		/// has expired.
		/// </summary>
		/// <returns>
		/// a reference to the bullet fired or null if conditions are not met
		/// </returns>
		public Bullet Shoot()
		{
			if (CanShoot && Ammo > 0 && Time.time >= nextFire)
			{
				Ammo--;
				nextFire = Time.time + SHOT_COOLDOWN;
				if (shotSfx != null) audioSource.PlayOneShot(shotSfx);
				return Instantiate(bullet, Vector3.zero, Camera.main.transform.rotation);
			}

			return null;
		}

		private void Start()
		{
			audioSource = GetComponent<AudioSource>();
			if (bulletCount != null)
				bulletCount.text = Ammo.ToString();
			CanShoot = true;
			UpdateHealthUI();
		}

		/// <summary>
		/// Player's health is reduced by <paramref name="damage"/>.
		/// If health vanishes, GameOver state is set.
		/// </summary>
		/// <param name="damage">the amount by which health is reduced</param>
		/// <seealso cref="GameState.GameOver"/>
		public void GetDamaged(int damage)
		{
			if (damage < 0) return;

			if (HasArmor)
			{
				HasArmor = false;
				return;
			}

			Health -= damage;
		}

		private void OnDestroy()
		{
			foreach (var h in healthIndicator)
			{
				Destroy(h.gameObject);
			}
		}
	}
}