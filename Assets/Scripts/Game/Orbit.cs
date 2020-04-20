using UnityEngine;

namespace ShootAR
{
	public struct Orbit
	{
		public Vector3 direction, centerPoint, perpendicularAxis;

		public Orbit(Vector3 direction, Vector3 centerPoint, Vector3 perpendicularAxis) {
			this.direction = direction;
			this.centerPoint = centerPoint;
			this.perpendicularAxis = perpendicularAxis;
		}

		/// <summary>
		/// Create a circular orbit around <see cref="centerPoint"/> with the radius of <see cref="point"/>'s magnitude.
		/// </summary>
		/// <param name="point">a point on the orbit</param>
		/// <param name="centerPoint">the center of the orbit</param>
		/// <param name="clockwise">the direction of the orbit. true = clockwise; true if omitted</param>
		public Orbit(Vector3 point, Vector3 centerPoint, bool clockwise = true) {
			direction = centerPoint - point;
			this.centerPoint = centerPoint;
			perpendicularAxis = Vector3.Cross(direction / 2, clockwise ? Vector3.left : Vector3.right);
		}
	}
}
