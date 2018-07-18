using ShootAR;
using UnityEngine;

public class OrbitTester : Spawnable
{
	private Orbit orbit;

	private void Start()
	{
		orbit = CalculateOrbit(Vector3.zero);
	}

	private void Update()
	{
		OrbitAround(orbit);
		Debug.DrawRay(orbit.centerPoint, orbit.perpendicularAxis, Color.blue);
		Debug.DrawLine(orbit.direction, orbit.centerPoint);
		Debug.DrawLine(transform.position, orbit.direction);
		Debug.DrawLine(transform.position, orbit.perpendicularAxis, Color.magenta);
	}
}
