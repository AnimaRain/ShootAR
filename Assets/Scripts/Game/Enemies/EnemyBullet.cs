using UnityEngine;

namespace ShootAR.Enemies
{
	public class EnemyBullet : Boopboop
	{
		private const float DEFAULT_SPEED = 5F;
		private const int DEFAULT_POINTS = 5;

		private static EnemyBullet prefab;

		protected override void Start() {
			base.Start();
			if (prefab == null)
				prefab = FindObjectOfType<PrefabContainer>()?.EnemyBullet;
		}

		protected override void OnEnable() {
			base.OnEnable();
			MoveTo(Vector3.zero);
		}

		public override void ResetState() {
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
			PointsValue = prefab is null ? DEFAULT_POINTS: prefab.PointsValue;
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
