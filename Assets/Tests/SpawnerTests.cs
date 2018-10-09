using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using ShootAR;
using ShootAR.Enemies;
using ShootAR.TestTools;

public class SpawnerTests
{

	[UnityTest]
	public IEnumerator SpawnerStopsWhenLimitReached()
	{

		Spawner spawner = Spawner.Create(
			objectToSpawn: TestEnemy.Create(),
			spawnLimit: 5,
			spawnRate: 1,
			maxDistanceToSpawn: 10f,
			minDistanceToSpawn: 3f,
			initialDelay: 0f
		);

		spawner.StartSpawning();
		yield return new WaitUntil(() => spawner.SpawnCount == spawner.SpawnLimit);

		Assert.That(!spawner.IsSpawning,
			"Spawner should stop spawining when the spawn limit is reached.");
	}

	[UnityTest]
	public IEnumerator SpawnerSpawnsTheCorrectNumberOfObjects()
	{
		Spawner spawner = Spawner.Create(
			objectToSpawn: TestEnemy.Create(),
			spawnLimit: 5,
			spawnRate: 1,
			maxDistanceToSpawn: 10f,
			minDistanceToSpawn: 3f,
			initialDelay: 0f
		);

		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.IsSpawning);

		int numberOfSpawned = Object.FindObjectsOfType<TestEnemy>().Length - 1;
		Assert.AreEqual(5, numberOfSpawned, "Should spawn 5 objects.");
	}

	[UnityTest]
	public IEnumerator SpawnerCanStopSpawning()
	{
		//Arrange
		Spawner spawner = Spawner.Create(TestEnemy.Create(),
			spawnLimit: 5, spawnRate: 1, initialDelay: 0f,
			maxDistanceToSpawn: 10f, minDistanceToSpawn: 3f);

		const int testLimit = 3;    //the # of spawns after which spawning will stop

		//Act
		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.SpawnCount < testLimit);
		spawner.StopSpawning();

		//Assert
		int numberOfSpawned = Object.FindObjectsOfType<TestEnemy>().Length - 1;
		Assert.AreEqual(testLimit, numberOfSpawned,
				$"Spawning should be interrupted after spawning {testLimit} objects.");
	}

	[UnityTest]
	public IEnumerator SpawnerStopsSpawningAtGameOver()
	{
		GameState gameState = GameState.Create(0);
		Spawner spawner = Spawner.Create(TestEnemy.Create(), 99, 0f, 1f, 5f, 3f, gameState);

		spawner.StartSpawning();
		yield return new WaitUntil(() => spawner.SpawnCount == 3);
		gameState.GameOver = true;
		yield return new WaitWhile(() => spawner.IsSpawning);

		Assert.AreEqual(3, spawner.SpawnCount,
			"Spawner should stop spawning at game over.");
	}

	[UnityTest]
	public IEnumerator ObjectsAreSpawnedInsideTheDesignatedArea()
	{
		/* This test is based on luck, making it pretty much unreliable.
		 * I feel confident enough that things workout, though.*/

		var prefab = TestEnemy.Create();
		Spawner spawner = Spawner.Create(prefab, 20, 0f, 0f, 5f, 3f);

		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.IsSpawning);
		Object.Destroy(prefab.gameObject);
		Enemy[] enemies = Object.FindObjectsOfType<Enemy>();
		foreach (var enemy in enemies)
		{
			if (enemy == spawner.ObjectToSpawn) continue;

			var distance = enemy.transform.position.magnitude;
			Assert.GreaterOrEqual(distance, spawner.MinDistanceToSpawn,
				"Objects should not be spawned below the minimum distance.");
			Assert.LessOrEqual(distance, spawner.MaxDistanceToSpawn,
				"Objects should not be spawned past the maximum distance.");
		}
	}

	[Test]
	public void SpawnerShouldNotBeRestartedWhileRunning()
	{
		Spawner spawner = Spawner.Create(
			TestEnemy.Create(), 100, 100f, 100f, 100f, 1f);
		UnityException error = null;

		try
		{
			spawner.StartSpawning();
			spawner.StartSpawning();
		}
		catch (UnityException e)
		{
			error = e;
		}

		Assert.IsNotNull(error,
			"Should not be able to restart a spawner while it is" +
			" currently spawning"
		);
	}
	[TearDown]
	public void ClearTestEnvironment()
	{
		GameObject[] objects = Object.FindObjectsOfType<GameObject>();
		foreach (var o in objects)
		{
			Object.Destroy(o.gameObject);
		}
	}
}
