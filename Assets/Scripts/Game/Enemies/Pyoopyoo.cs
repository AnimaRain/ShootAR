using UnityEngine;

/// <summary>
/// Long-Ranged class of Enemy
/// </summary>
public class Pyoopyoo: Enemy
{
    public EnemyBullet Bullet;
	/// <summary>
	/// the position where the bullets will get fired from
	/// </summary>
	public Transform BulletSpawnPoint;

    private EnemyBullet firedBullet;

    protected virtual void Update()
    {
		/* TODO: Make AI decide when to shoot, how to move, and targeting
         * the player.*/
		//Debug
		if (Time.realtimeSinceStartup >= 63) Shoot();
    }


    protected virtual void Shoot()
    {
        if (firedBullet == null)
        {
			sfx.Play();
			firedBullet = Instantiate(Bullet, BulletSpawnPoint.localPosition, BulletSpawnPoint.localRotation);
			firedBullet.Damage = Damage;
		}
    }
}