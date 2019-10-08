using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(AudioSource)),
	 RequireComponent(typeof(CapsuleCollider))]
	public class Capsule : Spawnable
	{
		private const float DEFAULT_SPEED = 15F;

		public enum CapsuleType
		{
			Bullet,
			Health,
			Armor,
			PowerUp
		}
		public CapsuleType Type { get; private set; }

		private Vector3 rotation;
		private AudioSource pickUpSfx;

		[SerializeField] private Player player;
		private static Capsule prefab;

		public static Capsule Create(CapsuleType type, float speed,
				Player player = null) {
			var o = new GameObject(nameof(Capsule)).AddComponent<Capsule>();
			o.Type = type;
			o.Speed = speed;
			o.player = player;

			var capsuleCollider = o.GetComponent<CapsuleCollider>();
			capsuleCollider.isTrigger = true;
			capsuleCollider.radius = 0.5f;
			capsuleCollider.height = 2f;

			return o;
		}

		protected override void Start() {
			base.Start();
			if (prefab is null)
				prefab = FindObjectOfType<PrefabContainer>()?.Capsule;

			rotation = Random.rotation.eulerAngles;
			pickUpSfx = GetComponent<AudioSource>();
		}

		public override void ResetState() {
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
			Type = (CapsuleType)Random.Range(0, 4);
		}

		private void Update() {
			//rotation
			transform.Rotate(rotation * Time.deltaTime);
			//orbit
			transform.RotateAround(Vector3.zero, Vector3.up, Speed * Time.deltaTime);
		}

		public override void Destroy() {
			GivePowerUp();
			pickUpSfx?.Play();
			ReturnToPool<Capsule>();
		}

		private void GivePowerUp() {
			switch (Type) {
				case 0:
					if (player != null)
						player.Ammo += 10;
					break;

					//UNDONE: Write cases for the rest of the types of capsule
			}
		}
	}
}
