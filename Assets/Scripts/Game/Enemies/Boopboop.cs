using UnityEngine;

/// <summary>
/// Short-Ranged class of Enemy
/// </summary>
public class Boopboop : Enemy
{
	protected override void Start()
	{
		base.Start();

		MoveToPlayer();
	}

	/// <summary>
	/// Enemy rotates and moves towards player.
	/// </summary>
	protected void MoveToPlayer()
	{
		transform.LookAt(Vector3.zero);
		transform.forward = transform.position;
		GetComponent<Rigidbody>().velocity = transform.forward * Speed;
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			sfx.Play();
			other.SendMessage("LoseHealth", Damage);
		}
	}
}
