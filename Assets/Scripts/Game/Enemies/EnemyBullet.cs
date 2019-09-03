using UnityEngine;

namespace ShootAR.Enemies
{
	public class EnemyBullet : Boopboop
	{
		protected override void Start() {
			base.Start();
		}

		protected override void OnEnable() {
			base.OnEnable();
			MoveTo(Vector3.zero);
		}

		public override void ResetState() {
			throw new System.NotImplementedException();
		}

		public override void Attack(Player target) {
			base.Attack(target);
			Destroy();
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<EnemyBullet>();
		}
	}
}
