using UnityEngine;

/// <summary>
/// Long-Ranged class of Enemy
/// </summary>
public partial class Pyoopyoo : Enemy
{
	[SerializeField]
	protected EnemyBullet Bullet;
	/// <summary>
	/// the position where the bullets will get fired from
	/// </summary>
	[SerializeField]
	protected Transform BulletSpawnPoint;

	/// <summary>
	/// The last bullet that was fired by this enemy
	/// </summary>
	protected EnemyBullet lastBullet;

	protected virtual void Shoot()
	{
		sfx.Play();
		lastBullet = Instantiate(Bullet, BulletSpawnPoint.position, BulletSpawnPoint.rotation);
		lastBullet.Damage = Damage;
	}
}