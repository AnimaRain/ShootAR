using System.Collections.Generic;
using UnityEngine;

namespace ShootAR
{
	public abstract class Spawnable : MonoBehaviour
	{
		public const int GLOBAL_SPAWN_LIMIT = 50;

		[SerializeField] private float speed;
		/// <summary>
		/// The speed at which this object is moving.
		/// </summary>
		public float Speed {
			get { return speed; }
			protected set { speed = value; }
		}

		/// <summary>
		/// Reference to the object holding the game state.
		/// </summary>
		protected GameState gameState;
		/// <summary>
		/// Setter for <see cref="Spawner"/>s to set a spawned object's game state.
		/// </summary>
		public GameState GameState {
			set { gameState = value; }
		}

		/// <summary>
		/// A container of object pools.
		/// </summary>
		private static Dictionary<System.Type, Stack<Spawnable>> pools;

		/// <summary>
		/// Get the types of available object pools.
		/// </summary>
		/// <returns>an array containing the types</returns>
		public static System.Type[] GetPools() {
			System.Type[] types = new System.Type[pools.Count];
			pools.Keys.CopyTo(types, 0);
			return types;
		}

		/// <summary>
		/// Request an object of type <typeparamref name="T"/> from the
		/// appropriate pool.
		/// </summary>
		/// <typeparam name="T">The type of object to match to a pool</typeparam>
		/// <returns>
		/// reference to an available object of type <typeparamref name="T"/>
		/// </returns>
		public static T RequestObject<T>() where T : Spawnable {
			//TODO: Check if there are available objects.
			//TODO: Expand and repopulate pool if necessary.
			T availableObject = (T)pools[typeof(T)].Pop();
			//TODO: Create Initialize method
			availableObject.ResetState();
			return availableObject;
		}

		/// <summary>
		/// Instead of destroying the object, deactivates and returns it to the pool
		/// to be used again when needed.
		/// </summary>
		public void ReturnToPool() {
			gameObject.SetActive(false);
			pools[GetType()].Push(this);
		}

		/// <summary>
		/// Return the object to an "as new" state so it can be used again.
		/// </summary>
		protected virtual void ResetState() {}
	}
}
