using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(SphereCollider))]
	public class Crasher : Boopboop
	{
		protected override void Start() {
			base.Start();

			MoveTo(Vector3.zero);
		}

		public override void ResetState() {
			throw new System.NotImplementedException();
		}

		public override void Destroy() {
			throw new System.NotImplementedException();
		}
	}
}
