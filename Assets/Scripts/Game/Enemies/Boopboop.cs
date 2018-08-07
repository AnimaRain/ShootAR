using System;

namespace ShootAR.Enemies
{
	[Serializable]
	public class Boopboop : Enemy
	{
		public IDamager Damager { get; set; }

		public Boopboop(float speed, int damage, int pointsValue)
			: base(speed, damage, pointsValue) { }

		public void Attack(UnityEngine.Collider other) => Damager.Attack(other);
	}
}