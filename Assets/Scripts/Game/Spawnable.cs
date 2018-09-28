/* The reason why this class exists is because Spawner.objectToSpawn can not
 * be assigned an interface through Unity's Inspector. Best solution was to
 * use a class that is parent of every object that can be spawned from a
 * spawner. */

using UnityEngine;

namespace ShootAR
{
	[RequireComponent(typeof(SphereCollider),typeof(Rigidbody))]
	public abstract class Spawnable : MonoBehaviour
	{
		/// <summary>
		/// The speed at which this object is moving.
		/// </summary>
		[SerializeField] private float speed;
		public float Speed {
			get { return speed; }
			protected set { speed = value; }
		}
	}
}