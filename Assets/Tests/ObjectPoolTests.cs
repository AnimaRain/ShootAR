using NUnit.Framework;
using ShootAR;
using UnityEngine.TestTools;
using ShootAR.TestTools;
using ShootAR.Enemies;

public class ObjectPoolTests : TestBase
{
	[Test]
	public void NoAvailableObjectToFetch() {
		const int limit = 3;
		Spawnable.Pool<Crasher>.Instance.Populate(TestEnemy.Create(0), limit);

		for (int i = 0; i < limit; i++)
			Spawnable.Pool<Crasher>.Instance.RequestObject();

		Assert.IsNull(Spawnable.Pool<Crasher>.Instance.RequestObject(),
				"A null reference should have been returned.");
	}

	//TODO: Should this be brought back or removed?
	[UnityTest, Ignore("Deprecated functionality")]
	public System.Collections.IEnumerator CreateNewObjectWhenPoolEmpty() {
		const int limit = 3;

		var objectToSpawn = TestEnemy.Create(0);
		Spawner spawner = Spawner.Create(
				typeof(Crasher), limit, 0, 0, 10, 0);
		Spawnable.Pool<Crasher>.Instance.Populate(objectToSpawn, limit);

		spawner.StartSpawning();
		yield return new UnityEngine.WaitWhile(() => spawner.IsSpawning);
		spawner.StartSpawning(typeof(Crasher), 1, 0, 0, 0, 10);
		yield return new UnityEngine.WaitWhile(() => spawner.IsSpawning);

		int expectedValue = limit + 1;
		/* Normal Destroy does not destroy the object before the assertion happens.
		 * Using DestroyImmediate instead. */
		UnityEngine.Object.DestroyImmediate(objectToSpawn.gameObject);
		int actualValue = UnityEngine.Object.FindObjectsOfType<TestEnemy>().Length;

		Assert.AreEqual(expectedValue, actualValue,
				$"Expected {expectedValue} spawned objects but got {actualValue}.");
	}

	[Test]
	public void ObjectReturnsToPool() {
		Spawnable.Pool<Crasher>.Instance.Populate(TestEnemy.Create(0), 1);
		TestEnemy testObject = Spawnable.Pool<Crasher>.Instance.RequestObject() as TestEnemy;

		testObject.ReturnToPool<Crasher>();

		Assert.AreEqual(1, Spawnable.Pool<Crasher>.Instance.Count,
				"No object available in the pool.");
		Assert.IsNotNull(Spawnable.Pool<Crasher>.Instance.RequestObject(),
				"No object available to return.");
	}
}
