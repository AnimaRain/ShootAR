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

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private float nextSpawn, x, z;
    private GameSounds gameSounds;


    private void Awake()
    {
        if (gameSounds == null) gameSounds = GameObject.Find("GameController").GetComponent<GameSounds>();
        SpawnLimit = -1;    //Initial value should not be 0, to refrain from enabling "Game Over" state when the game has just started.
    }
    

    /// <summary>
    /// Spawn objects until the spawn-limit is reached.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Spawn()
    {
        do
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
            spawnPosition = new Vector3(x, 0, z);
            spawnRotation = Quaternion.LookRotation(-spawnPosition);

            if (ObjectToSpawn.name == "Podpod")
            {
                GameObject Portal = Instantiate(Resources.Load("Effect_03", typeof(GameObject))) as GameObject;
                Portal.transform.position = spawnPosition;
                Destroy(Portal, 1);
                gameSounds.EnemySpawning();
            }

            var temp = Instantiate(ObjectToSpawn, spawnPosition, spawnRotation);   //TO DO: Remove temp variable when debug is not needed any more.        
            SpawnCount++;
#if DEBUG
            temp.name = ObjectToSpawn.name + SpawnCount;
#endif
        } while (SpawnCount < SpawnLimit);
    }
}