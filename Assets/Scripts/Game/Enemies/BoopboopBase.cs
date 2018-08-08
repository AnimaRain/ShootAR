using System;

namespace ShootAR.Enemies
{
	[Serializable]
	public class BoopboopBase : EnemyBase
	{
		public IDamager Damager { get; set; }

		public BoopboopBase(float speed, int damage, int pointsValue)
			: base(speed, damage, pointsValue) { }

		public void Attack(UnityEngine.Collider other) => Damager.Attack(other);
	}
}