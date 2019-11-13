using UnityEngine;

namespace ShootAR
{
	public class PowerUpCapsule : Capsule
	{
		public static PowerUpCapsule Create(float speed, Player player) {
			var o = new GameObject(nameof(PowerUpCapsule))
				.AddComponent<PowerUpCapsule>();
			o.SetBase(speed, player);
			return o;
		}

		protected override void GivePowerUp() {
			throw new System.NotImplementedException();
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<PowerUpCapsule>();
		}

		private static PowerUpCapsule prefab;
		protected override void Start() {
			if (prefab is null)
				prefab = FindObjectOfType<PrefabContainer>()?.PowerUpCapsule;
			base.Start();
		}

		public override void ResetState() {
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
		}
	}
}
