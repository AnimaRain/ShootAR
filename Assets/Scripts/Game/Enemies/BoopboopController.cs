using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Class of Short-Ranged Enemy
	/// </summary>
	public abstract class BoopboopController : EnemyController, IDamager
	{
		public Boopboop _ { get; set; }

		protected override void Start()
		{
			base.Start();
			_.Damager = this;
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			var target = other.GetComponent<Player>();
			if (target != null) Attack(target);
		}

		public virtual void Attack(Player target)
		{
			sfx.Play();
			target.Health -= _.damage;
		}
	}
}