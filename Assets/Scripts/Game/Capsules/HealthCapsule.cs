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

		private static HealthCapsule prefab;
		protected override void Start() {
			if (prefab is null)
				prefab = FindObjectOfType<PrefabContainer>()?.HealthCapsule;
			base.Start();
		}

		public override void ResetState() {
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
		}
	}
}
