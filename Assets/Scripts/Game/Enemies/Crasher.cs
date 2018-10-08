using UnityEngine;

namespace ShootAR.Enemies
{
	[RequireComponent(typeof(SphereCollider))]
	public class Crasher : Boopboop
	{
		protected override void Start()
		{
			MoveTo(Vector3.zero);
		}
	}
}