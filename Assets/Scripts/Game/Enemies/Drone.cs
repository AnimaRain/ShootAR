using System.Collections;
using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(CapsuleCollider))]
	public class Drone : Pyoopyoo
	{
		protected const float SHOOT_DELAY = 6f;
		protected const float MOVE_DELAY = 15F;

		/// <summary>
		/// The point in time that the next shot is allowed.
		/// </summary>
		protected float nextShot;

		/// <summary>
		/// The point in time that moving to a different spot is allowed.
		/// Does not count for orbiting.
		/// </summary>
		protected float nextMove;

		protected static float? prefabSpeed = null;
		protected static ulong? prefabPoints = null;
		protected static int? prefabDamage = null;

		public override void ResetState() {
			base.ResetState();
			Speed = (float)prefabSpeed;
			PointsValue = (ulong)prefabPoints;
			Damage = (int)prefabDamage;
		}

		protected override void Awake() {
			base.Awake();

			if (prefabSpeed is null || prefabPoints is null || prefabDamage is null) {
				Drone prefab = Resources.Load<Drone>(Prefabs.DRONE);
				if (prefabSpeed is null)
					prefabSpeed = prefab.Speed;
				if (prefabPoints is null)
					prefabPoints = prefab.PointsValue;
				if (prefabDamage is null)
					prefabDamage = prefab.Damage;
			}
		}

		protected override void Start() {
			base.Start();

			// Drones shouldn't shoot as soon as they appear, so the time
			// is initialy randomized.
			nextShot = Random.Range(5f, 10f);
		}

		protected void FixedUpdate() {
			// Shoot
			if (Time.time > nextShot) {
				transform.LookAt(Vector3.zero, Vector3.up);

				Shoot();
				nextShot = Time.time + SHOOT_DELAY;
			}

			// Move around

			// Don't go too close to player.
			float safetyDistance = 10f - transform.position.magnitude;
			if (safetyDistance > 0) {
				float randomFactor = Random.Range(10f, 25f);

				Vector3 retreatPoint = -transform.forward * (safetyDistance + randomFactor);
				retreatPoint += Random.insideUnitSphere * randomFactor;

				transform.LookAt(retreatPoint, Vector3.up);
				MoveTo(retreatPoint);
			}

			// Orbit the player for a while
			else if (Time.time > nextMove) {
				Vector3 halfPoint = transform.position / 2;
				Vector3 location = halfPoint
						+ Random.insideUnitSphere
						* (halfPoint.magnitude
							- halfPoint.magnitude >= 10f ? 10f : 0f);

				MoveTo(location);
				nextMove = Time.time + MOVE_DELAY;
			}

			// Move to a random point closer to player.
			else if (!IsMoving) {
				transform.LookAt(Vector3.zero, Vector3.up);
				transform.RotateAround(
					point: Vector3.zero,
					axis: Vector3.up,
					angle: Time.fixedDeltaTime * Speed
				);
			}
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<Drone>();
		}

		public override void Attack() => Shoot();

		protected override void Shoot() {
			EnemyBullet bullet = Pool<EnemyBullet>.Instance.RequestObject();
			if (bullet == null) return;

			bullet.transform.position = bulletSpawnPoint.position;
			bullet.transform.rotation = bulletSpawnPoint.rotation;
			bullet.Damage = Damage;
			bullet.gameObject.SetActive(true);

			if (sfx?.clip != null) sfx.Play();
		}
	}
}
