﻿using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(SphereCollider))]
	public class Crasher : Boopboop
	{
		private static float? prefabSpeed = null;
		private static int? prefabPoints = null,
		                    prefabDamage = null;

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
			Speed = (float)prefabSpeed;
			PointsValue = (int)prefabPoints;
			Damage = (int)prefabDamage;
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<Crasher>();
		}

		public override void Attack() {
			MoveTo(Vector3.zero);
		}

		protected override void Harm(Player target) {
			sfx.Play();
			target.GetDamaged(Damage);

			if (Camera.main != null)
				transform.position =
					target.transform.position - Camera.main.transform.forward * 50f;

			StopMoving();
		}

		protected void FixedUpdate() {
			if (!IsMoving && AiEnabled) {
				transform.LookAt(Vector3.zero, Vector3.up);
				MoveTo(Vector3.zero);
			}
		}
	}
}
