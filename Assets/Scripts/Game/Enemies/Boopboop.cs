using UnityEngine;

/// <summary>
/// Short-Ranged class of Enemy
/// </summary>
public partial class Boopboop : Enemy
{

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			sfx.Play();
			other.SendMessage("LoseHealth", Damage);
		}
	}
}
