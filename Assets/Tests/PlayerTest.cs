using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using ShootAR;

class PlayerTest
{
	internal class FakeBullet : MonoBehaviour
	{
		int damage;
		public bool hit;

		public static FakeBullet Create(int damage)
		{
			var o = new GameObject("Bullet").AddComponent<FakeBullet>();
			o.damage = damage;
			return o;
		}

		private void Start()
		{
			var collider = gameObject.AddComponent<SphereCollider>();
			collider.isTrigger = true;

			gameObject.AddComponent<Rigidbody>();
		}

		private void OnTriggerEnter(Collider other)
		{
			Debug.Log("Bullet hit!");
			other.GetComponent<Player>().Health -= damage;
			hit = true;
		}
	}

	[UnityTest]
	public IEnumerator PlayerShoots()
	{
		yield return null;
	}

	[UnityTest]
	public IEnumerator LoseHealthWhenHit()
	{
		Player player = Player.Create(3);
		var playerCollider = player.gameObject.AddComponent<CapsuleCollider>();
		playerCollider.height = 2f;
		playerCollider.isTrigger = true;

		var bullet = FakeBullet.Create(1);

		bullet.transform.position = player.transform.position;
		yield return new WaitUntil(() => bullet.hit);

		Assert.AreEqual(2, player.Health, "Player must lose health when hit.");
	}

	[UnityTest]
	public IEnumerator GameOverWhenHealthDepleted()
	{
		yield return null;
	}

	[UnityTest]
	public IEnumerator ArmorProtectsFromHits()
	{
		yield return null;
	}

	[UnityTest]
	public IEnumerator UseLastShotToHitCapsuleAndTakeBullets()
	{
		yield return null;
	}

	[UnityTest]
	public IEnumerator UseLastShotToKillLastEnemy()
	{
		yield return null;
	}

	[TearDown]
	public void ClearTestEnvironment()
	{
		GameObject[] objects = Object.FindObjectsOfType<GameObject>();

		foreach (GameObject o in objects)
		{
			Object.Destroy(o);
		}
	}
}