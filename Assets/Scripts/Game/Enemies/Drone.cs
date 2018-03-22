using UnityEngine;

public class Drone : Pyoopyoo
{
	private const float ShootDelay = 5f;

	protected void Update()
	{
		if (lastBullet == null)
		{
			Invoke(nameof(Shoot), 5f);
		}
	}

	protected void FixedUpdate()
	{
		OrbitAround(Vector3.zero);
	}
}
