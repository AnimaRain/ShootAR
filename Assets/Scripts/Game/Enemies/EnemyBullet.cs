using UnityEngine;

public class EnemyBullet : Boopboop
{
	protected override void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			base.OnTriggerEnter(other);
			Destroy(gameObject);
		}
	}
}