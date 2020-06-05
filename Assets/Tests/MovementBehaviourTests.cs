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

		public static MovementTestPoint Create(Vector3 position) {
			var o = Create();
			o.transform.position = position;
			return o;
		}

		private void Start() {
			var collider = gameObject.AddComponent<SphereCollider>();
			collider.radius = 0.1f;
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
		hamster.AiEnabled = false;
		hamster.gameObject.SetActive(true);

		MovementTestPoint testPoint = MovementTestPoint.Create(new Vector3(10f, 10f, 10f));

		hamster.MoveTo(testPoint.transform.position);
		yield return new WaitWhile(() => hamster.IsMoving);

		Assert.That(testPoint.reached,
			"Test object must move to the designated test point.");
	}

	[UnityTest]
	public IEnumerator MoveToPointBSkippingA() {
		MovementTestPoint pointA = MovementTestPoint.Create(new Vector3(5f, 5f, 5f));
		MovementTestPoint pointB = MovementTestPoint.Create(2 * pointA.transform.position);

		TestEnemy hamster = TestEnemy.Create(5f);
		hamster.AiEnabled = false;
		hamster.gameObject.SetActive(true);

		hamster.MoveTo(pointA.transform.position);
		hamster.MoveTo(pointB.transform.position);
		yield return new WaitWhile(() => hamster.IsMoving);

		Assert.True(pointA.reached, "Object did not pass through point A");
		Assert.True(pointB.reached, "Object did not reach the end.");
	}

	[UnityTest]
	public IEnumerator MoveToBTurningBeforeA() {
		MovementTestPoint pointA = MovementTestPoint.Create(new Vector3(5f, 0f, 5f));
		MovementTestPoint pointB = MovementTestPoint.Create(new Vector3(10f, 10f, 10f));

		TestEnemy hamster = TestEnemy.Create(5f);
		hamster.AiEnabled = false;
		hamster.gameObject.SetActive(true);

		hamster.MoveTo(pointA.transform.position);
		yield return new WaitUntil(
			() => hamster.transform.position.magnitude > pointA.transform.position.magnitude / 2);
		hamster.MoveTo(pointB.transform.position);
		yield return new WaitWhile(() => hamster.IsMoving);

		Assert.True(pointB.reached, "Object did not reach the end.");
		Assert.False(pointA.reached, "Object must not pass through point A");
	}

	[UnityTest]
	public IEnumerator IgnoreBumpOnOtherEnemy() {
		TestEnemy hamster = TestEnemy.Create(5f);
		hamster.AiEnabled = false;
		hamster.gameObject.SetActive(true);

		TestEnemy e = TestEnemy.Create(0f, 0, 0, 0f, 0f, 5f); // enemy blocking the path
		e.AiEnabled = false;
		e.gameObject.SetActive(true);

		Vector3 initialPosition = e.transform.position;

		MovementTestPoint endPoint = MovementTestPoint.Create(new Vector3(0f, 0f, 10f));

		hamster.MoveTo(endPoint.transform.position);
		yield return new WaitWhile(() => hamster.IsMoving);

		Assert.True(endPoint.reached);

		// Also assert that the other enemy was not bounced:
		Assert.That(Equals(initialPosition, e.transform.position));
	}

	[UnityTest]
	public IEnumerator MoveAroundPlayer() {
		TestEnemy hamster = TestEnemy.Create(5f,0,0,-5f);
		hamster.AiEnabled = false;
		hamster.gameObject.SetActive(true);

		Player.Create();

		MovementTestPoint endPoint = MovementTestPoint.Create(-hamster.transform.position);

		hamster.MoveTo(endPoint.transform.position);
		yield return new WaitWhile(() => hamster.IsMoving);

		Assert.True(endPoint.reached);
	}

	[UnityTest]
	public IEnumerator EveryoneMovesToTheSameSpot() {
		TestEnemy[] hamsters = new TestEnemy[15];
		hamsters[0] = TestEnemy.Create(3f, 0, 0, -4f, 0f, 5f);
		hamsters[1] = TestEnemy.Create(3f, 0, 0, -2f, 0f, 5f);
		hamsters[2] = TestEnemy.Create(3f, 0, 0, 0f, 0f, 5f);
		hamsters[3] = TestEnemy.Create(3f, 0, 0, 2f, 0f, 5f);
		hamsters[4] = TestEnemy.Create(3f, 0, 0, 4f, 0f, 5f);
		hamsters[5] = TestEnemy.Create(3f, 0, 0, -4f, 2f, 5f);
		hamsters[6] = TestEnemy.Create(3f, 0, 0, -2f, 2f, 5f);
		hamsters[7] = TestEnemy.Create(3f, 0, 0, 0f, 2f, 5f);
		hamsters[8] = TestEnemy.Create(3f, 0, 0, 2f, 2f, 5f);
		hamsters[9] = TestEnemy.Create(3f, 0, 0, 4f, 2f, 5f);
		hamsters[10] = TestEnemy.Create(3f, 0, 0, -4f, 4f, 5f);
		hamsters[11] = TestEnemy.Create(3f, 0, 0, -2f, 4f, 5f);
		hamsters[12] = TestEnemy.Create(3f, 0, 0, 0f, 4f, 5f);
		hamsters[13] = TestEnemy.Create(3f, 0, 0, 2f, 4f, 5f);
		hamsters[14] = TestEnemy.Create(3f, 0, 0, 4f, 4f, 5f);

		MovementTestPoint gatheringPoint = MovementTestPoint.Create();

		foreach (var hamster in hamsters) {
			hamster.gameObject.SetActive(true);
			hamster.AiEnabled = false;
			hamster.MoveTo(gatheringPoint.transform.position);
		}

		yield return new WaitForSecondsRealtime(3f);

		bool allMovementStoped;
		do {
			allMovementStoped = false;
			foreach (var hamster in hamsters)
				if (hamster.IsMoving) {
					allMovementStoped = true;
					break;
				}
			yield return new WaitForFixedUpdate();
		} while (allMovementStoped && gatheringPoint.reached);
	}

	[UnityTest, Ignore("Needs fixing")]
	public IEnumerator OrbitAroundZero() {
		Vector3 startingPoint = new Vector3(5f, 5f, 5f);

		OrbitTester orbiter = OrbitTester.Create(new Orbit(startingPoint, Vector3.zero), 2f);
		orbiter.gameObject.SetActive(true);

		MovementTestPoint endPoint = MovementTestPoint.Create();
		endPoint.transform.position = -startingPoint;

		yield return new WaitUntil(() => endPoint.reached);
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
