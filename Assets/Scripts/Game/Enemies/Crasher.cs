using UnityEngine;

namespace ShootAR.Enemies
{
	public class Crasher : Boopboop
	{
		protected override void Start()
		{
			MoveTo(Vector3.zero);
		}
	}
}