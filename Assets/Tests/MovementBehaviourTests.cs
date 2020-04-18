using NUnit.Framework;
using ShootAR;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;
using ShootAR.TestTools;

internal class MovementBehaviourTests : TestBase
{
	internal class MovementTestPoint : MonoBehaviour
	{
		public bool reached;
		public Vector3 recordedPosition;

		public static MovementTestPoint Create()
			=> new GameObject("Movement Test Point").AddComponent<MovementTestPoint>();

		private void Start() {
			var collider = gameObject.AddComponent<SphereCollider>();
			collider.radius = 1f;
			collider.isTrigger = true;
		}

		private void LateUpdate() {
			Debug.DrawLine(transform.position, Vector3.zero,
				reached ? Color.green : Color.red);
		}

		private void OnTriggerEnter(Collider other) {
			reached = true;
			recordedPosition = other.transform.position;
			Debug.Log($"Object passed the trigger at {recordedPosition}");
		}
	}

	[UnityTest]
	public IEnumerator MoveFromPointAToPointB() {
		TestEnemy hamster = TestEnemy.Create(2f);
		hamster.gameObject.SetActive(true);

		MovementTestPoint testPoint = MovementTestPoint.Create();
		testPoint.transform.position = new Vector3(10f, 10f, 10f);

		hamster.MoveTo(testPoint.transform.position);
		yield return new WaitWhile(() => hamster.IsMoving);

		Assert.That(testPoint.reached,
			"Test object must move to the designated test point.");
	}

	// TODO: Fix this test
	[UnityTest]
	[Ignore("Needs to be fixed.")]
	public IEnumerator OrbitCalculationValid() {
		//Arrange
		Vector3 startingPoint = new Vector3(5f, 5f, 5f);

		OrbitTester orbiter = Object.Instantiate(
			new GameObject("TestObject").AddComponent<OrbitTester>());
		orbiter.transform.Translate(startingPoint);
		/*FIXME: Fix this line
		 * orbiter.Speed = 100f; */
		var oCollider = orbiter.gameObject.AddComponent<SphereCollider>();
		oCollider.radius = 1f;

		MovementTestPoint endPoint = Object.Instantiate(
			new GameObject("End Point").AddComponent<MovementTestPoint>(),
			-startingPoint, Quaternion.LookRotation(startingPoint));

		//Act
		yield return new WaitUntil(() => endPoint.reached);

		//Assert
		Assert.That(
			Vector3EqualityComparer.Instance.Equals(
			endPoint.transform.position, endPoint.recordedPosition),
			"Points (5, 5, 5) and (-5,-5,-5) are both part of the orbit path.");
	}

	// UNDONE: Write the following test
	[UnityTest]
	[Ignore("Not yet written")]
	public IEnumerator FollowOrbitWhileMoving() {
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
