using UnityEngine;

/// <summary>
/// Parent class of spawnable objects.
/// </summary>
public class SpawnableObject : MonoBehaviour
{
	/// <summary>
	/// The speed at which this object is moving.
	/// </summary>
	public float Speed;

	protected GameController gameController;

	protected virtual void Awake()
    {
        if (gameController == null)
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

	/// <summary>
	/// Enemy moves towards a point using the physics engine.
	/// </summary>
	protected void MoveTo(Vector3 point)
	{
		transform.LookAt(point);
		transform.forward = -transform.position;
		GetComponent<Rigidbody>().velocity = transform.forward * Speed;
	}

	/// <summary>
	/// Returns a circular orbit around centerPoint with the range of the object's position vector.
	/// </summary>
	/// <param name="centerPoint">the center of the orbit</param>
	/// <param name="clockwise">the direction of the orbit</param>
	/// <returns>Orbit</returns>
	protected Orbit CalculateOrbit(Vector3 centerPoint, bool clockwise = true)
	{

		Vector3 direction = centerPoint - transform.position;
		Vector3 perpendicularAxis = Vector3.Cross(direction, clockwise ? Vector3.left : Vector3.right);
		return new Orbit(direction, perpendicularAxis);
	}

	/// <summary>
	/// Object orbits around a defined point by an angle based on its speed.
	/// </summary>
	/// <param name="orbit">The orbit to move in</param>
	protected void OrbitAround(Orbit orbit)
	{
		transform.LookAt(orbit.direction, orbit.perpendicularAxis);
		transform.RotateAround(orbit.direction, orbit.perpendicularAxis, Speed * Time.deltaTime);
#if DEBUG
		Debug.DrawRay(orbit.direction, orbit.perpendicularAxis, Color.blue);
		Debug.DrawLine(transform.position, orbit.direction);
		Debug.DrawLine(transform.position, orbit.perpendicularAxis, Color.red);
#endif
	}
}
