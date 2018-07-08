using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using ShootAR;

public class SpawnerTests {

	[UnityTest]
	public IEnumerator SpawnerStopsWhenLimitReached() {

		Spawner spawner = new GameObject("Spawner").AddComponent<Spawner>();
		spawner.Initialize(
			objectToSpawn: new GameObject("SpawnableObject").AddComponent<SpawnableObject>(),
			spawnLimit: 5,
			spawnRate: 1
		);

		spawner.StartSpawning();
		yield return new WaitUntil(() => spawner.SpawnCount == spawner.SpawnLimit);

		Assert.That(spawner.IsSpawning == false, "Spawner stops when the spawn" +
			" limit is reached.");
	}

	[UnityTest]
	public IEnumerator SpawnerSpawnsTheCorrectNumberOfObjects()
	{
		Spawner spawner = new GameObject("Spawner").AddComponent<Spawner>();
		spawner.Initialize(
			objectToSpawn: new GameObject("SpawnableObject").AddComponent<SpawnableObject>(),
			spawnLimit: 5,
			spawnRate: 1
		);

		spawner.StartSpawning();
		yield return new WaitWhile(() => spawner.IsSpawning);

		int numberOfSpawned = Object.FindObjectsOfType<SpawnableObject>().Length - 1;
		Assert.AreEqual(5, numberOfSpawned, "Spawned 5 objects as expected.");
	}
}