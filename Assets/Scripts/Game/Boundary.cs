using UnityEngine;

namespace ShootAR
{
	[System.Obsolete("Objects don't need to be destroyed anymore after the " +
		"introduction of object pools. Bullets themselves check their" +
		"distance from the player and then return to the pool.")]
	public class Boundary : MonoBehaviour
	{
		private void OnTriggerExit(Collider other) {
			// Destroy everything that leaves the trigger
			Destroy(other.gameObject);
		}
	}
}
