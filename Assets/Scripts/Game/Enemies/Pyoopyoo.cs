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

		protected virtual void Shoot() {
			EnemyBullet bullet = Pool<EnemyBullet>.RequestObject();
			if (bullet is null) return;

			bullet.transform.position = bulletSpawnPoint.position;
			bullet.transform.rotation = bulletSpawnPoint.rotation;
			bullet.Damage = Damage;

			sfx.Play();
		}
	}
}
