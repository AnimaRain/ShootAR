using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Long-Ranged class of Enemy
	/// </summary>
	public abstract class Pyoopyoo : Enemy
	{
		/// <summary>
		/// the position where the bullets will get fired from
		/// </summary>
		[SerializeField] protected Transform bulletSpawnPoint;

		/// <summary>
		/// Spawns an <see cref="EnemyBullet"/> to attack the player.
		/// </summary>
		protected abstract void Shoot();
	}
}
