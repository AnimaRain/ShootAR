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
		[SerializeField] private BulletCapsule bulletCapsule;
		[SerializeField] private HealthCapsule healthCapsule;
		[SerializeField] private ArmorCapsule armorCapsule;
		[SerializeField] private PowerUpCapsule powerUpCapsule;
		[SerializeField] private Crasher crasher;
		[SerializeField] private Drone drone;
		[SerializeField] private Bullet bullet;
		[SerializeField] private EnemyBullet enemyBullet;

		public Crasher Crasher { get => crasher; }
		public Drone Drone { get => drone; }
		public Bullet Bullet { get => bullet; }
		public EnemyBullet EnemyBullet { get => enemyBullet; }
		public BulletCapsule BulletCapsule { get => bulletCapsule; }
		public HealthCapsule HealthCapsule { get => healthCapsule; }
		public ArmorCapsule ArmorCapsule { get => armorCapsule; }
		public PowerUpCapsule PowerUpCapsule { get => powerUpCapsule; }

#if DEBUG
		public static PrefabContainer Create(
				BulletCapsule bc, ArmorCapsule ac, HealthCapsule hc,
				PowerUpCapsule pc, Crasher cr, Drone d, Bullet b,
				EnemyBullet eb) {
			PrefabContainer o = new GameObject("Prefabs")
					.AddComponent<PrefabContainer>();
			o.bulletCapsule = bc;
			o.armorCapsule = ac;
			o.healthCapsule = hc;
			o.powerUpCapsule = pc;
			o.crasher = cr;
			o.drone = d;
			o.bullet = b;
			o.enemyBullet = eb;

			return o;
		}
#endif
	}
}
