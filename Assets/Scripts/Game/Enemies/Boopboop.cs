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
			if (other.CompareTag("Player"))
			{
				Attack(other);
			}
		}

		public void Attack(Collider other)
		{
			sfx.Play();
			other.GetComponent<Player>().Health -= Controller.damage;
		}
	}
}