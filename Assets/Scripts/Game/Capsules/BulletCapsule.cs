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

		private static BulletCapsule prefab;
		protected override void Start() {
			if (prefab is null)
				prefab = FindObjectOfType<PrefabContainer>()?.BulletCapsule;
			base.Start();
		}

		public override void ResetState() {
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
		}
	}
}
