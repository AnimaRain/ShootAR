namespace ShootAR.Enemies
{
	public class Drone : Pyoopyoo
	{
		private const float ShootDelay = 5f;

		protected void Update()
		{
			if (lastBullet == null)
			{
				Invoke(nameof(Shoot), ShootDelay);
			}
		}

		protected void FixedUpdate()
		{
			//OrbitAround();
		}
	}
}