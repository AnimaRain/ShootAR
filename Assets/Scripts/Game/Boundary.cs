using UnityEngine;

public class Boundary : MonoBehaviour
{
	private void OnTriggerExit(Collider other)
    {
        // Destroy everything that leaves the trigger
        Destroy(other.gameObject);
    }
}