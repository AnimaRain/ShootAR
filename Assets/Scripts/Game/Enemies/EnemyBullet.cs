using UnityEngine;

public class EnemyBullet : Boopboop
{
	private void Start()
	{
		MoveTo(Vector3.zero);
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			base.OnTriggerEnter(other);
			Destroy(gameObject);
		}
	}
}