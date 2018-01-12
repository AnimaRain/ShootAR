using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public bool hit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) hit = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}