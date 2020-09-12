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
			player.PowerUp(
				(Player.WeaponUpgrade)Random.Range(
					0,
					System.Enum.GetNames(typeof(Player.WeaponUpgrade)).Length
				)
			);
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<PowerUpCapsule>();
		}

		private static float? prefabSpeed;
		protected void Awake() {
			if (prefabSpeed is null)
				prefabSpeed = Resources.Load<PowerUpCapsule>(Prefabs.POWER_UP_CAPSULE).Speed;
		}

		public override void ResetState() {
			Speed = (float)prefabSpeed;
		}
	}
}
