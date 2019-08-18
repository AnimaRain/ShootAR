using NUnit.Framework;
using ShootAR;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class CapsuleTests : ShootAR.TestTools.TestBase
{

	[UnityTest]
	public IEnumerator CapsuleGivesBullets() {
		//Set up Test
		Player player = Player.Create();
		Capsule capsule =
			Capsule.Create(Capsule.CapsuleType.Bullet, 0, player);

		yield return null;
		//Perform Test
		capsule.Destroy();

		//Assert
		Assert.Greater(player.Ammo, 0);
	}

	/*UNDONE: Tests for conditional-capsule
	 * e.g. Capsules that appears after performing a combo, killing a couple of
	 * enemies in row without missing.
	 * 
	 * Maybe this belongs elsewhere. Maybe in spawners...
	 * ...or in an other game-logic system?
	 */
	[UnityTest]
	[Ignore("Not yet implemented")]
	public IEnumerator BonusCapsuleAppearsAfterKillingXEnemiesConsecutively() {
		yield return null;
	}

	[UnityTest]
	public IEnumerator CapsuleDestroyedByBullet() {
		var capsule = Capsule.Create(0, 0);
		var bullet = Bullet.Create(0);

		bullet.gameObject.SetActive(true);

		yield return new WaitUntil(
			() => Spawnable.Pool<Capsule>.Count > 0
		);

		Assert.AreSame(capsule, Spawnable.Pool<Capsule>.RequestObject());
	}
}
