using UnityEngine;
using System.Collections;

namespace ShootAR
{

	public class Spawner : MonoBehaviour
	{
		[SerializeField] private ISpawnable objectToSpawn;
		public ISpawnable ObjectToSpawn {
			get {return objectToSpawn; }
			private set {objectToSpawn = value; }
		}
		public float SpawnRate { get; set; }
		/// <summary>
		/// Distance away from player.
		/// </summary>
		[SerializeField] private float maxDistanceToSpawn, minDistanceToSpawn;
		public float MaxDistanceToSpawn
		{
			get { return maxDistanceToSpawn; }
			private set { maxDistanceToSpawn = value; }
		}
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
		/// Counts how many instances of ObjectToSpawn were spawned. Resets 
		/// every time StartSpawning is called.
		/// </summary>
		public int SpawnCount { get; private set; }
		public bool IsSpawning { get; private set; }

		private GameState gameState;
		[SerializeField] private readonly AudioClip spawnSfx;
		[SerializeField] private readonly GameObject portal;
		private AudioSource sfx;

		public static Spawner Create(
			ISpawnable objectToSpawn, int spawnLimit, float spawnRate, 
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

		private void Awake()
		{
			//Initial value should not be 0 to refrain from enabling
			//"Game Over" state when the game has just started.
			if (SpawnLimit == 0) SpawnLimit = -1;
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
		/// <returns></returns>
		public IEnumerator Spawn()
		{
			IsSpawning = true;
			while (IsSpawning)
			{
				yield return new WaitForSeconds(SpawnRate);

				/* IsSpawning is checked again, in case StopSpawning() is called
				 * while being in the middle of this function call. */
				if (!IsSpawning) break;

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

				Instantiate((Object)ObjectToSpawn,
					transform.localPosition, transform.localRotation);
				SpawnCount++;
				if (SpawnCount == SpawnLimit) StopSpawning();
			}
		}

		public void StartSpawning()
		{
			SpawnCount = 0;
			StartCoroutine(Spawn());
		}

		/// <summary>
		/// Automatically start a Spawn coroutine after setting the spawn limit
		/// </summary>
		/// <param name="spawnLimit">Number of objects to spawn</param>
		public void StartSpawning(int spawnLimit)
		{
			SpawnLimit = spawnLimit;
			StartSpawning();
		}

		/// <summary>
		/// Automatically start a Spawn coroutine after setting the spawn limit and
		/// the rate of spawn.
		/// </summary>
		/// <param name="spawnLimit">Number of objects to spawn</param>
		/// <param name="rateOfSpawn">The time in seconds to wait before each spawn</param>
		public void StartSpawning(int spawnLimit, float rateOfSpawn)
		{
			SpawnRate = rateOfSpawn;
			SpawnLimit = spawnLimit;
			StartSpawning();
		}

		public void StopSpawning()
		{
			IsSpawning = false;
			StopCoroutine(Spawn());
		}
	}
}