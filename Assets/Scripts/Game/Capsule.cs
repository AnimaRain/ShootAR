using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(AudioSource))]
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
				Player player = null, GameState gameState = null) {
			var o = new GameObject(nameof(Capsule)).AddComponent<Capsule>();
			o.Type = type;
			o.Speed = speed;
			o.player = player;
			o.GameState = gameState;
			return o;
		}

		protected void Start() {
			rotation = new Vector3(15, 30, 45);
			pickUpSfx = GetComponent<AudioSource>();
		}

		private void Update() {
			//rotation
			transform.Rotate(rotation * Time.deltaTime);
			//orbit
			transform.RotateAround(Vector3.zero, Vector3.up, Speed * Time.deltaTime);
		}

		protected void OnDestroy() {
			if (gameState != null ? gameState.GameOver : false) return;

			GivePowerUp();
			pickUpSfx?.Play();
		}

		public void GivePowerUp() {
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
