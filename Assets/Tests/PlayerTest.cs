using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using ShootAR;
using ShootAR.TestTools;

class PlayerTest
{
	[UnityTest]
	public IEnumerator PlayerCanShoot()
	{
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: new GameObject("Camera").AddComponent<Camera>(),
			bullet: Bullet.Create(50f),
			ammo: 10);

		yield return null;
		Bullet shotBullet = player.Shoot();
		shotBullet?.gameObject.SetActive(true);

		Assert.IsNotNull(shotBullet,
			"Player must be able to fire bullets.");
	}

	[UnityTest]
	public IEnumerator LoseHealthWhenHit()
	{
		Player player = Player.Create();
		var playerCollider = player.gameObject.AddComponent<CapsuleCollider>();
		playerCollider.height = 2f;
		playerCollider.isTrigger = true;

		var bullet = TestBullet.Create(1);

		bullet.transform.position = player.transform.position;
		yield return new WaitUntil(() => bullet.hit);

		Assert.AreEqual(Player.MAXIMUM_HEALTH - bullet.damage, player.Health, "Player must lose health when hit.");
	}

	[UnityTest]
	public IEnumerator GameOverWhenHealthDepleted()
	{
		GameState gameState = GameState.Create(0);
		Player player = Player.Create(1, null, null, 0, gameState);

		player.Health = 0;
		yield return null;

		Assert.True(gameState.GameOver,
			"Game should be in the \"Game Over\" state when player's" +
			"health is depleted.");
	}

	[UnityTest]
	public IEnumerator ArmorProtectsFromDamage()
	{
		Player player = Player.Create(Player.MAXIMUM_HEALTH);
		player.HasArmor = true;

		yield return null;
		player.GetDamaged(1);

		Assert.That(player.Health == Player.MAXIMUM_HEALTH,
			"When player gets damaged and has armor, health should not be reduced.");
	}

	[UnityTest]
	public IEnumerator ArmorLostWhenPlayerGetsDamaged()
	{

		Player player = Player.Create();
		player.HasArmor = true;

		yield return null;
		player.GetDamaged(1);

		Assert.IsFalse(player.HasArmor,
			"Armored player should lose its armor when damaged.");
	}

	[UnityTest]
	public IEnumerator ShootingUsesUpAmmo()
	{
		const int initialAmmoAmount = 10;
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: new GameObject().AddComponent<Camera>(),
			bullet: Bullet.Create(1),
			ammo: initialAmmoAmount);

		yield return null;
		var bullet = player.Shoot();
		bullet.gameObject.SetActive(true);

		Assert.Less(player.Ammo, initialAmmoAmount,
			"After shooting, player should have one less bullet.");
	}

	[UnityTest]
	public IEnumerator CannotShootWithoutAmmo()
	{
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: new GameObject("Camera").AddComponent<Camera>(),
			bullet: Bullet.Create(0),
			ammo: 0);

		yield return null;
		var firedBullet = player.Shoot();

		Assert.IsNull(firedBullet,
			"Player shouldn't be able to shoot without ammo.");
	}

	[TearDown]
	public void ClearTestEnvironment()
	{
		GameObject[] objects = Object.FindObjectsOfType<GameObject>();

		foreach (GameObject o in objects)
		{
			Object.Destroy(o.gameObject);
		}
	}
}