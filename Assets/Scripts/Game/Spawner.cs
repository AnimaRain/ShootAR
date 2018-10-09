using UnityEngine;
using System.Collections;

namespace ShootAR
{

	public class Spawner : MonoBehaviour
	{
		[SerializeField] private Spawnable objectToSpawn;
		/// <summary>
		/// Reference to the <see cref="Spawnable"/> prefab to copy
		/// while spawnning.
		/// </summary>
		public Spawnable ObjectToSpawn {
			get {return objectToSpawn; }
			private set {objectToSpawn = value; }
		}
		/// <summary>
		/// The time interval between each spawn.
		/// </summary>
		public float SpawnRate { get; set; }
		/// <summary>
		/// The initial delay before spawning the first object.
		/// </summary>
		public float InitialDelay { get; private set; }
		[SerializeField] private float maxDistanceToSpawn, minDistanceToSpawn;
		/// <summary>
		/// Maximum distance away from player that <see cref="ObjectToSpawn"/> is
		/// allowed to spawn.
		/// </summary>
		public float MaxDistanceToSpawn
		{
			get { return maxDistanceToSpawn; }
			private set { maxDistanceToSpawn = value; }
		}
		/// <summary>
		/// Minimum distance away from player that <see cref="ObjectToSpawn"/> is
		/// allowed to spawn.
		/// </summary>
		public float MinDistanceToSpawn
		{
			get { return minDistanceToSpawn; }
			private set { minDistanceToSpawn = value; }
		}
		/// <summary>
		/// Number Of ObjectToSpawn objects to spawn.
		/// </summary>
		public int SpawnLimit { get; private set; }
		/// <summary>
		/// Count of how many instances of <see cref="ObjectToSpawn"/> were spawned.
		/// </summary>
		/// <remarks>
		/// Resets every time StartSpawning is called.
		/// </remarks>
		public int SpawnCount { get; private set; }
		public bool IsSpawning { get; private set; }

		[SerializeField] private GameState gameState;
		[SerializeField] private AudioClip spawnSfx;
		[SerializeField] private GameObject portal;
		private AudioSource sfx;

		private void Awake()
		{
			//Initial value should not be 0 to refrain from enabling
			//"Game Over" state when the game has just started.
			if (SpawnLimit == 0) SpawnLimit = -1;
		}

		public static Spawner Create(
			Spawnable objectToSpawn, int spawnLimit, float initialDelay, float spawnRate, 
			float maxDistanceToSpawn, float minDistanceToSpawn,
			GameState gameState = null)
		{
			var o = new GameObject(nameof(Spawner)).AddComponent<Spawner>();

			o.ObjectToSpawn = objectToSpawn;
			o.SpawnLimit = spawnLimit;
			o.SpawnRate = spawnRate;
			o.MaxDistanceToSpawn = maxDistanceToSpawn;
			o.MinDistanceToSpawn = minDistanceToSpawn;
			o.gameState = gameState;

			// Since Create() is not an actual constructor, when object o is
			// created, o.gameState is null and the code in OnEnable() won't
			// run, so the following lines were copied here as well.
			if (gameState != null)
			{
				gameState.OnGameOver += o.StopSpawning;
				gameState.OnRoundWon += o.StopSpawning;
			}

			return o;
		}

		private void Start()
		{
			if (spawnSfx != null)
			{
				sfx = gameObject.AddComponent<AudioSource>();
				sfx.clip = spawnSfx;
				sfx.volume = 0.2f;
			}
		}

		private void OnEnable()
		{
			if (gameState != null)
			{
				gameState.OnGameOver += StopSpawning;
				gameState.OnRoundWon += StopSpawning;
			}
		}

		private void OnDisable()
		{
			if (gameState != null)
			{
				gameState.OnGameOver -= StopSpawning;
				gameState.OnRoundWon -= StopSpawning;
			}
		}

		/// <summary>
		/// Spawn objects until the spawn-limit is reached.
		/// </summary>
		/// <remarks>
		/// Automaticaly called through <see cref="StartSpawning"/>. Iteration will
		/// stop when the limit defined by <see cref="SpawnLimit"/> is reached or
		/// can be manually stopped, using <see cref="StopSpawning"/>.
		/// </remarks>
		public IEnumerator Spawn()
		{
			yield return new WaitForSeconds(InitialDelay);
			do
			{
				Debug.Log("Spawn...");

				/* IsSpawning is checked here, in case StopSpawning() is called
				 * while being in the middle of this function call. */
				if (!IsSpawning) break;
				Debug.Log("Spawn!!!");

				float r = Random.Range(minDistanceToSpawn, maxDistanceToSpawn);
				float theta = Random.Range(0f, Mathf.PI);
				float fi = Random.Range(0f, 2 * Mathf.PI);
				float x = r * Mathf.Sin(theta) * Mathf.Cos(fi);
				float y = r * Mathf.Sin(theta) * Mathf.Sin(fi);
				float z = r * Mathf.Cos(theta);

				transform.localPosition = new Vector3(x, y, z);
				transform.localRotation = Quaternion.LookRotation(
						-transform.localPosition);

				//Spawn special effects
				if (portal != null)
					Instantiate(portal,
						transform.localPosition, transform.localRotation);
				if (spawnSfx != null)
					sfx.Play();

				var spawned = Instantiate(ObjectToSpawn,
					transform.localPosition, transform.localRotation);
				spawned.GameState = gameState;
				SpawnCount++;
				if (SpawnCount == SpawnLimit) StopSpawning();
				yield return new WaitForSeconds(SpawnRate);
			} while (IsSpawning);
		}

		/// <summary>
		/// Automatically start a <see cref="Spawn"/> coroutine.
		/// </summary>
		public void StartSpawning()
		{
			if (IsSpawning)
				throw new UnityException(
					"A spawner should not be restarted before stopping it first");
					
			SpawnCount = 0;
			IsSpawning = true;
			StartCoroutine(Spawn());
		}

		/// <summary>
		/// Automatically start a <see cref="Spawn"/> coroutine after setting the spawn limit
		/// </summary>
		/// <param name="limit">Number of objects to spawn</param>
		public void StartSpawning(int limit)
		{
			SpawnLimit = limit;
			StartSpawning();
		}

		/// <summary>
		/// Automatically start a <see cref="Spawn"/> coroutine after setting the spawn limit and
		/// the rate of spawn.
		/// </summary>
		/// <param name="limit">Number of objects to spawn</param>
		/// <param name="rate">The time in seconds to wait before each spawn</param>
		public void StartSpawning(int limit, float rate)
		{
			SpawnRate = rate;
			SpawnLimit = limit;
			StartSpawning();
		}

		/// <summary>
		/// Automatically start a <see cref="Spawn"/> coroutine after setting the spawn limit,
		/// the rate of spawn and the initial delay.
		/// </summary>
		/// <param name="limit">Number of objects to spawn</param>
		/// <param name="rate">The time in seconds to wait before each spawn</param>
		/// <param name="delay">The time in seconds to wait before first spawn</param>
		/// <seealso cref="SpawnLimit"/>
		/// <seealso cref="SpawnRate"/>
		/// <seealso cref="SpawnDelay"/>
		public void StartSpawning(int limit, float rate, float delay)
		{
			InitialDelay = delay;
			StartSpawning(limit, rate);
		}

		public void StopSpawning()
		{
			Debug.Log("Spawn stopped");

			IsSpawning = false;
			StopCoroutine(Spawn());
		}
	}
}