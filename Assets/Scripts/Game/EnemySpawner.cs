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
		sfx.volume = 0.5f;
	}

	public override IEnumerator Spawn()
	{
		//Spawn special effects
		Instantiate(Portal, transform.position, transform.rotation);
		sfx.Play();

		base.Spawn();
		yield return new WaitForSeconds(0f);
	}
}
