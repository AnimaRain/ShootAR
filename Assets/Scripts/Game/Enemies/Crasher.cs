using UnityEngine;

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
			Crasher prefab = Resources.Load<Crasher>(Prefabs.CRASHER);
			if (prefabSpeed is null)
				prefabSpeed = prefab.Speed;
			if (prefabPoints is null)
				prefabPoints = prefab.PointsValue;
			if (prefabDamage is null)
				prefabDamage = prefab.Damage;
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
	}
}
