using UnityEngine;
using System.Collections.Generic;

public partial class Delta : Pyoopyoo
{
	private const int LinkLimit = 2;
	private Delta[] Link;
	public static Vector3[] DeltaTrianglePoint;
	/// <summary>
	/// Tells if the Delta attack is allowed to be launched.
	/// All Delta enemies share the same value.
	/// </summary>
	public static bool DeltaAttackReady;
	public bool IsCharging;
	public delegate void GetInFormation();
	public static event GetInFormation Triangulate;

	public bool linkOpen;
	private int linkCounter;
	public bool inPosition;


	protected override void Start()
	{
		base.Start();

		DeltaTrianglePoint.Initialize();
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

			//When links with two other Deltas have been made, create the triangle
			if (!linkOpen)
			{
				/* Create the Delta Attack triangle:
				 * Find the mean of the distances of the three Deltas from
				 * the player. The mean is the distance ("d") from the player,
				 * while "a" is the distance between two Deltas.
				 * Choose a random point with "d" being the max distance, and
				 * create an equilateral triangle. The three vertices are the
				 * points where the Deltas will move before attacking. */

				float[] distance = new float[linkCounter];
				distance[0] = GetDistanceFromPlayer();
				for (int i = 0; i < linkCounter; i++)
				{
					distance[i + 1] = Link[i].GetDistanceFromPlayer();
				}
				float sum = 0;
				for (int i = 0; i < linkCounter; i++)
					sum = distance[i];
				float d = sum / linkCounter;    //distance from a vertex (Delta) to the center of the triangle (player).
				float a = d * Mathf.Sqrt(3);    //distance from a vertex (Delta) to another vertex of the triangle (linked Delta).

				//Create the triangle.
				for (int i = 0; i < linkCounter; i++)
				{
					do
					{
						DeltaTrianglePoint[i] = Random.insideUnitSphere * d;
					} while (Mathf.Sqrt(Mathf.Pow(DeltaTrianglePoint[i].x, 2) + Mathf.Pow(DeltaTrianglePoint[i].y, 2) + Mathf.Pow(DeltaTrianglePoint[i].z, 2)) >= 15f);
				}
			}
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

		//Delta attacks cannot happen simultaneously.
		DeltaAttackReady = false;
		//If this Delta is not linked with two other Deltas, then link.
		if (!linkOpen)
		{
			#region DeltaAttack
			
			/* When everyone is in position, start charging the attack. The
			 * time required for the charge is relevant to the distance from
			 * the player. */

			//The enemies move to the points to form the triangle.
			StartCoroutine(nameof(MoveTo), DeltaTrianglePoint[0]);
			for (int i = 0; i < linkCounter; i++)
			{
				Link[i].StartCoroutine(nameof(MoveTo), DeltaTrianglePoint[i]);

			}

			IsCharging = true;
			#endregion
		}
	}

	/// <summary>
	/// Returns the distance of the object from the player.
	/// Turns the object toward player and raycasts towards player.
	/// </summary>
	/// <returns>The distance from the player or infinity on fail</returns>
	private float GetDistanceFromPlayer()
	{
		transform.LookAt(gameController.playerPosition);
		RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);

		float res = Mathf.Infinity;
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
