using NUnit.Framework;
using ShootAR;
using ShootAR.Enemies;
using ShootAR.TestTools;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;

public class GameStateTests : PatternsTestBase
{
	[UnityTest]
	public IEnumerator UseLastShotToHitCapsuleAndTakeBullets() {
		GameState gameState = GameState.Create(0);
		Camera camera = new GameObject().AddComponent<Camera>();
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: camera,
			ammo: 1,
			gameState: gameState);

		string[] data = new string[] {
			"<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
			"",
			"<spawnerconfiguration>",
			"\t<level>",
			"\t\t<spawnable type=\"Crasher\">",
			"\t\t\t<pattern>",
			"\t\t\t\t<limit>1</limit>",
			"\t\t\t\t<rate>0</rate>",
			"\t\t\t\t<delay>0</delay>",
			"\t\t\t\t<maxDistance>5000</maxDistance>",
			"\t\t\t\t<minDistance>5000</minDistance>",
			"\t\t\t</pattern>",
			"\t\t</spawnable>",
			"\t\t<spawnable type=\"BulletCapsule\">",
			"\t\t\t<pattern>",
			"\t\t\t\t<limit>1</limit>",
			"\t\t\t\t<rate>0</rate>",
			"\t\t\t\t<delay>0</delay>",
			"\t\t\t\t<maxDistance>20</maxDistance>",
			"\t\t\t\t<minDistance>10</minDistance>",
			"\t\t\t</pattern>",
			"\t\t</spawnable>",
			"\t</level>",
			"</spawnerconfiguration>"
		};

		File.WriteAllLines(file, data);

		GameManager.Create(player, gameState, file);

		yield return new WaitForFixedUpdate();
		player.Ammo = 1;

		yield return new WaitUntil(() => gameState.RoundStarted);
		Spawner capsuleSpawner = null;
		do {
			var ss = Object.FindObjectsOfType<Spawner>();
			foreach (var s in ss) {
				if (s.ObjectToSpawn == typeof(BulletCapsule)) {
					capsuleSpawner = s;
					break;
				}
			}
			yield return new WaitForFixedUpdate();
		} while (capsuleSpawner is null);
		yield return new WaitUntil(() => capsuleSpawner.SpawnCount > 0);
		BulletCapsule capsule = Object.FindObjectOfType<BulletCapsule>();
		capsule.Speed = 0f;
		capsule.transform.Translate(new Vector3(10f, 10f, 10f));
		camera.transform.LookAt(capsule.transform);

		Assert.NotZero(Spawnable.Pool<Bullet>.Count);
		Assert.IsNotNull(player.Shoot());

		yield return new WaitWhile(() => capsule.isActiveAndEnabled);
		yield return new WaitForFixedUpdate();

		Assert.NotZero(player.Ammo, "Player should have bullets at the end.");
		Assert.False(gameState.GameOver,
				"The game must not end if restocked on bullets.");
	}

	[UnityTest]
	public IEnumerator UseLastShotToKillLastEnemy() {
		GameState gameState = GameState.Create(0);
		Camera camera = new GameObject("Camera").AddComponent<Camera>();
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: camera,
			ammo: 1,
			gameState: gameState);

		string[] data = new string[] {
			"<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
			"",
			"<spawnerconfiguration>",
			"\t<level>",
			"\t\t<spawnable type=\"Crasher\">",
			"\t\t\t<pattern>",
			"\t\t\t\t<limit>1</limit>",
			"\t\t\t\t<rate>0</rate>",
			"\t\t\t\t<delay>0</delay>",
			"\t\t\t\t<maxDistance>30</maxDistance>",
			"\t\t\t\t<minDistance>15</minDistance>",
			"\t\t\t</pattern>",
			"\t\t</spawnable>",
			"\t</level>",
			"</spawnerconfiguration>"
		};

		File.WriteAllLines(file, data);

		GameManager.Create(player, gameState, file);

		yield return new WaitForFixedUpdate();
		yield return new WaitUntil(() => Object.FindObjectOfType<Spawner>()
				.SpawnCount > 0);

		var enemy = Object.FindObjectOfType<Crasher>();
		camera.transform.LookAt(enemy.transform);
		player.Shoot();

		yield return new WaitForSeconds(2f);
		yield return new WaitForFixedUpdate();

		Assert.False(gameState.GameOver,
			"The game must not end if the last enemy dies by the last bullet.");
		Assert.True(gameState.RoundWon,
			"The round should be won when the last enemy dies by the last bullet.");
	}
}
