using UnityEngine;
using System.Collections;

public class EnemySpawner : Spawner
{
	public AudioClip SpawnSfx;
	public GameObject Portal;

	protected AudioSource sfx;

	protected override void Awake()
	{
		base.Awake();

		sfx = gameObject.AddComponent<AudioSource>();
		sfx.clip = SpawnSfx;
		sfx.volume = 0.2f;
	}

	public override IEnumerator Spawn()
	{
		base.Spawn();

		//Spawn special effects
		Instantiate(Portal, spawnPosition, spawnRotation);
		sfx.Play();

		yield return new WaitForSeconds(0f);
	}
}
