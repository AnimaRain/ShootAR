using UnityEngine;
using ShootAR.Enemies;

namespace ShootAR
{
	/// <summary>
	/// Contains the original prefabs which the objects in the pools are cloned
	/// from.
	/// </summary>
	/// <remarks>
	/// Its values are assigned through the Inspector and are never changed
	/// during runtime.
	/// </remarks>
	public class PrefabContainer : MonoBehaviour
	{
		[SerializeField] private Capsule capsule;
		[SerializeField] private Crasher crasher;
		[SerializeField] private Drone drone;
		[SerializeField] private Bullet bullet;
		[SerializeField] private EnemyBullet enemyBullet;

		public Capsule Capsule { get => capsule; }
		public Crasher Crasher { get => crasher; }
		public Drone Drone { get => drone; }
		public Bullet Bullet { get => bullet; }
		public EnemyBullet EnemyBullet { get => enemyBullet; }

#if DEBUG
		public static PrefabContainer Create(
				Capsule c, Crasher cr, Drone d, Bullet b, EnemyBullet eb) {
			PrefabContainer o = new GameObject("Prefabs")
					.AddComponent<PrefabContainer>();
			o.capsule = c;
			o.crasher = cr;
			o.drone = d;
			o.bullet = b;
			o.enemyBullet = eb;

			return o;
		}
#endif
	}
}
