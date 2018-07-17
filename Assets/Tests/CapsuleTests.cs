using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using ShootAR;

public class CapsuleTests {

    /*[UnityTest]
    public IEnumerator CapsuleGivesBulletsWhenDestroyed() {
		//TODO: Set up Test
		Capsule capsule = Capsule.Create(Capsule.CapsuleType.Bullet);
		Player stubPlayer = new GameObject("Player").AddComponent<Player>();
		stubPlayer.Bullets = 10;

		//TODO: Perform Test
		Object.Destroy(capsule.gameObject);

		//TODO: Assert
		Assert.AreEqual(10, stubPlayer.Bullets);
		yield return null;
    }*/

	[UnityTest]
	public IEnumerator UndestroyedCapsuleGivesPointsAtRoundEnd()
	{
		//TODO: Set up Test
		//Create capsule

		//TODO: Perform Test

		//TODO: Assert

		yield return null;
	}

	//TODO:
	/* Tests for conditional-capsule
	 * e.g. Capsules that appears after performing a combo, killing a couple of
	 * enemies in row without missing.
	 * 
	 * Maybe this belongs elsewhere. Maybe in spawners...
	 * ...or in an other game-logic system?
	 * 
	[UnityTest]
	public IEnumerator SpecialCapsuleAppearsAfterKillingXEnemiesInARow()
	{
		yield return null;
	}
	*/
}
