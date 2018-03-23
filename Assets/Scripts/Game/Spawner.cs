//TO DO: Remove local variable temp

using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public SpawnableObject ObjectToSpawn;
    public float RateOfSpawn;
    /// <summary>
    /// Distance away from player.
    /// </summary>
    public float MaxDistanceToSpawn, MinDistanceToSpawn;
    /// <summary>
    /// Number Of ObjectToSpawn objects to spawn.
    /// </summary>
    public int SpawnLimit;
    /// <summary>
    /// Counts how many times ObjectToSpawn spawned.
    /// </summary>
    [HideInInspector]
    public int SpawnCount;
	[HideInInspector]
	public bool isSpawning;
	[SerializeField]
	private AudioClip SpawnSfx;
	[SerializeField]
	private GameObject Portal;

	private float x, z;
	private AudioSource sfx;


	private void Awake()
    {
        SpawnLimit = -1;    //Initial value should not be 0 to refrain from enabling "Game Over" state when the game has just started.

		if (SpawnSfx != null)
		{
			sfx = gameObject.AddComponent<AudioSource>();
			sfx.clip = SpawnSfx;
			sfx.volume = 0.2f;
		}
	}
    

    /// <summary>
    /// Spawn objects until the spawn-limit is reached.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Spawn()
    {
		isSpawning = true;
		while (isSpawning && SpawnCount < SpawnLimit)
		{
            yield return new WaitForSeconds(RateOfSpawn);
            //The following do-while assigns random coordinates for the
            //next spawn according to the defined range. Then, the object's
            //rotation is set towards the player and spawns the object.
            do
            {
                x = Random.Range(-MaxDistanceToSpawn, MaxDistanceToSpawn);
                z = Random.Range(-MaxDistanceToSpawn, MaxDistanceToSpawn);
            } while (Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2)) <= MinDistanceToSpawn);
            transform.localPosition = new Vector3(x, 0, z);
            transform.localRotation = Quaternion.LookRotation(-transform.localPosition);

			if (isSpawning)
			{
				//Spawn special effects
				if (Portal != null)
					Instantiate(Portal, transform.localPosition, transform.localRotation);
				if (SpawnSfx != null)
					sfx.Play();

				var temp = Instantiate(ObjectToSpawn, transform.localPosition, transform.localRotation);
				SpawnCount++;
				if (SpawnCount == SpawnLimit) break;
#if DEBUG
				temp.name = ObjectToSpawn.name + SpawnCount;
#endif
			}
        };
		isSpawning = false;
    }

	/// <summary>
	/// Automatically start a Spawn coroutine after setting the spawn limit
	/// </summary>
	/// <param name="SpawnLimit">Number of objects to spawn</param>
	public void Spawn(int SpawnLimit)
	{
		this.SpawnLimit = SpawnLimit;
		StartCoroutine(Spawn());
	}

	/// <summary>
	/// Automatically start a Spawn coroutine after setting the spawn limit
	/// </summary>
	/// <param name="SpawnLimit">Number of objects to spawn</param>
	/// <param name="RateOfSpawn">The time in seconds to wait before each spawn</param>
	public void Spawn(int SpawnLimit, float RateOfSpawn)
	{
		this.RateOfSpawn = RateOfSpawn;
		Spawn(SpawnLimit);
	}
}