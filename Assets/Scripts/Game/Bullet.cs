using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
	public class Bullet : Spawnable
	{
		/// <summary>
		/// Total count of spawned bullets during the current round.
		/// </summary>
		public static int Count { private set; get; }
		/// <summary>
		/// Count of currently active bullets.
		/// </summary>
		public static int ActiveCount { get; private set; }

		public static Bullet Create(float speed) {
			var o = new GameObject(nameof(Bullet)).AddComponent<Bullet>();

			o.GetComponent<Rigidbody>().useGravity = false;
			o.GetComponent<SphereCollider>().isTrigger = true;
			o.Speed = speed;

			// When the bullet gets created, it starts moving. This is
			// a solution that currently works. After Player.Shoot() gets called
			// the new bullet must be set active. Mind that direct usage of
			// Bullet.Create() is intended mostly for test purposes.
			o.gameObject.SetActive(false);
			return o;
		}

		protected void Start() {
			transform.rotation =
					Camera.main?.transform.rotation
					?? new Quaternion(0, 0, 0, 0);
			transform.position = Vector3.zero;
			GetComponent<Rigidbody>().velocity = transform.forward * Speed;

			Count++;
			ActiveCount++;
		}

		public override void ResetState() {
			throw new System.NotImplementedException();
		}

		public override void Destroy() {
			ActiveCount--;
		}
	}
}
