using UnityEngine;

namespace ShootAR.Enemies
{
	public class EnemyBullet : Boopboop
	{
		protected override void Start()
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
}