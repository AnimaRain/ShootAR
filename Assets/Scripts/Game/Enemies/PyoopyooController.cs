using UnityEngine;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Long-Ranged class of Enemy
	/// </summary>
	public abstract class PyoopyooController : EnemyController
	{
		[SerializeField] protected EnemyBulletController bullet;
		/// <summary>
		/// the position where the bullets will get fired from
		/// </summary>
		[SerializeField] protected Transform bulletSpawnPoint;

		/// <summary>
		/// The last bullet that was fired by this enemy
		/// </summary>
		protected EnemyBulletController lastBullet;

		protected virtual void Shoot()
		{
			sfx.Play();
			lastBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
			lastBullet._.damage = Base.damage;
		}
	}
}