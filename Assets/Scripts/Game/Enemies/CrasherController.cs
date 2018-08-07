using UnityEngine;

namespace ShootAR.Enemies
{
	public class CrasherController : BoopboopController
	{
		protected override void Start()
		{
			MoveTo(Vector3.zero);
		}
	}
}