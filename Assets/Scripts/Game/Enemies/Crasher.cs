using UnityEngine;

public class Crasher : Boopboop
{
	protected override void Start()
	{
		MoveTo(Vector3.zero);
	}
}