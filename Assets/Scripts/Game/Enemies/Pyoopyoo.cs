using UnityEngine;

/// <summary>
/// Long-Ranged class of Enemy
/// </summary>
public class Pyoopyoo: Enemy
{
    public EnemyBullet Bullet;

    private EnemyBullet firedBullet;

    protected virtual void Update()
    {
        /* TODO: Make AI decide when to shoot, how to move, and targeting
         * the player.*/
        Shoot();
    }


    protected virtual void Shoot()
    {
        if (firedBullet == null)
        {
			firedBullet = Instantiate(Bullet, transform.forward * 10, transform.rotation);
			firedBullet.Damage = Damage;
		}
    }
}