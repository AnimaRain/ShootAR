using ShootAR.Enemies;
using UnityEngine;
using NUnit.Framework;
using System.IO;

namespace ShootAR.TestTools
{
	/// <summary>
	/// Test class for replacing enemy classes in tests.
	/// </summary>
	[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
	internal class TestEnemy : Crasher
	{
		public bool GotHit { get; private set; }

		protected override void OnTriggerEnter(Collider other) {
			base.OnTriggerEnter(other);
			if (other.GetComponent<Bullet>() != null)
				GotHit = true;
		}

		public static TestEnemy Create(
				float speed = default(float),
				int damage = default(int),
				int pointsValue = default(int),
				float x = 0, float y = 0, float z = 0) {
			var o = new GameObject(nameof(TestEnemy)).AddComponent<TestEnemy>();
			o.Speed = speed;
			o.Damage = damage;
			o.PointsValue = pointsValue;
			o.transform.position = new Vector3(x, y, z);
			o.gameObject.SetActive(false);
			return o;
		}

		protected new void Start() {
			GetComponent<SphereCollider>().isTrigger = true;
			GetComponent<Rigidbody>().useGravity = false;
		}

		public new void ResetState() { }

		public new void Destroy() {
			ReturnToPool<TestEnemy>();
			ActiveCount--;
		}
	}

	//FIXME: Inherit from EnemyBullet.
	/// <summary>
	/// Bullet to test hitting player
	/// </summary>
	internal class TestBullet : MonoBehaviour
	{
		public int damage;
		public bool hit;

		public static TestBullet Create(int damage) {
			var o = new GameObject("Bullet").AddComponent<TestBullet>();
			o.damage = damage;
			return o;
		}

		private void Start() {
			var collider = gameObject.AddComponent<SphereCollider>();
			collider.isTrigger = true;

			var body = gameObject.AddComponent<Rigidbody>();
			body.useGravity = false;
		}

		private void OnTriggerEnter(Collider other) {
			var player = other.GetComponent<Player>();
			if (player == null)
				return;

			Debug.Log("Bullet hit!");
			player.GetDamaged(damage);
			hit = true;
		}
	}

	/// <summary>
	/// Bare bones <see cref="Spawnable"/> object for test purposes.
	/// </summary>
	internal class TestObject : Spawnable
	{
		public static TestObject Create(
			float speed = default,
			float x = 0, float y = 0, float z = 0,
			GameState gameState = null) {
			var o = new GameObject(nameof(TestObject)).AddComponent<TestObject>();
			o.Speed = speed;
			o.transform.position = new Vector3(x, y, z);
			return o;
		}

		public override void Destroy() {
			throw new System.NotImplementedException();
		}

		public override void ResetState() {
			throw new System.NotImplementedException();
		}
	}

	public class TestBase
	{
		[TearDown]
		public void ClearTestEnvironment() {
			GameObject[] objects = Object.FindObjectsOfType<GameObject>();

			foreach (GameObject o in objects) {
				Object.Destroy(o.gameObject);
			}

			Spawnable.Pool<BulletCapsule>.Empty();
			Spawnable.Pool<ArmorCapsule>.Empty();
			Spawnable.Pool<HealthCapsule>.Empty();
			Spawnable.Pool<PowerUpCapsule>.Empty();
			Spawnable.Pool<Bullet>.Empty();
			Spawnable.Pool<EnemyBullet>.Empty();
			Spawnable.Pool<Crasher>.Empty();
			Spawnable.Pool<Drone>.Empty();
			Spawnable.Pool<TestEnemy>.Empty();
		}
	}

	public class PatternsTestBase : TestBase
	{
		protected const string PATTERN_FILE = "patternstestfile.xml";
		///<summary>pattern file's full path</summary>
		protected string file;

		[SetUp]
		public void FileSetUp() {
			file = Path.Combine(Application.persistentDataPath, PATTERN_FILE);
		}

		[TearDown]
		public void DeletePatternFile() {
			if (File.Exists(file))
				File.Delete(file);

			Assert.That(
				!File.Exists(file),
				"The file should be deleted when the test ends."
			);
		}
	}
}
