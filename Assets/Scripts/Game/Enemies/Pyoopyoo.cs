using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Long-Ranged class of Enemy
	/// </summary>
	public abstract class Pyoopyoo : Enemy
	{
		[SerializeField] protected EnemyBullet bullet;
		/// <summary>
		/// the position where the bullets will get fired from
		/// </summary>
		[SerializeField] protected Transform bulletSpawnPoint;

		/// <summary>
		/// The last bullet that was fired by this enemy
		/// </summary>
		protected EnemyBullet lastBullet;

		protected virtual void Shoot() {
			EnemyBullet bullet = Pool<EnemyBullet>.RequestObject();
			if (bullet is null) return;

			bullet.ResetState(
				position: bulletSpawnPoint.position,
				rotation: bulletSpawnPoint.rotation,
				speed: 5);

			bullet.Damage = Damage;

			sfx.Play();
		}
	}
}
