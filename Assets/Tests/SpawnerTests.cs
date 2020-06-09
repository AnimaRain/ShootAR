using NUnit.Framework;
using ShootAR;
using ShootAR.Enemies;
using ShootAR.TestTools;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

internal class SpawnerTests : TestBase
{
	[UnityTest]
	public IEnumerator SpawnerFetchesFromPool() {
		Spawner spawner = Spawner.Create(
			objectToSpawn: typeof(BulletCapsule),
			spawnLimit: 1,
			initialDelay: 0,
			spawnRate: 1,
			maxDistanceToSpawn: 50,
			minDistanceToSpawn: 20
		);
		Spawnable.Pool<BulletCapsule>.Instance.Populate(
			BulletCapsule.Create(0, Player.Create())
		);

		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.IsSpawning);

		Assert.Less(Spawnable.Pool<BulletCapsule>.Instance.Count, Spawnable.GLOBAL_SPAWN_LIMIT,
				"Pool population is not diminishing.");
	}

	[UnityTest]
	public IEnumerator SpawnerStopsWhenLimitReached() {
		Spawnable.Pool<Crasher>.Instance.Populate(TestEnemy.Create());
		Spawner spawner = Spawner.Create(
			objectToSpawn: typeof(Crasher),
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
	public IEnumerator SpawnerSpawnsTheCorrectNumberOfObjects() {
		Spawnable.Pool<Crasher>.Instance.Populate(TestEnemy.Create());
		Spawner spawner = Spawner.Create(
					objectToSpawn: typeof(Crasher),
					spawnLimit: 5,
					spawnRate: 1,
					maxDistanceToSpawn: 10f,
					minDistanceToSpawn: 3f,
					initialDelay: 0f
				);

		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.IsSpawning);

		int numberOfSpawned = Object.FindObjectsOfType<TestEnemy>().Length;
		Assert.AreEqual(5, numberOfSpawned, "Should spawn 5 objects.");
	}

	[UnityTest]
	public IEnumerator SpawnerCanStopSpawning() {
		Spawnable.Pool<Crasher>.Instance.Populate(TestEnemy.Create());
		Spawner spawner = Spawner.Create(typeof(Crasher),
			spawnLimit: 5, spawnRate: 1, initialDelay: 0f,
			maxDistanceToSpawn: 10f, minDistanceToSpawn: 3f);

		const int testLimit = 3;    //the # of spawns after which spawning will stop

		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.SpawnCount < testLimit);
		spawner.StopSpawning();

		int numberOfSpawned = Object.FindObjectsOfType<TestEnemy>().Length;
		Assert.AreEqual(testLimit, numberOfSpawned,
				$"Spawning should be interrupted after spawning {testLimit} objects.");
	}

	[UnityTest]
	public IEnumerator SpawnerStopsSpawningAtGameOver() {
		Spawnable.Pool<Crasher>.Instance.Populate(TestEnemy.Create());
		GameState gameState = GameState.Create(0);
		Spawner spawner = Spawner.Create(typeof(Crasher), 99, 0f, 1f, 5f, 3f, gameState);

		spawner.StartSpawning();
		yield return new WaitUntil(() => spawner.SpawnCount == 3);
		gameState.GameOver = true;
		yield return new WaitWhile(() => spawner.IsSpawning);

		Assert.AreEqual(3, spawner.SpawnCount,
			"Spawner should stop spawning at game over.");
	}

	[UnityTest]
	public IEnumerator ObjectsAreSpawnedInsideTheDesignatedArea() {
		/* This test is based on luck, making it pretty much unreliable.
		 * I feel confident enough that things workout, though.*/

		var prefab = TestEnemy.Create();
		Spawnable.Pool<Crasher>.Instance.Populate(prefab);
		Spawner spawner = Spawner.Create(typeof(Crasher), 20, 0f, 0f, 5f, 3f);

		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.IsSpawning);
		Object.Destroy(prefab.gameObject);
		yield return new WaitForFixedUpdate();

		Enemy[] enemies = Object.FindObjectsOfType<Enemy>();
		foreach (var enemy in enemies) {
			var distance = enemy.transform.position.magnitude;

			Assert.GreaterOrEqual(distance, spawner.MinDistanceToSpawn,
				"Objects should not be spawned below the minimum distance.");
			Assert.LessOrEqual(distance, spawner.MaxDistanceToSpawn,
				"Objects should not be spawned past the maximum distance.");
		}
	}

	[UnityTest]
	public IEnumerator SpawnerShouldNotBeRestartedWhileRunning() {
		Spawner spawner = Spawner.Create(
			typeof(Crasher), 100, 100f, 100f, 100f, 1f);
		Spawnable.Pool<Crasher>.Instance.Populate(TestEnemy.Create());
		UnityException error = null;

		yield return null;
		try {
			spawner.StartSpawning();
			spawner.StartSpawning();
		} catch (UnityException e) {
			error = e;
		}

		Assert.IsNotNull(error,
			"Should not be able to restart a spawner while it is" +
			" currently spawning"
		);
	}


	/**<summary>
	 * <see cref="Spawner"/>s should yield their spawning behaviour when the
	 * global limit of allowed <see cref="Spawnable"/>s has been reached.
	 * </summary>
	 * <remarks>
	 * In order to maintain the performance as well as the playability of the
	 * game in acceptable levels, there should be an upper limit of how many
	 * <see cref="Spawnable"/>s of any kind can be spawned by the spawners.
	 * </remarks> */
	[UnityTest]
	public IEnumerator SpawnersHaveAGlobalSpawningLimit() {
		int limit = 100;    // Should be used only for the following check
							// and assigning spawnLimit.
		Assert.Less(Spawnable.GLOBAL_SPAWN_LIMIT, limit,
				$"Test is not set up correctly:\n" +
				$"The spawner's limit ({limit}) is lower than the " +
				$"global spawn limit ({Spawnable.GLOBAL_SPAWN_LIMIT})."
			);

		Spawnable.Pool<Crasher>.Instance.Populate(TestEnemy.Create());
		Spawner spawner = Spawner.Create(
			objectToSpawn: typeof(Crasher),
			spawnLimit: limit,
			initialDelay: 0f,
			spawnRate: 0f,
			maxDistanceToSpawn: 100f,
			minDistanceToSpawn: 0f
		);

		spawner.StartSpawning();
		yield return new WaitUntil(
			() => spawner.SpawnCount == Spawnable.GLOBAL_SPAWN_LIMIT);
		yield return new WaitForSecondsRealtime(.5f);

		Assert.That(spawner.IsSpawning, "Spawner should not stop spawning.");
		Assert.LessOrEqual(spawner.SpawnCount, Spawnable.GLOBAL_SPAWN_LIMIT,
			"Spawners must respect the global spawn limit."
		);
	}
}
