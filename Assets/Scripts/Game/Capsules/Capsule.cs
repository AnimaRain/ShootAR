using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(AudioSource)),
	 RequireComponent(typeof(CapsuleCollider))]
	public abstract class Capsule : Spawnable
	{
		protected const float DEFAULT_SPEED = 15F;

		public bool IsMoving { get; protected set; } = true;

		private Vector3 rotation;
		private AudioSource pickUpSfx;

		protected static Player player;

		/// <summary>
		/// Use in Capsule sub-classes. Sets the base of Capsules.
		/// </summary>
		protected void SetBase(float speed, Player player) {
			Speed = speed;
			Capsule.player = player;	/* Object.Instantiate can not set
										 * reference to the player so this had
										 * to be static */

			var capsuleCollider = GetComponent<CapsuleCollider>();
			capsuleCollider.isTrigger = true;
			capsuleCollider.radius = 0.5f;
			capsuleCollider.height = 2f;

			gameObject.SetActive(false);
		}

		protected void Start() {
			if (player == null)
				player = FindObjectOfType<Player>();

			rotation = Random.rotation.eulerAngles;
			pickUpSfx = GetComponent<AudioSource>();
		}

		private void Update() {
			//rotation
			transform.Rotate(rotation * Time.deltaTime);

			if (IsMoving) {
				//orbit
				transform.RotateAround(Vector3.zero, Vector3.up, Speed * Time.deltaTime);
			}
		}

		public override void Destroy() {
			GivePowerUp();
			pickUpSfx?.Play();
		}

		protected abstract void GivePowerUp();

		public void StartMoving() => IsMoving = true;

		public void StopMoving() => IsMoving = false;
	}
}
