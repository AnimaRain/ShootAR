using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Parent class of all types of enemies.
	/// </summary>
	public class Enemy : MonoBehaviour, ISpawnable, IOrbiter
	{
		private EnemyController controller;
		//[SerializeField] private float speed;

		public EnemyController Controller { get { return controller; } }

		[SerializeField] protected AudioClip attackSfx;
		[SerializeField] protected GameObject explosion;

		protected AudioSource sfx;
		protected static GameManager gameManager;

		public static Enemy Create(float speed,
			float x = 0f, float y = 0f, float z = 0f)
		{
			var o = new GameObject("Enemy").AddComponent<Enemy>();
			o.controller = new EnemyController(speed);
			o.transform.position = new Vector3(x, y, z);
			return o;
		}

		protected void Awake()
		{
			activeCount++;
			count++;
		}

		protected virtual void Start()
		{
			//Create an audio source to play the audio clips
			sfx = gameObject.AddComponent<AudioSource>();
			sfx.clip = attackSfx;
			sfx.volume = 0.3f;
			sfx.playOnAwake = false;
			sfx.maxDistance = 10f;

			controller.Orbiter = this;

			if (gameManager != null) gameManager = FindObjectOfType<GameManager>();
		}

		protected virtual void OnDestroy()
		{
			if (!gameManager.gameOver)
			{
				gameManager.AddScore(controller.pointsValue);

				//Explosion special effects
				Instantiate(explosion, transform.position, transform.rotation);
			}
			activeCount--;
		}

		/// <summary>
		/// Enemy moves towards a point using the physics engine.
		/// </summary>
		public void MoveTo(Vector3 point)
		{
			transform.LookAt(point);
			transform.forward = -transform.position;
			GetComponent<Rigidbody>().velocity = transform.forward * controller.Speed;
		}

		public void MoveTo(float x, float y, float z)
		{
			Vector3 point = new Vector3(x, y, z);
			MoveTo(point);
		}

		/// <summary>
		/// Object orbits around a defined point by an angle based on its speed.
		/// </summary>
		/// <param name="orbit">The orbit to move in</param>
		public void OrbitAround(Orbit orbit)
		{
			transform.LookAt(orbit.direction, orbit.perpendicularAxis);
			transform.RotateAround(orbit.direction, orbit.perpendicularAxis, controller.Speed * Time.deltaTime);
		}
	}
}