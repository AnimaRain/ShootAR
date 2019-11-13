using UnityEngine;

namespace ShootAR
{
	public class ArmorCapsule : Capsule
	{
		private const int REWARD = 10;

		[SerializeField] private ArmorCapsule prefab;

		public static ArmorCapsule Create(float speed, Player player) {
			var o = new GameObject(nameof(ArmorCapsule))
				.AddComponent<ArmorCapsule>();
			o.SetBase(speed, player);
			return o;
		}

		protected override void Start() {
			if (prefab is null)
				prefab = FindObjectOfType<PrefabContainer>()?.ArmorCapsule;
			base.Start();
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
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
		}
	}
}
