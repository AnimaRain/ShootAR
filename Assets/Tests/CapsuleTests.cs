using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using ShootAR;

public class CapsuleTests {

    [UnityTest]
    public IEnumerator CapsuleGivesBulletsWhenDestroyed() {
		//Set up Test
		Player player = Player.Create();
		Capsule capsule =
			Capsule.Create(Capsule.CapsuleType.Bullet, 0, player);

		//Perform Test
		Object.Destroy(capsule.gameObject);
		yield return new WaitUntil(() => capsule == null);

		//Assert
		Assert.Greater(player.Ammo, 0);
    }

	//UNDONE:
	/* Tests for conditional-capsule
	 * e.g. Capsules that appears after performing a combo, killing a couple of
	 * enemies in row without missing.
	 * 
	 * Maybe this belongs elsewhere. Maybe in spawners...
	 * ...or in an other game-logic system?
	 * 
	[UnityTest]
	public IEnumerator BonusCapsuleAppearsAfterKillingXEnemiesConsecutively()
	{
		yield return null;
	}
	*/

	[TearDown]
	public void ClearEnvironment()
	{
		var objects = Object.FindObjectsOfType<GameObject>();
		foreach (var o in objects) Object.Destroy(o.gameObject);
	}
}
