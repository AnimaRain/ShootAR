using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
	public class Bullet : Spawnable
	{
		public const float  MAX_TRAVEL_DISTANCE = 70f;

		/// <summary>
		/// Total count of spawned bullets during the current round.
		/// </summary>
		public static int Count { private set; get; }
		/// <summary>
		/// Count of currently active bullets.
		/// </summary>
		public static int ActiveCount { get; private set; }

		private static float? bulletPrefabSpeed = null;

		public static Bullet Create(float speed) {
			var o = new GameObject(nameof(Bullet)).AddComponent<Bullet>();

			o.GetComponent<Rigidbody>().useGravity = false;
			o.GetComponent<SphereCollider>().isTrigger = true;
			o.Speed = speed;

			o.gameObject.SetActive(false);
			return o;
		}

		protected void Awake() {
			if (bulletPrefabSpeed is null)
				bulletPrefabSpeed = Resources.Load<Bullet>(Prefabs.BULLET).Speed;
		}
		protected void Start() {
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

		protected void OnTriggerEnter(Collider other) {
			Spawnable o;
			if ((o = other.GetComponent<Enemies.Enemy>()) != null
			|| (o = other.GetComponent<Capsule>()) != null) {
				o.Destroy();
				Destroy();
			}
		}

		public override void ResetState() {
			Speed = (float)bulletPrefabSpeed;
		}

		public override void Destroy() {
			ReturnToPool<Bullet>();
		}
	}
}
