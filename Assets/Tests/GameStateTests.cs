using NUnit.Framework;
using ShootAR;
using ShootAR.TestTools;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class GameStateTests : TestBase
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
		var spawner = Spawner.Create();
		var bulletCapsule = BulletCapsule.Create(0, player);
		var bullet = Bullet.Create(10);
		var enemy = TestEnemy.Create(); // Create an enemy to stop game manager
										  // from switching state to "round won".

		GameManager.Create(player, gameState,
				"Assets/Tests/GameStateTests-testpattern.xml");

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
		GameManager.Create(
			player, gameState,
			"Assets/Tests/GameStateTests-testpattern0.xml"
		);
			var enemy = TestEnemy.Create(0, 0, 0, 10, 10, 10);
			var bullet = Bullet.Create(100f);
			var spawner = Spawner.Create();

		yield return new WaitForFixedUpdate();
		yield return new WaitUntil(() => Object.FindObjectOfType<Spawner>()
				.SpawnCount > 0);

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
