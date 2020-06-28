using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ShootAR
{
	[RequireComponent(typeof(AudioSource), typeof(CapsuleCollider))]
	public class Player : MonoBehaviour
	{
		/// <summary>Maximum allowed health for player</summary>
		public const sbyte MAXIMUM_HEALTH = 6;
		private const float SHOT_COOLDOWN = 0.50f;
		private const float DAMAGE_COOLDOWN = 1f;

		/// <summary>
		/// For how many seconds the amount of restored
		/// bullets will float above the counter.
		/// </summary>
		private const float BULLET_PLUS_FLOAT_TIME = 3f;

		[SerializeField]
		private GameObject[] healthIndicator = new GameObject[MAXIMUM_HEALTH];

		[Range(0, MAXIMUM_HEALTH), SerializeField]
		private int health;
		[Range(0, 999), SerializeField]
		private int ammo;

		private float nextFire;
		private float nextDamage;

		[SerializeField] private GameState gameState;
		private AudioSource audioSource;
#pragma warning disable CS0649
		[SerializeField] private AudioClip shotSfx;
		[SerializeField] private Text bulletCount;
		[SerializeField] private Text bulletPlus;
#pragma warning restore CS0649

		/// <summary>
		/// Player's health.
		/// If heatlh drops to zero, the game is over.
		/// </summary>
		public int Health {
			get { return health; }

			set {
				health = Mathf.Clamp(value, 0, MAXIMUM_HEALTH);
				if (health == 0 && gameState != null)
					gameState.GameOver = true;
				UpdateHealthUI();
			}
		}

		private void UpdateHealthUI() {
			if (healthIndicator[0] == null) return;

			for (int i = 0; i < MAXIMUM_HEALTH; i++) {
				healthIndicator[i].SetActive(i < health);
			}
		}

		/// <summary>
		/// The ammount of bullets the player has.
		/// Setting this also sets the bullet count on the UI.
		/// </summary>
		public int Ammo {
			get { return ammo; }
			set {
				// pop up a number showing player how many bullets they got
				if (bulletPlus != null && value - ammo > 0) {
					bulletPlus.text = $"+{value - ammo}";

					IEnumerator FadeBulletPlus() {
						bulletPlus.gameObject.SetActive(true);
						bulletPlus.CrossFadeAlpha(1f, 0f, true);

						yield return new WaitForSecondsRealtime(BULLET_PLUS_FLOAT_TIME);

						do {
							bulletPlus.CrossFadeAlpha(0f, 5f, true);
							yield return new WaitForFixedUpdate();
						} while (bulletPlus.color.a != 0f);

						bulletPlus.gameObject.SetActive(false);
					}
					StartCoroutine(FadeBulletPlus());
				}

				ammo = value;
				if (bulletCount != null)
					bulletCount.text = ammo.ToString();
			}
		}

		public bool HasArmor { get; set; } = false;
		public bool CanShoot { get; set; } = true;

		public static Player Create(
			int health = MAXIMUM_HEALTH, Camera camera = null,
			int ammo = 0, GameState gameState = null) {
			var o = new GameObject(nameof(Player)).AddComponent<Player>();

			var healthUI = new GameObject("HealthUI").transform;
			for (int i = 0; i < MAXIMUM_HEALTH; i++) {
				o.healthIndicator[i] = new GameObject("HealthIndicator");
				o.healthIndicator[i].transform.parent = healthUI;
			}
			o.Health = health;
			o.Ammo = ammo;
			o.gameState = gameState;
			if (camera != null) camera.tag = "MainCamera";

			CapsuleCollider collider = o.GetComponent<CapsuleCollider>();
			collider.radius = 0.5f;
			collider.height = 1.9f;
			collider.isTrigger = true;

			return o;
		}

		/// <summary>
		/// Instantiate a bullet if able, there is enough ammo and the cooldown
		/// has expired.
		/// </summary>
		/// <returns>
		/// a reference to the bullet fired or null if conditions are not met
		/// </returns>
		public Bullet Shoot() {
			if (CanShoot && Ammo > 0 && Time.time >= nextFire) {
				Bullet bullet = Spawnable.Pool<Bullet>.Instance.RequestObject();
				if (bullet is null) return null;

				Ammo--;
				nextFire = Time.time + SHOT_COOLDOWN;
				if (shotSfx != null) audioSource.PlayOneShot(shotSfx);
				bullet.transform.position = Vector3.zero;
				bullet.transform.rotation = Camera.main.transform.rotation;
				bullet.gameObject.SetActive(true);
				return bullet;
			}

			return null;
		}

		private void Start() {
			audioSource = GetComponent<AudioSource>();
			if (bulletCount != null)
				bulletCount.text = Ammo.ToString();
			UpdateHealthUI();
		}

		/// <summary>
		/// Player's health is reduced by <paramref name="damage"/>.
		/// If health vanishes, GameOver state is set.
		/// </summary>
		/// <param name="damage">the amount by which health is reduced</param>
		/// <seealso cref="GameState.GameOver"/>
		public void GetDamaged(int damage) {
			if (damage <= 0 || Time.time < nextDamage) return;
			nextDamage = Time.time + DAMAGE_COOLDOWN;

			if (HasArmor) {
				HasArmor = false;
				return;
			}

			Health -= damage;
		}

		private void OnDestroy() {
			foreach (var h in healthIndicator) {
				Destroy(h.gameObject);
			}
		}
	}
}
