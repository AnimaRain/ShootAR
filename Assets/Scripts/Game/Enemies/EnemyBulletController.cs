using UnityEngine;

namespace ShootAR.Enemies
{
	public class EnemyBulletController : BoopboopController
	{
		protected override void Start()
		{
			MoveTo(Vector3.zero);
		}

		public override void Attack(Player target)
		{
			base.Attack(target);
			Destroy(gameObject);
		}
	}
}