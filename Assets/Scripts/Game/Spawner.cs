//TO DO: Remove local variable temp

using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public readonly SpawnableObject ObjectToSpawn;
    public float RateOfSpawn;
    /// <summary>
    /// Distance away from player.
    /// </summary>
    public float maxDistanceToSpawn, minDistanceToSpawn;
    /// <summary>
    /// Number Of ObjectToSpawn objects to spawn.
    /// </summary>
    [HideInInspector] public int SpawnLimit;
    /// <summary>
    /// Counts how many times ObjectToSpawn spawned. Resets every time StartSpawning is called.
    /// </summary>
    [HideInInspector] public int SpawnCount;
	[HideInInspector] public bool IsSpawning;
	[SerializeField] private readonly AudioClip spawnSfx;
	[SerializeField] private readonly GameObject portal;

	private float x, z;
	private AudioSource sfx;


	private void Awake()
    {
        SpawnLimit = -1;    //Initial value should not be 0 to refrain from enabling "Game Over" state when the game has just started.

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
		/* IsSpawning is checked in the while-condition function, in case
		 * StopSpawning() is called while being in the in the middle of 
		 * the iteration;
		 */
		while (IsSpawning && SpawnCount < SpawnLimit)
		{
            yield return new WaitForSeconds(RateOfSpawn);
            //The following do-while assigns random coordinates for the
            //next spawn according to the defined range. Then, the object's
            //rotation is set towards the player and spawns the object.
            do
            {
                x = Random.Range(-maxDistanceToSpawn, maxDistanceToSpawn);
                z = Random.Range(-maxDistanceToSpawn, maxDistanceToSpawn);
            } while (Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2)) <= minDistanceToSpawn);
            transform.localPosition = new Vector3(x, 0, z);
            transform.localRotation = Quaternion.LookRotation(-transform.localPosition);

			if (IsSpawning)
			{
				//Spawn special effects
				if (portal != null)
					Instantiate(portal, transform.localPosition, transform.localRotation);
				if (spawnSfx != null)
					sfx.Play();

				var temp = Instantiate(ObjectToSpawn, transform.localPosition, transform.localRotation);
				SpawnCount++;
#if DEBUG
				temp.name = ObjectToSpawn.name + SpawnCount;
#endif
			}
        }
		IsSpawning = false;
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
		this.SpawnLimit = spawnLimit;
		StartSpawning();
	}

	/// <summary>
	/// Automatically start a Spawn coroutine after setting the spawn limit
	/// </summary>
	/// <param name="spawnLimit">Number of objects to spawn</param>
	/// <param name="rateOfSpawn">The time in seconds to wait before each spawn</param>
	public void StartSpawning(int spawnLimit, float rateOfSpawn)
	{
		this.RateOfSpawn = rateOfSpawn;
		StartSpawning(spawnLimit);
	}

	public void StopSpawning()
	{
		IsSpawning = false;
		StopCoroutine(Spawn());
	}
}