using UnityEngine;
using System.Collections;

namespace ShootAR
{

	public class Spawner : MonoBehaviour
	{
		public ISpawnable ObjectToSpawn { get; private set; }
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
		/// Counts how many times ObjectToSpawn spawned. Resets every time
		/// StartSpawning is called.
		/// </summary>
		public int SpawnCount { get; private set; }
		public bool IsSpawning { get; private set; }
		[SerializeField] private readonly AudioClip spawnSfx;
		[SerializeField] private readonly GameObject portal;

		private AudioSource sfx;

		public static Spawner Create(
			ISpawnable objectToSpawn, int spawnLimit, int spawnRate, 
			float maxDistanceToSpawn, float minDistanceToSpawn)
		{
			var o = new GameObject("Spawner").AddComponent<Spawner>();
			o.ObjectToSpawn = objectToSpawn;
			o.SpawnLimit = spawnLimit;
			o.SpawnRate = spawnRate;
			o.MaxDistanceToSpawn = maxDistanceToSpawn;
			o.MinDistanceToSpawn = minDistanceToSpawn;
			return o;
		}

		private void Awake()
		{
			SpawnLimit = -1;    //Initial value should not be 0 to refrain from
								//enabling "Game Over" state when the game has
								//just started.

			if (spawnSfx != null)
			{
				sfx = gameObject.AddComponent<AudioSource>();
				sfx.clip = spawnSfx;
				sfx.volume = 0.2f;
			}
		}


		/// <summary>
		/// Spawn objects until the spawn-limit is reached.
		/// </summary>
		/// <returns></returns>
		public IEnumerator Spawn()
		{
			IsSpawning = true;
			float x, z;
			while (IsSpawning)
			{
				yield return new WaitForSeconds(SpawnRate);

				/* IsSpawning is checked again, in case StopSpawning() is called
				 * while being in the middle of this function call. */
				if (!IsSpawning) break;

				do
				{
					x = Random.Range(-MaxDistanceToSpawn, MaxDistanceToSpawn);
					z = Random.Range(-MaxDistanceToSpawn, MaxDistanceToSpawn);
				} while (Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2)) <= MinDistanceToSpawn);
				transform.localPosition = new Vector3(x, 0, z);
				transform.localRotation = Quaternion.LookRotation(-transform.localPosition);

				//Spawn special effects
				if (portal != null)
					Instantiate(portal, transform.localPosition, transform.localRotation);
				if (spawnSfx != null)
					sfx.Play();

				Instantiate((Object)ObjectToSpawn, transform.localPosition, transform.localRotation);
				SpawnCount++;
				if (SpawnCount == SpawnLimit) IsSpawning = false;
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