using System;

namespace ShootAR
{
	[Serializable]
	public class Capsule
	{
		public float Speed { get; set; }

		public enum CapsuleType
		{
			Bullet,
			Health,
			Armor,
			PowerUp
		}
		public CapsuleType Type { get; private set; }

		public Capsule(CapsuleType type, float speed)
		{
			Type = type;
			Speed = speed;
		}
	}
}
