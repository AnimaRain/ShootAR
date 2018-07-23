using System;

namespace ShootAR.Enemies
{
	[Serializable]
	public class BoopboopController : EnemyController
	{
		public IDamager Damager { get; set; }

		public BoopboopController(float speed, int damage, int pointsValue)
			: base(speed, damage, pointsValue) { }

		public void Attack(UnityEngine.Collider other) => Damager.Attack(other);
	}
}