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
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: new GameObject().AddComponent<Camera>(),
			ammo: 1,
			gameState: gameState);
		Capsule capsule = Capsule.Create(
			type: Capsule.CapsuleType.Bullet,
			speed: 0,
			player: player);
		Spawnable.Pool<Bullet>.Populate(Bullet.Create(10));

		// Create an enemy to stop game manager from switching state to "round won".
		var enemy = TestTarget.Create();
		enemy.transform.Translate(Vector3.right * 500f);

		GameManager.Create(player, gameState);

		yield return null;  // without this, player.Shoot() will return null.

		capsule.transform.Translate(new Vector3(10f, 10f, 10f));
		player.transform.LookAt(capsule.transform);
		player.Shoot()
			.gameObject.SetActive(true);


		yield return new WaitWhile(() => player.Ammo > 0);

		Assert.False(gameState.GameOver,
				"The game must not end, if restocked on bullets.");
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
		GameManager.Create("Assets\\Tests\\GameStateTests-testpattern0.xml",
			player, gameState,
			PrefabContainer.Create(
				cr: TestEnemy.Create(0, 0, 0, 10, 10, 10),
				b: Bullet.Create(100f),
				bc: null, ac: null, hc: null, pc: null, d: null, eb: null
		));

		yield return new WaitForFixedUpdate();
		yield return new WaitUntil(() => Object.FindObjectOfType<Spawner>()
				.SpawnCount > 0);

		TestEnemy enemy = Object
				.FindObjectOfType<ShootAR.Enemies.Crasher>() as TestEnemy;
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
