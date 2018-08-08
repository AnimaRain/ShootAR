using System;

namespace ShootAR
{
	[Serializable]
	public class CapsuleBase
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

		public CapsuleBase(CapsuleType type, float speed)
		{
			Type = type;
			Speed = speed;
		}
	}
}
