using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Class of Short-Ranged Enemy
	/// </summary>
	public abstract class Boopboop : Enemy
	{
		protected virtual void OnTriggerEnter(Collider other)
		{
			var target = other.GetComponent<Player>();
			if (target != null) Attack(target);
		}

		public virtual void Attack(Player target)
		{
			sfx.Play();
			target.GetDamaged(damage);
		}
	}
}