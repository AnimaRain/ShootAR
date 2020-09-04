using UnityEngine;

namespace ShootAR
{
	public class HealthCapsule : Capsule
	{
		private const int REWARD = 1;

		public static HealthCapsule Create(float speed, Player player) {
			var o = new GameObject(nameof(HealthCapsule))
				.AddComponent<HealthCapsule>();
			o.SetBase(speed, player);
			return o;
		}

		protected override void GivePowerUp() {
			if (player != null)
				player.Health += REWARD;
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<HealthCapsule>();
		}

		private static float? prefabSpeed;
		protected void Awake() {
			if (prefabSpeed is null)
				prefabSpeed = Resources.Load<HealthCapsule>(Prefabs.HEALTH_CAPSULE).Speed;
		}

		public override void ResetState() {
			Speed = (float)prefabSpeed;
		}
	}
}
