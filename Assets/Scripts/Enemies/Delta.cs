using UnityEngine;

public class Delta : Pyoopyoo
{
	private const int LinkLimit = 2;
	private Delta[] Link;
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

		//Delta attacks cannot happen simultaneously.
		DeltaAttackReady = false;
		//If this Delta is not linked with two other Deltas, then link.
		if (!linkOpen)
		{
			#region DeltaAttack
			/* Find the mean of the distances of the three Deltas from
			 * the player. The mean is the distance ("d") from the player,
			 * while "a" being the distance between two Deltas.
			 * Choose a random point and by using those parameters, create 
			 * an equilateral triangle. The three vertices are the points,
			 * where the Deltas will move before attacking.
			 * When everyone is in position, start charging the attack. The
			 * time required for the charge is relevant to the distance from
			 * the player. */

			CreateDeltaAttackTriangle(this);

			Vector3[] point = new Vector3[linkCounter];
			for (int i = 0; i < linkCounter; i++)
			{
				do
				{
					point[i] = Random.insideUnitSphere * d;
				} while (Mathf.Sqrt(Mathf.Pow(point[i].x, 2) + Mathf.Pow(point[i].y, 2) + Mathf.Pow(point[i].z, 2)) >= 10);
			}

			//The enemies move to the points to form the triangle.
			StartCoroutine(nameof(MoveTo), point[0]);
			for (int i = 0; i < linkCounter; i++)
			{
				Link[i].StartCoroutine(nameof(MoveTo), point[i]);
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

	/// <summary>
	/// Create the triangle for the delta attack
	/// </summary>
	/// <param name="delta"></param>
	public static void CreateDeltaAttackTriangle(Delta delta)
	{
		int l = delta.linkCounter;

		float[] distance = new float[l];
		distance[0] = delta.GetDistanceFromPlayer();
		for (int i = 0; i < l; i++)
		{
			distance[i + 1] = delta.Link[i].GetDistanceFromPlayer();
		}
		float sum = 0;
		for (int i = 0; i < l; i++)
			sum = distance[i];
		float d = sum / l;    //distance from a vertice (Delta) to the center of the triangle (player).
		float a = d * Mathf.Sqrt(3);    //distance from a vertice (Delta) to another vertice of the triangle (linked Delta).
	}
}

public class DeltaTriangle
{
	public Vector3 Position { private set; get; }
	/// <summary>
	/// Distance from a vertice (Delta) to the center of the triangle (player).
	/// </summary>
	public Vector3 VerticeDistance { private set; get; }

	public DeltaTriangle(Vector3 position)
	{
		Position = position;
	}
}