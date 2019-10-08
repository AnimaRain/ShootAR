using System.Collections;
using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(CapsuleCollider))]
	public class Drone : Pyoopyoo
	{
		protected const float	ShootDelay = 5f,
								DEFAULT_SPEED = 10F;
		protected const int DEFAULT_POINTS = 20,
							DEFAULT_DAMAGE = 1;
		/// <summary>
		/// The point in time that the next shot is allowed.
		/// </summary>
		protected float nextShot;

		private static Drone prefab;

		public override void ResetState() {
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
			PointsValue = prefab is null ? DEFAULT_POINTS: prefab.PointsValue;
			Damage = prefab is null ? DEFAULT_DAMAGE : prefab.Damage;
		}

		protected override void Start() {
			base.Start();
			if (prefab is null)
				prefab = FindObjectOfType<PrefabContainer>()?.Drone;
		}

		protected void FixedUpdate() {
			if (!gameState.GameOver) {
				if (lastBullet == null && Time.time > nextShot) {
					Shoot();
					nextShot = Time.time + ShootDelay;
				}

				//OrbitAround();
			}
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<Drone>();
		}
	}
}
