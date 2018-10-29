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

		public static Bullet Create(float speed)
		{
			var o = new GameObject(nameof(Bullet)).AddComponent<Bullet>();

			o.GetComponent<Rigidbody>().useGravity = false;
			o.GetComponent<SphereCollider>().isTrigger = true;
			o.Speed = speed;

			// When the bullet gets created, it starts moving. This is
			// a solution that currently works. After Player.Shoot() gets called
			// the new bullet must be set active. Mind that Bullet.Create() is
			// intended for test purposes only.
			o.gameObject.SetActive(false);
			return o;
		}

		protected void Start()
		{
			transform.rotation = Camera.main.transform.rotation;
			transform.position = Vector3.zero;
			GetComponent<Rigidbody>().velocity = transform.forward * Speed;

			Count++;
			ActiveCount++;
		}

		protected void OnTriggerEnter(Collider col)
		{
			if (col.GetComponent<Enemies.Enemy>() || col.GetComponent<Capsule>())
			{
				Destroy(col.gameObject);
				Destroy(gameObject);
			}
		}

		protected void OnDestroy()
		{
			ActiveCount--;
		}
	}
}