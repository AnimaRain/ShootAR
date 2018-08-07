using System;

namespace ShootAR
{
	[Serializable]
	public class Capsule
	{
		private GameManager gameManager;

		public enum CapsuleType
		{
			Bullet,
			Health,
			Armor,
			PowerUp
		}
		public CapsuleType Type { get; private set; }

	}
}
