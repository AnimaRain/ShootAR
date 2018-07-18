using UnityEngine;

namespace ShootAR
{
	/// <inheritdoc />
	/// <summary>
	/// Parent class of spawnable objects.
	/// </summary>
	public abstract class Spawnable : MonoBehaviour
	{
		/// <summary>
		/// The speed at which this object is moving.
		/// </summary>
		[SerializeField]
		private float speed;

		public float Speed
		{
			get
			{
				return speed;
			}

			set
			{
				speed = value;
			}
		}

		public struct Orbit
		{

			public Vector3 direction, centerPoint, perpendicularAxis;

			public Orbit(Vector3 direction, Vector3 centerPoint, Vector3 perpendicularAxis)
			{
				this.direction = direction;
				this.centerPoint = centerPoint;
				this.perpendicularAxis = perpendicularAxis;
			}
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
			Vector3 perpendicularAxis = Vector3.Cross(direction/2, clockwise ? Vector3.left : Vector3.right);
			return new Orbit(direction, centerPoint, perpendicularAxis);
		}

		/// <summary>
		/// Object orbits around a defined point by an angle based on its speed.
		/// </summary>
		/// <param name="orbit">The orbit to move in</param>
		protected void OrbitAround(Orbit orbit)
		{
			transform.LookAt(orbit.direction, orbit.perpendicularAxis);
			transform.RotateAround(orbit.direction, orbit.perpendicularAxis, Speed * Time.deltaTime);
		}
	}
}