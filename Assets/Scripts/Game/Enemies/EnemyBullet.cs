using UnityEngine;

namespace ShootAR.Enemies
{
	public class EnemyBullet : Boopboop
	{
		protected override void Start() {
			base.Start();
			MoveTo(Vector3.zero);
		}

		public override void ResetState() {
			throw new System.NotImplementedException();
		}

		public override void Attack(Player target) {
			base.Attack(target);
			Destroy(gameObject);
		}

		public override void Destroy() {
			throw new System.NotImplementedException();
		}
	}
}
