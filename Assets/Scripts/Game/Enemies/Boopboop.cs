using UnityEngine;

/// <summary>
/// Short-Ranged class of Enemy
/// </summary>
public class Boopboop : Enemy
{
	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			sfx.Play();
			other.GetComponent<Player>().Health -= damage;
		}
	}
}
