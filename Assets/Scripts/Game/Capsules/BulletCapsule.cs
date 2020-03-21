using UnityEngine;

namespace ShootAR
{
	public class BulletCapsule : Capsule
	{
		private const int REWARD = 10;

		public static BulletCapsule Create(float speed, Player player) {
			var o = new GameObject(nameof(BulletCapsule))
				.AddComponent<BulletCapsule>();
			o.SetBase(speed, player);
			return o;
		}

		protected override void GivePowerUp() {
			if (player != null)
				player.Ammo += REWARD;
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<BulletCapsule>();
		}

		private static float? prefabSpeed = null;
		protected override void Start() {
			if (prefabSpeed is null)
				prefabSpeed = Resources.Load<BulletCapsule>(Prefabs.BULLET_CAPSULE).Speed;
			base.Start();
		}

		public override void ResetState() {
			Speed = (float)prefabSpeed;
		}
	}
}
