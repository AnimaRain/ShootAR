using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
	public class Bullet : Spawnable
	{
		public const float	MAX_TRAVEL_DISTANCE = 70f,
							DEFAULT_SPEED = 10f;

		/// <summary>
		/// Total count of spawned bullets during the current round.
		/// </summary>
		public static int Count { private set; get; }
		/// <summary>
		/// Count of currently active bullets.
		/// </summary>
		public static int ActiveCount { get; private set; }

		private static Bullet prefab;

		public static Bullet Create(float speed) {
			var o = new GameObject(nameof(Bullet)).AddComponent<Bullet>();

			o.GetComponent<Rigidbody>().useGravity = false;
			o.GetComponent<SphereCollider>().isTrigger = true;
			o.Speed = speed;

			o.gameObject.SetActive(false);
			return o;
		}

		protected override void Start() {
			base.Start();
			if (prefab is null)
				prefab = FindObjectOfType<PrefabContainer>()?.Bullet;

			transform.rotation =
					Camera.main?.transform.rotation
					?? new Quaternion(0, 0, 0, 0);
		}

		private void OnEnable() {
			GetComponent<Rigidbody>().velocity = transform.forward * Speed;

			Count++;
			ActiveCount++;
		}

		private void OnDisable() {
			ActiveCount--;
		}

		private void LateUpdate() {
			if (transform.position.magnitude >= MAX_TRAVEL_DISTANCE) Destroy();
		}

		protected new void OnTriggerEnter(Collider other) {
			if (other.GetComponent<Enemies.Enemy>()
					|| other.GetComponent<Capsule>())
				Destroy();
		}

		public override void ResetState() {
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
		}

		public override void Destroy() {
			ReturnToPool<Bullet>();
		}
	}
}
