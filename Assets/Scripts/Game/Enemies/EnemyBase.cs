namespace ShootAR.Enemies
{
	[System.Serializable]
	public class EnemyBase
	{
		/// <summary>
		/// The speed at which this object is moving.
		/// </summary>
		public float Speed { get; set; }
		/// <summary>
		/// The amount of points added to the player's score when destroyed.
		/// </summary>
		public int PointsValue { get; }
		/// <summary>
		/// The amount of damage the player recieves from this object's attack.
		/// </summary>
		[UnityEngine.Range(-Player.HEALTH_MAX, Player.HEALTH_MAX), UnityEngine.SerializeField]
		public int damage;
		public int Damage { get { return damage; } set { damage = value; } }

		public IOrbiter Orbiter { get; set; }

		protected void MoveTo(float x, float y, float z) => Orbiter.MoveTo(x, y, z);
		protected void OrbitAround(Orbit orbit) => Orbiter.OrbitAround(orbit);

		public EnemyBase(float speed, int damage, int pointsValue)
		{
			Speed = speed;
		}
	}
}
