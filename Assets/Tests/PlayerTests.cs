using NUnit.Framework;
using ShootAR;
using ShootAR.TestTools;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

internal class PlayerTest : TestBase
{
	[UnityTest]
	public IEnumerator PlayerCanShoot() {
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: new GameObject("Camera").AddComponent<Camera>(),
			ammo: 10);
		Spawnable.Pool<Bullet>.Instance.Populate(Bullet.Create(50f), 1);

		yield return null;
		Bullet shotBullet = player.Shoot();

		Assert.IsNotNull(shotBullet,
			"Player must be able to fire bullets.");
	}

	[UnityTest]
	public IEnumerator LoseHealthWhenHit() {
		Player player = Player.Create();
		var playerCollider = player.gameObject.AddComponent<CapsuleCollider>();
		playerCollider.height = 2f;
		playerCollider.isTrigger = true;

		var bullet = TestBullet.Create(1);
		yield return new WaitUntil(() => bullet.hit);

		Assert.AreEqual(Player.MAXIMUM_HEALTH - bullet.damage, player.Health,
			"Player must lose health when hit.");
	}

	[UnityTest]
	public IEnumerator GameOverWhenHealthDepleted() {
		GameState gameState = GameState.Create(0);
		Player player = Player.Create(1, null, 0, gameState);

		player.Health = 0;
		yield return null;

		Assert.True(gameState.GameOver,
			"Game should be in the \"Game Over\" state when player's" +
			"health is depleted.");
	}

	[UnityTest]
	public IEnumerator ArmorProtectsFromDamage() {
		Player player = Player.Create(Player.MAXIMUM_HEALTH);
		player.HasArmor = true;

		yield return null;
		player.GetDamaged(1);

		Assert.That(player.Health == Player.MAXIMUM_HEALTH,
			"When player gets damaged and has armor, health should not be reduced.");
	}

	[UnityTest]
	public IEnumerator ArmorLostWhenPlayerGetsDamaged() {

		Player player = Player.Create();
		player.HasArmor = true;

		yield return null;
		player.GetDamaged(1);

		Assert.IsFalse(player.HasArmor,
			"Armored player should lose its armor when damaged.");
	}

	[UnityTest]
	public IEnumerator ShootingUsesUpAmmo() {
		const int initialAmmoAmount = 10;
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: new GameObject().AddComponent<Camera>(),
			ammo: initialAmmoAmount);
		Spawnable.Pool<Bullet>.Instance.Populate(Bullet.Create(1), 1);

		yield return null;
		var bullet = player.Shoot();

		Assert.Less(player.Ammo, initialAmmoAmount,
			"After shooting, player should have one less bullet.");
	}

	[UnityTest]
	public IEnumerator CannotShootWithoutAmmo() {
		Player player = Player.Create(
			health: Player.MAXIMUM_HEALTH,
			camera: new GameObject("Camera").AddComponent<Camera>(),
			ammo: 0);
		Spawnable.Pool<Bullet>.Instance.Populate(Bullet.Create(0f));

		yield return null;
		var firedBullet = player.Shoot();

		Assert.IsNull(firedBullet,
			"Player shouldn't be able to shoot without ammo.");
	}

	[UnityTest]
	public IEnumerator PlayerCanNotBeHitInARapidSuccession() {
		Player player = Player.Create();
		var playerCollider = player.gameObject.AddComponent<CapsuleCollider>();
		playerCollider.height = 2f;
		playerCollider.isTrigger = true;

		var bullet = TestBullet.Create(1);
		yield return new WaitUntil(() => bullet.hit);
		var secondBullet = TestBullet.Create(1);
		yield return new WaitUntil(() => secondBullet.hit);

		Assert.AreEqual(Player.MAXIMUM_HEALTH - bullet.damage, player.Health,
			"Second hit must not damage player, since there is a cooldown.");
	}

	[UnityTest]
	public IEnumerator BulletsAreDestroyedAfterTravelingTooFar() {
		Bullet prefab = Bullet.Create(100f);
		Spawnable.Pool<Bullet>.Instance.Populate(prefab, 1);
		Object.Destroy(prefab.gameObject);

		Bullet shotBullet = Spawnable.Pool<Bullet>.Instance.RequestObject();
		shotBullet.gameObject.SetActive(true);
		yield return new WaitUntil(
						() => shotBullet.transform.position.magnitude
								>= Bullet.MAX_TRAVEL_DISTANCE);
		yield return new WaitForFixedUpdate();

		Assert.AreEqual(1, Spawnable.Pool<Bullet>.Instance.Count,
				"Bullet has not returned to the pool.");
	}
}
