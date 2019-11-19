using System.Collections;
using UnityEngine;

namespace ShootAR
{

	public class Spawner : MonoBehaviour
	{
		[SerializeField] private System.Type objectToSpawn;
		/// <summary>
		/// Reference to the type of <see cref="Spawnable"/> prefab to copy
		/// while spawnning.
		/// </summary>
		public System.Type ObjectToSpawn {
			get { return objectToSpawn; }
			private set { objectToSpawn = value; }
		}
		/// <summary>
		/// The time interval between each spawn in seconds.
		/// </summary>
		public float SpawnRate { get; set; }
		/// <summary>
		/// The initial delay before spawning the first object in seconds.
		/// </summary>
		/// <remarks>
		/// Mind the additional waiting time from <see cref="SpawnRate"/>.
		/// </remarks>
		public float InitialDelay { get; private set; }
		[SerializeField] private float maxDistanceToSpawn, minDistanceToSpawn;
		/// <summary>
		/// Maximum distance away from player that <see cref="ObjectToSpawn"/> is
		/// allowed to spawn.
		/// </summary>
		public float MaxDistanceToSpawn {
			get { return maxDistanceToSpawn; }
			private set { maxDistanceToSpawn = value; }
		}
		/// <summary>
		/// Minimum distance away from player that <see cref="ObjectToSpawn"/> is
		/// allowed to spawn.
		/// </summary>
		public float MinDistanceToSpawn {
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
		public bool IsSpawning { get; private set; } = false;

		private AudioSource audioPlayer;
		private static GameState gameState;
#pragma warning disable CS0649
		[SerializeField] private GameObject portal;
		[SerializeField] private AudioClip spawnSfx;
#pragma warning restore CS0649

		private void Awake() {
			//Initial value should not be 0 to refrain from enabling
			//"Game Over" state when the game has just started.
			if (SpawnLimit == 0) SpawnLimit = -1;
		}

		public static Spawner Create(
				System.Type objectToSpawn = null, int spawnLimit = default,
				float initialDelay = default, float spawnRate = default,
				float maxDistanceToSpawn = default,
				float minDistanceToSpawn = default,
				GameState gameState = null) {
			var o = new GameObject(nameof(Spawner)).AddComponent<Spawner>();

			o.ObjectToSpawn = objectToSpawn;
			o.SpawnLimit = spawnLimit;
			o.SpawnRate = spawnRate;
			o.MaxDistanceToSpawn = maxDistanceToSpawn;
			o.MinDistanceToSpawn = minDistanceToSpawn;
			Spawner.gameState = gameState;

			// Since Create() is not an actual constructor, when object o is
			// created, o.gameState is null and the code in OnEnable() won't
			// run.
			o.OnEnable();

			return o;
		}

		private void Start() {
			if (spawnSfx != null) {
				audioPlayer = gameObject.AddComponent<AudioSource>();
				audioPlayer.clip = spawnSfx;
				audioPlayer.volume = 0.2f;
			}

			if (gameState is null)
				gameState = FindObjectOfType<GameState>();
		}

		private void OnEnable() {
			if (gameState != null) {
				gameState.OnGameOver += StopSpawning;
				gameState.OnRoundWon += StopSpawning;
			}
		}

		private void OnDisable() {
			if (gameState != null) {
				gameState.OnGameOver -= StopSpawning;
				gameState.OnRoundWon -= StopSpawning;
			}
		}

		/// <summary>
		/// Spawn objects until the spawn-limit is reached.
		/// </summary>
		/// <remarks>
		/// Automaticaly called through <see cref="StartSpawning"/>. Iteration
		/// will stop when the limit defined by <see cref="SpawnLimit"/> is
		/// reached or can be manually stopped, using
		/// <see cref="StopSpawning"/>.
		///
		/// The spawner changes its position and rotation before spawning an
		/// object. The object is spawned at the same position and with the same
		/// rotation as the spawner.
		///
		/// A pool containing copies of <see cref="objectToSpawn"/> is required.
		/// </remarks>
		/// <seealso cref="Spawnable.Pool{T}"/>
		private IEnumerator Spawn() {
			yield return new WaitForSeconds(InitialDelay);
			while (IsSpawning) {
				yield return new WaitForSeconds(SpawnRate);

				/* IsSpawning is checked here in case StopSpawning() is called
				 * while being in the middle of this function call. */
				if (!IsSpawning) break;
				if (SpawnCount >= Spawnable.GLOBAL_SPAWN_LIMIT) continue;

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
					audioPlayer.Play();

				/* Make checks for each and every type of Spawnable, because
				 * a class inheriting from MonoBehaviour cannot be a generic
				 * class. */
				if (objectToSpawn == typeof(Enemies.Crasher))
					InstantiateSpawnable<Enemies.Crasher>();
				else if (objectToSpawn == typeof(Enemies.Drone))
					InstantiateSpawnable<Enemies.Drone>();
				else if (objectToSpawn == typeof(BulletCapsule))
					InstantiateSpawnable<BulletCapsule>();
				else if (objectToSpawn == typeof(ArmorCapsule))
					InstantiateSpawnable<ArmorCapsule>();
				else if (objectToSpawn == typeof(HealthCapsule))
					InstantiateSpawnable<HealthCapsule>();
				else if (objectToSpawn == typeof(PowerUpCapsule))
					InstantiateSpawnable<PowerUpCapsule>();
				else
					throw new UnityException("Unrecognised type of Spawnable");

				SpawnCount++;

				if (SpawnCount == SpawnLimit) StopSpawning();
			}
		}

		private void InstantiateSpawnable<T>() where T : Spawnable {
			var spawned = Spawnable.Pool<T>.RequestObject();

			spawned.transform.position = transform.localPosition;
			spawned.transform.rotation = transform.localRotation;
			spawned.gameObject.SetActive(true);
		}

		/// <summary>
		/// Start a <see cref="Spawn"/> coroutine.
		/// </summary>
		public void StartSpawning() {
			if (IsSpawning)
				throw new UnityException(
					"A spawner should not be restarted before stopping it first");

			SpawnCount = 0;
			IsSpawning = true;
			StartCoroutine(Spawn());
		}

		/// <summary>
		/// Automatically start a <see cref="Spawn"/> coroutine after
		/// configuring the spawner.
		/// </summary>
		/// <param name="type">The type of object to spawn</param>
		/// <param name="limit">Number of objects to spawn</param>
		/// <param name="rate">
		/// The time in seconds to wait before each spawn
		/// </param>
		/// <param name="delay">
		/// The time in seconds to wait before first spawn
		/// </param>
		/// <param name="maxDistance">
		/// Max distance allowed to spawn away from player
		/// </param>
		/// <param name="minDistance">
		/// Min distance allowed to spawn away from player
		/// </param>
		/// <seealso cref="ObjectToSpawn"/>
		/// <seealso cref="SpawnLimit"/>
		/// <seealso cref="SpawnRate"/>
		/// <seealso cref="SpawnDelay"/>
		/// <seealso cref="MaxDistanceToSpawn"/>
		/// <seealso cref="MinDistanceToSpawn"/>
		public void StartSpawning(System.Type type, int limit, float rate,
					float delay, float maxDistance, float minDistance) {
			ObjectToSpawn = type;
			SpawnLimit = limit;
			SpawnRate = rate;
			InitialDelay = delay;
			MaxDistanceToSpawn = maxDistance;
			MinDistanceToSpawn = minDistance;
			StartSpawning();
		}

		public void StopSpawning() {
			if (!IsSpawning) return;
#if DEBUG
			Debug.Log("Spawn stopped");
#endif

			IsSpawning = false;
			StopCoroutine(Spawn());
		}
	}
}
