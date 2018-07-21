using UnityEngine;

namespace ShootAR
{
	/// <inheritdoc />
	/// <summary>
	/// Parent class of spawnable objects.
	/// </summary>
	public class Spawnable : MonoBehaviour, IOrbiter
	{
		private _Spawnable @base;
		//[SerializeField] private float speed;

		public _Spawnable Self { get { return @base; } }

		public static Spawnable Create(float speed,
			float x = 0f, float y = 0f, float z = 0f)
		{
			var o = new GameObject("Spawnable").AddComponent<Spawnable>();
			o.@base = new _Spawnable(speed);
			o.transform.position = new Vector3(x, y, z);
			return o;
		}

		protected void OnEnable()
		{
			@base.Orbiter = this;
		}

		/// <summary>
		/// Enemy moves towards a point using the physics engine.
		/// </summary>
		public void MoveTo(Vector3 point)
		{
			transform.LookAt(point);
			transform.forward = -transform.position;
			GetComponent<Rigidbody>().velocity = transform.forward * @base.Speed;
		}

		public void MoveTo(float x, float y, float z)
		{
			Vector3 point = new Vector3(x, y, z);
			MoveTo(point);
		}

		/// <summary>
		/// Object orbits around a defined point by an angle based on its speed.
		/// </summary>
		/// <param name="orbit">The orbit to move in</param>
		public void OrbitAround(Orbit orbit)
		{
			transform.LookAt(orbit.direction, orbit.perpendicularAxis);
			transform.RotateAround(orbit.direction, orbit.perpendicularAxis, @base.Speed * Time.deltaTime);
		}
	}
}