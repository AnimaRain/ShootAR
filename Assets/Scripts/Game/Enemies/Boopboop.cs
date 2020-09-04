using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Class of Short-Ranged Enemy
	/// </summary>
	public abstract class Boopboop : Enemy
	{
		protected override void OnTriggerEnter(Collider other) {
			base.OnTriggerEnter(other);

			var target = other.GetComponent<Player>();
			if (target != null) Attack(target);
		}

		public virtual void Attack(Player target) {
			sfx.Play();
			target.GetDamaged(Damage);
		}
	}
}
