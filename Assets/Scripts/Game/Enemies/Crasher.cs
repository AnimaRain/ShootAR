using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(SphereCollider))]
	public class Crasher : Boopboop
	{
		private const float DEFAULT_SPEED = 5F;
		private const int	DEFAULT_POINTS = 10,
							DEFAULT_DAMAGE = 1;

		private static Crasher prefab;

		protected override void Start() {
			base.Start();
			if (prefab is null)
				prefab = FindObjectOfType<PrefabContainer>()?.Crasher;
		}

		protected override void OnEnable() {
			base.OnEnable();
			MoveTo(Vector3.zero);
		}

		public override void ResetState() {
			Speed = prefab is null ? DEFAULT_SPEED : prefab.Speed;
			PointsValue = prefab is null ? DEFAULT_POINTS: prefab.PointsValue;
			Damage = prefab is null ? DEFAULT_DAMAGE : prefab.Damage;
		}

		public override void Destroy() {
			base.Destroy();
			ReturnToPool<Crasher>();
		}
	}
}
