using UnityEngine;

namespace ShootAR.Enemies
{
	public class EnemyBullet : Boopboop
	{
		private const float DEFAULT_SPEED = 5F;
		private const int DEFAULT_POINTS = 5;

		private static float? prefabSpeed = null;
		private static ulong? prefabPointValue = null;

		protected override void Awake() {
			base.Awake();

			EnemyBullet prefab;
			if (prefabSpeed == null || prefabPointValue == null) {
				prefab = Resources.Load<EnemyBullet>(Prefabs.ENEMY_BULLET);

				if (prefabSpeed == null)
					prefabSpeed = prefab.Speed;
				if (prefabPointValue == null)
					prefabPointValue = prefab.PointsValue;
			}
		}

		protected override void OnEnable() {
			base.OnEnable();
			MoveTo(Vector3.zero);
		}

		public override void ResetState() {
			base.ResetState();
			Speed = (float)prefabSpeed;
			PointsValue = (ulong)prefabPointValue;
		}

		public override void Attack(){
			MoveTo(Vector3.zero);
		}

		protected override void Harm(Player target) {
			sfx.Play();
			target.GetDamaged(Damage);
			Destroy();
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<EnemyBullet>();
		}
	}
}
