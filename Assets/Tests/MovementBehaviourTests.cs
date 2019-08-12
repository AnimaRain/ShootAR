using NUnit.Framework;
using ShootAR;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

internal class MovementBehaviourTests
{
	internal class EndPoint : MonoBehaviour
	{
		public bool testObjectPassedThePoint;
		public Vector3 recordedPosition;

		private void Start() {
			var collider = gameObject.AddComponent<SphereCollider>();
			collider.radius = 1f;
			collider.isTrigger = true;
		}

		private void LateUpdate() {
			Debug.DrawLine(transform.position, Vector3.zero,
				testObjectPassedThePoint ? Color.green : Color.red);
		}

		private void OnTriggerEnter(Collider other) {
			testObjectPassedThePoint = true;
			recordedPosition = other.transform.position;
			Debug.Log($"Object passed the trigger at {recordedPosition}");
		}
	}

	// TODO: Fix this test
	[UnityTest]
	[Ignore("Needs to be fixed.")]
	public IEnumerator OrbitCalculationValid()
	{
		//Arrange
		Vector3 startingPoint = new Vector3(5f, 5f, 5f);

		OrbitTester orbiter = Object.Instantiate(
			new GameObject("TestObject").AddComponent<OrbitTester>());
		orbiter.transform.Translate(startingPoint);
		/*FIXME: Fix this line
		 * orbiter.Speed = 100f; */
		var oCollider = orbiter.gameObject.AddComponent<SphereCollider>();
		oCollider.radius = 1f;

		EndPoint endPoint = Object.Instantiate(
			new GameObject("End Point").AddComponent<EndPoint>(),
			-startingPoint, Quaternion.LookRotation(startingPoint));

		//Act
		yield return new WaitUntil(() => endPoint.testObjectPassedThePoint);

		//Assert
		Assert.That(
			UnityEngine.TestTools.Utils.Vector3EqualityComparer.Instance.Equals(
			endPoint.transform.position, endPoint.recordedPosition),
			"Points (5, 5, 5) and (-5,-5,-5) are both part of the orbit path.");
	}

	// UNDONE: Write the following test
	[UnityTest]
	[Ignore("Not yet written")]
	public IEnumerator FollowOrbitWhileMoving()
	{
		yield return null;
	}

	[TearDown]
	public void CleanUp() {
		var objects = Object.FindObjectsOfType<GameObject>();
		foreach (var o in objects) {
			Object.Destroy(o.gameObject);
		}
	}
}
