using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(SphereCollider))]
	public class Crasher : Boopboop
	{
		protected override void Start()
		{
			base.Start();

			MoveTo(Vector3.zero);
		}
	}
}