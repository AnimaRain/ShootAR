using UnityEngine;

public class EnemyBullet : Boopboop
{
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		Destroy(gameObject);
	}
}