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
				ulong pointsValue = default(int),
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

		public new void ResetState() {
			StopMoving();
			CanMove = true;
		}

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

	internal class OrbitTester : TestEnemy
	{
		public Orbit Orbit { get; set; }

		public static OrbitTester Create(
				Orbit orbit,
				float speed = default,
				int damage = default,
				ulong pointsValue = default) {
			var o = new GameObject(nameof(OrbitTester)).AddComponent<OrbitTester>();

			o.Orbit = orbit;
			o.Speed = speed;
			o.Damage = damage;
			o.PointsValue = pointsValue;
			o.transform.position = orbit.direction + orbit.centerPoint;
			o.gameObject.SetActive(false);
			return o;
		}

		private void Update() {
			OrbitAround(Orbit);
			Debug.DrawRay(Orbit.centerPoint, Orbit.perpendicularAxis, Color.blue);
			Debug.DrawLine(Orbit.direction, Orbit.centerPoint);
			Debug.DrawLine(transform.position, Orbit.direction);
			Debug.DrawLine(transform.position, Orbit.perpendicularAxis, Color.magenta);
		}

		public new void Destroy() {
			ReturnToPool<OrbitTester>();
			ActiveCount--;
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

			Spawnable.Pool<BulletCapsule>.Instance.Empty();
			Spawnable.Pool<ArmorCapsule>.Instance.Empty();
			Spawnable.Pool<HealthCapsule>.Instance.Empty();
			Spawnable.Pool<PowerUpCapsule>.Instance.Empty();
			Spawnable.Pool<Bullet>.Instance.Empty();
			Spawnable.Pool<EnemyBullet>.Instance.Empty();
			Spawnable.Pool<Crasher>.Instance.Empty();
			Spawnable.Pool<Drone>.Instance.Empty();
			Spawnable.Pool<TestEnemy>.Instance.Empty();
		}
	}

	public class PatternsTestBase : TestBase
	{
		protected const string PATTERN_FILE = "patternstestfile.xml";

		///<summary>pattern file's full path</summary>
		protected string file;

		protected FileInfo patternNames;
		protected FileInfo patternNamesBackup;

		[SetUp]
		public void FileSetUp() {
			file = Path.Combine(
				Application.persistentDataPath,
				Configuration.PATTERNS_DIR,
				PATTERN_FILE
			);

			/* The file's name need to be included in a spesific file in order
			 * to be able to be read later. */
			patternNames = new FileInfo(Path.Combine(
				Application.persistentDataPath, Configuration.PATTERN_NAMES));

			// Back up that special file, if it already exists, to be restored later.
			if (patternNames.Exists) {
				patternNamesBackup = patternNames.CopyTo($"{patternNames.FullName}-{Random.value}~");

				patternNames.Delete();
			}

			// Write file's name into the special file.
			using (BinaryWriter writer = new BinaryWriter(patternNames.OpenWrite())) {
				writer.Write(1); // one string in file
				writer.Write(PATTERN_FILE.TrimEnd(".xml".ToCharArray()));
			}
		}

		[TearDown]
		public void DeletePatternFile() {
			if (patternNamesBackup != null && patternNamesBackup.Exists) {
				patternNamesBackup.CopyTo(patternNames.FullName, true);
				patternNamesBackup.Delete();
			}
			else patternNames.Delete();

			if (File.Exists(file))
				File.Delete(file);

			Assert.That(
				!File.Exists(file),
				"The file should be deleted when the test ends."
			);
		}
	}
}
