using UnityEngine;

namespace ShootAR.Enemies
{

	/// <summary>
	/// Parent class of all types of enemies.
	/// </summary>
	public class Enemy : Spawnable
	{

		/// <summary>
		/// Total count of spawned enemies during the current round.
		/// </summary>
		public static int count;
		/// <summary>
		/// Count of currently active enemies.
		/// </summary>
		public static int activeCount;
		/// <summary>
		/// The amount of points added to the player's score when destroyed.
		/// </summary>
		public int pointsValue;
		/// <summary>
		/// The amount of damage the player recieves from this object's attack.
		/// </summary>
		[Range(-Player.HEALTH_MAX, Player.HEALTH_MAX)]
		public int damage;
		[SerializeField] protected AudioClip attackSfx;
		[SerializeField] protected GameObject explosion;

		protected AudioSource sfx;
		protected static GameManager gameManager;


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

			if (gameManager != null) gameManager = FindObjectOfType<GameManager>();
		}

		protected virtual void OnDestroy()
		{
			if (!gameManager.gameOver)
			{
				gameManager.AddScore(pointsValue);

				//Explosion special effects
				Instantiate(explosion, transform.position, transform.rotation);
			}
			activeCount--;
		}

	}
}