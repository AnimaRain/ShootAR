using UnityEngine;

namespace ShootAR
{
	public class ArmorCapsule : Capsule
	{
		public static ArmorCapsule Create(float speed, Player player) {
			var o = new GameObject(nameof(ArmorCapsule))
				.AddComponent<ArmorCapsule>();
			o.SetBase(speed, player);
			return o;
		}

		private static float? prefabSpeed = null;
		protected void Awake() {
			if (prefabSpeed is null)
				prefabSpeed = Resources.Load<ArmorCapsule>(Prefabs.ARMOR_CAPSULE).Speed;
		}

		protected override void GivePowerUp() {
			if (player != null)
				player.HasArmor = true;
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<ArmorCapsule>();
		}

		public override void ResetState() {
			Speed = (float)prefabSpeed;
		}
	}
}
