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
		BulletCapsule capsule =
			BulletCapsule.Create(0, player);

		yield return new WaitForSecondsRealtime(5f);
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
		var capsule = BulletCapsule.Create(0, Player.Create());
		var bullet = Bullet.Create(0);

		capsule.gameObject.SetActive(true);
		bullet.gameObject.SetActive(true);

		yield return new WaitUntil(
			() => Spawnable.Pool<BulletCapsule>.Instance.Count > 0
		);

		Assert.AreSame(capsule, Spawnable.Pool<BulletCapsule>.Instance.RequestObject());
	}
}
