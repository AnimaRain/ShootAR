using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(AudioSource)),
	 RequireComponent(typeof(CapsuleCollider))]
	public class Capsule : Spawnable
	{
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

		protected void Start() {
			rotation = new Vector3(15, 30, 45);
			pickUpSfx = GetComponent<AudioSource>();
		}

		public override void ResetState() {
			throw new System.NotImplementedException();
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
