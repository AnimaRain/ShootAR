using UnityEngine;

public struct Orbit {

	public Vector3 direction, perpendicularAxis;

	public Orbit(Vector3 direction, Vector3 perpendicularAxis)
	{
		this.direction = direction;
		this.perpendicularAxis = perpendicularAxis;
	}
}