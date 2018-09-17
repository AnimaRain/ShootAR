using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using ShootAR;
using ShootAR.Enemies;

public class SpawnerTests
{

	private class TestObject : Enemy
	{
		public static TestObject Create(float speed, int damage, int pointsValue, float x = 0, float y = 0, float z = 0)
		{
			var o = new GameObject(nameof(TestObject)).AddComponent<TestObject>();
			o.Speed = speed;
			o.Damage = damage;
			o.PointsValue = pointsValue;
			o.transform.position = new Vector3(x, y, z);
			return o;
		}

		protected override void OnDestroy() { return; }
	}

	[UnityTest]
	public IEnumerator SpawnerStopsWhenLimitReached()
	{

		Spawner spawner = Spawner.Create(
			objectToSpawn: TestObject.Create(0,0,0),
			spawnLimit: 5,
			spawnRate: 1,
			maxDistanceToSpawn: 10f,
			minDistanceToSpawn: 3f
		);

		spawner.StartSpawning();
		yield return new WaitUntil(() => spawner.SpawnCount == spawner.SpawnLimit);

		Assert.That(spawner.IsSpawning == false, "Spawner stops when the spawn" +
			" limit is reached.");
	}

	[UnityTest]
	public IEnumerator SpawnerSpawnsTheCorrectNumberOfObjects()
	{
		Spawner spawner = Spawner.Create(
			objectToSpawn: new GameObject("SpawnableObject").AddComponent<TestObject>(),
			spawnLimit: 5,
			spawnRate: 1,
			maxDistanceToSpawn: 10f,
			minDistanceToSpawn: 3f
		);

		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.IsSpawning);

		int numberOfSpawned = Object.FindObjectsOfType<TestObject>().Length - 1;
		Assert.AreEqual(5, numberOfSpawned, "Spawn 5 objects.");
	}

	[UnityTest]
	public IEnumerator SpawnerCanStopSpawning()
	{
		//Arrange
		Spawner spawner = Spawner.Create(
			new GameObject("Spawnable").AddComponent<TestObject>(),
			spawnLimit: 5, spawnRate: 1,
			maxDistanceToSpawn: 10f, minDistanceToSpawn: 3f);

		int testLimit = 3;	//the # of spawns after which spawning will stop

		//Act
		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.SpawnCount < testLimit);
		spawner.StopSpawning();

		//Assert
		int numberOfSpawned = Object.FindObjectsOfType<TestObject>().Length - 1;
		Assert.AreEqual(testLimit, numberOfSpawned,
				$"Interrupt spawning after spawning {testLimit} objects.");
	}

	[UnityTest]
	public IEnumerator SpawnerStopsSpawningAtGameOver()
	{
		Spawner spawner = Spawner.Create(TestObject.Create(0, 0, 0), 99, 1, 5, 3);
		GameManager gm = GameManager.Create(Player.Create(3));

		spawner.StartSpawning();
		yield return new WaitForSecondsRealtime(3f);
		gm.GameOver = true;

		Assert.That(!spawner.IsSpawning, "Spawner stops spawning at game over.");
	}

	[UnityTest]
	public IEnumerator ObjectsAreSpawnedInsideTheDesignatedArea()
	{
		yield return null;
	}

	private class TestShooter : Pyoopyoo { private new void OnDestroy() { } }
	private class TestMeleer : Boopboop { private new void OnDestroy() { } }

	[Test]
	public void MultipleSpawnersOnSceneHaveDistinctTypesOfSpawnables()
	{
		var testObject1 = new GameObject().AddComponent<Capsule>();
		var testObject2 = new GameObject().AddComponent<TestMeleer>();
		var testObject3 = new GameObject().AddComponent<TestShooter>();

		Spawner spawner1 = Spawner.Create(testObject1, 1, 1, 2, 1);
		Spawner spawner2 = Spawner.Create(testObject2, 1, 1, 2, 1);
		Spawner spawner3 = Spawner.Create(testObject3, 1, 1, 2, 1);

		Assert.IsInstanceOf(typeof(Capsule), spawner1.ObjectToSpawn,
			$"{nameof(spawner1)}'s object should be of type {typeof(Capsule)}.");
		Assert.IsInstanceOf(typeof(TestMeleer), spawner2.ObjectToSpawn,
			$"{nameof(spawner2)}'s object should be of type {typeof(TestMeleer)}.");
		Assert.IsInstanceOf(typeof(TestShooter), spawner3.ObjectToSpawn,
			$"{nameof(spawner3)}'s object should be of type {typeof(TestShooter)}.");
	}

	[TearDown]
	public void ClearTestEnvironment()
	{
		TestObject[] objects = Object.FindObjectsOfType<TestObject>();
		foreach (var o in objects)
		{
			Object.Destroy(o.gameObject);
		}

		Spawner[] spawners = Object.FindObjectsOfType<Spawner>();
		foreach (var s in spawners)
		{
			Object.Destroy(s.gameObject);
		}
	}
}
 