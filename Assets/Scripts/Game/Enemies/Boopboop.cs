using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Class of Short-Ranged Enemy
	/// </summary>
	public abstract class Boopboop : Enemy
	{
		/// <summary>
		/// Cause damage to player.
		///
		/// Triggers on a successful attack against player.
		/// </summary>
		/// <param name="target">Player object</param>
		protected abstract void Harm(Player target);

		protected virtual void OnTriggerEnter(Collider other) {
			var target = other.GetComponent<Player>();
			if (target != null) Harm(target);
		}
	}
}
