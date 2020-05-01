using System.Collections;
using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(CapsuleCollider))]
	public class Drone : Pyoopyoo
	{
		protected const float SHOOT_DELAY = 5f;

		/// <summary>
		/// The point in time that the next shot is allowed.
		/// </summary>
		protected float nextShot;

		protected static float? prefabSpeed = null;
		protected static int? prefabPoints = null,
							 prefabDamage = null;

		public override void ResetState() {
			Speed = (float)prefabSpeed;
			PointsValue = (int)prefabPoints;
			Damage = (int)prefabDamage;
		}

		protected override void Awake() {
			base.Awake();

			Drone prefab = Resources.Load<Drone>(Prefabs.DRONE);
			if (prefabSpeed is null)
				prefabSpeed = prefab.Speed;
			if (prefabPoints is null)
				prefabPoints = prefab.PointsValue;
			if (prefabDamage is null)
				prefabDamage = prefab.Damage;
		}

		protected void FixedUpdate() {
			//TODO: Shoot();

			/* if (!gameState.GameOver) {
				if (Time.time > nextShot) {
					Shoot();
					nextShot = Time.time + SHOOT_DELAY;
				}

			} */

			//TODO: OrbitAround();
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<Drone>();
		}

		public override void Attack() => Shoot();

		protected override void Shoot() {
			EnemyBullet bullet = Pool<EnemyBullet>.RequestObject();
			if (bullet is null) return;

			bullet.transform.position = bulletSpawnPoint.position;
			bullet.transform.rotation = bulletSpawnPoint.rotation;
			bullet.Damage = Damage;
			bullet.gameObject.SetActive(true);

			if (sfx?.clip != null) sfx.Play();
		}
	}
}
