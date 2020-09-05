using UnityEngine;
using UnityEngine.TestTools;
using ShootAR;
using ShootAR.TestTools;
using NUnit.Framework;
using System.Collections;
using ShootAR.Enemies;

public class EnemyAttackTests : TestBase {
	[UnityTest]
	public IEnumerator CrasherAttacksPlayer() {
		Player player = Player.Create(health: 3);
		Crasher crasher = Object.Instantiate<Crasher>(
			Resources.Load<Crasher>("Prefabs/Enemies/Crasher"),
			new Vector3(10f, 10f, 5f), new Quaternion()
		);

		crasher.Attack();
		yield return new WaitUntil(() => player.Health != 3);

		Assert.Less(player.Health, 3);
	}

	[UnityTest]
	public IEnumerator DroneAttacksPlayer() {
		Player player = Player.Create(health: 3);
		Drone drone = Object.Instantiate<Drone>(
			Resources.Load<Drone>("Prefabs/Enemies/Drone"),
			new Vector3(10f, 10f, 5f), new Quaternion()
		);

		Spawnable.Pool<EnemyBullet>.Instance.Populate(
			Resources.Load<EnemyBullet>("Prefabs/Enemies/EnemyBullet"),
			lot: 2
		);

		drone.Attack();
		yield return new WaitUntil(() => player.Health != 3);

		Assert.Less(player.Health, 3);
	}
}
