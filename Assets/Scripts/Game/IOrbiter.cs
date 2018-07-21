namespace ShootAR
{
	public interface IOrbiter
	{
		void OrbitAround(Orbit orbit);

		void MoveTo(UnityEngine.Vector3 point);

		void MoveTo(float x, float y, float z);
	}
}
