using ShootAR;
using ShootAR.Enemies;
using UnityEngine;

public class OrbitTester : Enemy
{
	private Orbit orbit;

	protected override void Start() {
		base.Start();
		orbit = new Orbit(transform.position, Vector3.zero);
	}

	private void Update() {
		OrbitAround(orbit);
		Debug.DrawRay(orbit.centerPoint, orbit.perpendicularAxis, Color.blue);
		Debug.DrawLine(orbit.direction, orbit.centerPoint);
		Debug.DrawLine(transform.position, orbit.direction);
		Debug.DrawLine(transform.position, orbit.perpendicularAxis, Color.magenta);
	}

	public override void ResetState() {
		throw new System.NotImplementedException();
	}
}
