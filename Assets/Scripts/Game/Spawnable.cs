using System.Collections.Generic;
using UnityEngine;
using ShootAR.Enemies;

namespace ShootAR
{
	[RequireComponent(typeof(Transform))]
	public abstract class Spawnable : MonoBehaviour
	{
		public const int GLOBAL_SPAWN_LIMIT = 50;

		[SerializeField] private float speed;
		/// <summary>
		/// The speed at which this object is moving.
		/// </summary>
		public float Speed {
			get { return speed; }
			set { speed = value; }
		}

		/// <summary>
		/// Reference to the object holding the game state.
		/// </summary>
		protected static GameState gameState;

		protected virtual void Start() {
			if (gameState is null)
				gameState = FindObjectOfType<GameState>();
		}
		/// <summary>
		/// Contains object pools that hold already instantiated objects ready
		/// to be used when requested.
		/// </summary>
		/// <typeparam name="T">
		/// The type of object to match to a pool
		/// </typeparam>
		public static class Pool<T> where T : Spawnable
		{
			internal static Stack<T> objectStack = new Stack<T>(GLOBAL_SPAWN_LIMIT);

			/// <summary>
			/// The number of objects available in the pool
			/// </summary>
			public static int Count { get => objectStack.Count; }

			/// <summary>
			/// Fill the appropriate pool with copies of
			/// <paramref name="referenceObject"/>.
			/// </summary>
			/// <param name="referenceObject"></param>
			/// <param name="lot">how many objects to add to the pool</param>
			public static void Populate(T referenceObject, int lot = GLOBAL_SPAWN_LIMIT) {
				if (objectStack.Count > 0)
					throw new UnityException("Trying to populate an already populated pool.");
				else
					for (int i = 0; i < lot; i++) {
						T spawnedObject = Instantiate(referenceObject);
						spawnedObject.gameObject.SetActive(false);
						objectStack.Push(spawnedObject);
					}
			}

			/// <summary>
			/// Fill the appropriate pool with copies of the object loaded from Resources.
			/// Which file is loaded is determined by the type of the pool.
			/// </summary>
			/// <param name="lot">how many objects to add to the pool</param>
			public static void Populate(int lot = GLOBAL_SPAWN_LIMIT) {
				string prefab = "";
				if (typeof(T) == typeof(Crasher))
					prefab = Prefabs.CRASHER;
				else if (typeof(T) == typeof(Drone))
					prefab = Prefabs.DRONE;
				else if (typeof(T) == typeof(ArmorCapsule))
					prefab = Prefabs.ARMOR_CAPSULE;
				else if (typeof(T) == typeof(HealthCapsule))
					prefab = Prefabs.HEALTH_CAPSULE;
				else if (typeof(T) == typeof(PowerUpCapsule))
					prefab = Prefabs.POWER_UP_CAPSULE;
				else if (typeof(T) == typeof(BulletCapsule))
					prefab = Prefabs.BULLET_CAPSULE;
				else if (typeof(T) == typeof(EnemyBullet))
					prefab = Prefabs.ENEMY_BULLET;
				else if (typeof(T) == typeof(Bullet))
					prefab = Prefabs.BULLET;

				Populate(
					Resources.Load<T>(prefab),
					lot
				);
			}

			/// <summary>
			/// Request an object of type <typeparamref name="T"/> from the
			/// appropriate pool.
			/// </summary>
			/// <remarks><list type="bullet">
			/// <item>Use <see cref="ResetState(Vector3, Quaternion)"/> before
			/// using an object to set its initial values.</item>
			/// <item>Returned object's <c>gameObject</c> need to be
			/// activated manually.</item>
			/// </list></remarks>
			/// <returns>
			/// reference to an available object of type <typeparamref name="T"/>
			/// </returns>
			public static T RequestObject() {
				if (objectStack.Count == 0) return null;

				return objectStack.Pop();
			}

			/// <summary>
			/// Dereference all objects contained in the pool.
			/// </summary>
			public static void Empty() {
				objectStack.Clear();
			}
		}

		/// <summary>
		/// Object is deactivated and returns to the pool to be used again when
		/// needed.
		/// </summary>
		/// <typeparam name="T">
		/// the type of pool the object will be returned to
		/// </typeparam>
		public void ReturnToPool<T>() where T : Spawnable {
			gameObject.SetActive(false);
			Pool<T>.objectStack.Push((T)this);
			ResetState();
		}

		/// <summary>
		/// Reset object to the default values.
		/// </summary>
		public abstract void ResetState();

		protected virtual void OnTriggerEnter(Collider other) {
			if (other.GetComponent<Bullet>())
				Destroy();
		}

		public abstract void Destroy();
	}
}
