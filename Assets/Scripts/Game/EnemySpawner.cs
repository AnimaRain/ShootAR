using UnityEngine;
using System.Collections;

public class EnemySpawner : Spawner
{
	public AudioClip SpawnSfx;
	public GameObject Portal;

	private AudioSource sfx;

	protected override void Awake()
	{
		base.Awake();

		sfx = new AudioSource();
	}

	public override IEnumerator Spawn()
	{
		//Spawn special effects
		Instantiate(Portal, transform.position, transform.rotation);
		sfx.PlayOneShot(SpawnSfx, 0.5f);

		base.Spawn();
		yield return new WaitForSeconds(0f);
	}
}
