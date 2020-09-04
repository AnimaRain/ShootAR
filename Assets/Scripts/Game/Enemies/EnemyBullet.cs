using UnityEngine;

namespace ShootAR.Enemies
{
	public class EnemyBullet : Boopboop
	{
		private const float DEFAULT_SPEED = 5F;
		private const int DEFAULT_POINTS = 5;

		private static float? prefabSpeed = null;
		private static int? prefabPointValue = null;

		protected override void Awake() {
			base.Awake();
			if (prefabSpeed == null)
				prefabSpeed = Resources.Load<EnemyBullet>(Prefabs.ENEMY_BULLET).Speed;
			if (prefabPointValue == null)
				prefabPointValue = Resources.Load<EnemyBullet>(Prefabs.ENEMY_BULLET).PointsValue;
		}

		protected override void OnEnable() {
			base.OnEnable();
			MoveTo(Vector3.zero);
		}

		public override void ResetState() {
			Speed = (float)prefabSpeed;
			PointsValue = (int)prefabPointValue;
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
