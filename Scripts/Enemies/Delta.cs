public class Delta : Pyoopyoo
{
	protected const int LinkLimit = 2;
	private Delta[] Link;

	public bool linkOpen;
	private int linkCounter;

	protected override void Start()
	{
		base.Start();

		linkOpen = true;
		Link = new Delta[2];
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

	/// <summary>
	/// Create link between this and otherDelta, and vice versa.
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
}