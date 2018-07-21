namespace ShootAR
{
	[System.Serializable]
	public class _Spawnable
	{
		/// <summary>
		/// The speed at which this object is moving.
		/// </summary>
		public float Speed { get; set; }

		public IOrbiter Orbiter { get; set; }

		protected void MoveTo(float x, float y, float z) => Orbiter.MoveTo(x, y, z);
		protected void OrbitAround(Orbit orbit) => Orbiter.OrbitAround(orbit);

		public _Spawnable(float speed)
		{
			Speed = speed;
		}
	}
}
