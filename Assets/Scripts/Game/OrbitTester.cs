public class OrbitTester : SpawnableObject
{
	Orbit orbit;

	private void Start()
	{
		orbit = CalculateOrbit(UnityEngine.Vector3.zero);
	}

	private void Update()
	{
		OrbitAround(orbit);
	}
}
