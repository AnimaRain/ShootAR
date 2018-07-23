using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Short-Ranged class of Enemy
	/// </summary>
	public abstract class Boopboop : Enemy, IDamager
	{
		public new BoopboopController Controller { get; set; }

		protected override void Start()
		{
			base.Start();
			Controller.Damager = this;
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			var target = other.GetComponent<Player>();
			if (target != null) Attack(target);
		}

		public virtual void Attack(Player target)
		{
			sfx.Play();
			target.Health -= Controller.damage;
		}
	}
}