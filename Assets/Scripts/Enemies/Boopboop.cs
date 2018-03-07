using UnityEngine;

/// <summary>
/// Short-Ranged class of Enemy
/// </summary>
public class Boopboop : Enemy
{
	/// <summary>
	/// Enemy rotates and moves towards player.
	/// </summary>
	protected void MoveToPlayer()
	{
		transform.LookAt(gameController.PlayerPosition);
		transform.forward = transform.position;
		GetComponent<Rigidbody>().velocity = transform.forward * Speed;
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.SendMessage("LoseHealth", Damage);
		}
	}
}
