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

    protected override void FixedUpdate()
    {
		base.FixedUpdate();
		if (firedBullet != null && firedBullet.hit) Attack();
    }


    protected virtual void Shoot()
    {
        if (firedBullet == null)
        {
            firedBullet = Instantiate(Bullet, transform.forward * 10, transform.rotation);
        }
    }

	/// <summary>
	/// Returns the distance of the object from the player.
	/// (The object turns towards player.)
	/// </summary>
	/// <returns>The distance from the player or infinity on fail</returns>
	protected float GetDistanceFromPlayer()
	{
		transform.LookAt(gameController.playerPosition);
		RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);

		float res = Mathf.Infinity;
		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.CompareTag("Player"))
			{
				res = hit.distance;
				break;
			}
		}
		return res;
	}
}