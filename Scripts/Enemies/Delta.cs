using System;
using UnityEngine;

public class Delta : Pyoopyoo
{
	protected const int LinkLimit = 2;
	private Delta[] Link;

	public bool linkOpen;
	private int linkCounter;

	/// <summary>
	/// Tells if the Delta attack is allowed to be launched.
	/// All Delta enemies share the same value.
	/// </summary>
	public static bool DeltaAttackReady;


	protected override void Start()
	{
		base.Start();

		linkOpen = true;
		Link = new Delta[2];
		LinkWithDeltas();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		foreach (Delta link in Link)
			DelinkWith(link);
	}


	/// <summary>
	/// Create a link between this and otherDelta, and vice versa.
	/// </summary>
	/// <param name="otherDelta">The object to link with</param>
	/// <param name="linkBack">Whether otherDelta will link back. Default: true</param>
	public void LinkWith(Delta otherDelta, bool linkBack = true)
	{
		if (linkOpen)
		{
			//Create link with the otherDelta.
			Link[linkCounter++] = otherDelta;
			if (linkCounter >= LinkLimit) linkOpen = false;
			//When all link spots are filled, ask the other linked Deltas to link together as well.
			if (!linkOpen) Link[0].LinkWith(otherDelta);

			//Ask otherDelta to link back.
			if (linkBack) otherDelta.LinkWith(this, false);
		}
	}

	/// <summary>
	/// Destroy the link between this and otherDelta, and vice versa.
	/// </summary>
	/// <param name="otherDelta">The object to link with</param>
	/// <param name="delinkBack">Whether otherDelta will link back. Default: true</param>
	public void DelinkWith(Delta otherDelta, bool delinkBack = true)
	{
		for (int i = 0; i < linkCounter; i++)
			if (Link[i] == otherDelta)
			{
				//Ask otherDelta to delink with this as well.
				if (delinkBack) Link[i].DelinkWith(this, false);
				//Destroy link
				Link[i] = null;
				linkCounter--;
				linkOpen = true;
			}
	}

	/// <summary>
	/// Look for other Delta type enemies and create links with them.
	/// </summary>
	public void LinkWithDeltas()
	{
		Delta[] others = FindObjectsOfType<Delta>();

		/* Find other Delta type enemies and create links among this
		 * and two others. */
		foreach (Delta otherDelta in others)
		{
			//Check if otherDelta is not this object and it has open links left.
			if (otherDelta != this) LinkWith(otherDelta);
			if (!linkOpen) break;
		}
	}

	protected override void SpecialAttack()
	{
		base.SpecialAttack();

		DeltaAttackReady = false;
		if (linkOpen) LinkWithDeltas();

		float[] distance = new float[3];
		distance[0] = GetDistanceFromPlayer();
		for (int i = 0; i < Link.Length; i++)
		{
			Link[i].GetDistanceFromPlayer();
		}
		float d = a / Math.Sqrt(3);
	}

	private float GetDistanceFromPlayer()
	{
		transform.LookAt(gameController.playerPosition);
		RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);

		float res = 0;
		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.CompareTag("Player"))
			{
				res = hit.distance;
				break;
			}
		}
		return res;
	}
}