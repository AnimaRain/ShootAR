using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(SphereCollider))]
	public class Crasher : Boopboop
	{
		private static float? prefabSpeed = null;
		private static ulong? prefabPoints = null;
		private static int? prefabDamage = null;

		protected override void Awake() {
			base.Awake();
			if (prefabSpeed is null || prefabPoints is null || prefabDamage is null) {
				Crasher prefab = Resources.Load<Crasher>(Prefabs.CRASHER);
				if (prefabSpeed is null)
					prefabSpeed = prefab.Speed;
				if (prefabPoints is null)
					prefabPoints = prefab.PointsValue;
				if (prefabDamage is null)
					prefabDamage = prefab.Damage;
			}
		}

		public override void ResetState() {
			base.ResetState();
			Speed = (float)prefabSpeed;
			PointsValue = (ulong)prefabPoints;
			Damage = (int)prefabDamage;
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<Crasher>();
		}

		public override void Attack() {
			transform.LookAt(Vector3.zero, Vector3.up);
			MoveTo(Vector3.zero);
		}

		protected override void Harm(Player target) {
			sfx?.Play();
			StopMoving();
			target.GetDamaged(Damage);

			/* When the enemy's model passes through the player, it looks ugly,
			 * so the enemy is teleported to a random spot behind the player. */
			if (Camera.main != null) {
				Vector3 cameraForward = Camera.main.transform.forward;
				transform.position =
					target.transform.position - cameraForward * 50f;

				transform.Translate(
					x: Random.Range(-25f, 25f) * cameraForward.x,
					y: Random.Range(-25f, 25f) * cameraForward.y,
					z: Random.Range(-25f, 25f) * cameraForward.z
				);
			}

			StopMoving();
		}

		protected void FixedUpdate() {
			if (AiEnabled) {
				if (!IsMoving) {
					Attack();
				} else {
					transform.LookAt(Vector3.zero, Vector3.up);
				}
			}
		}
	}
}
