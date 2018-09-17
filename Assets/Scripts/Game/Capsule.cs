using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{

	public class Capsule : MonoBehaviour, ISpawnable
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

		private Vector3 rotation;
		private AudioSource pickUpSfx;  //TODO: move the sound effect of picking up bonuses to the player

		private Text bulletCountText;
		[SerializeField] private readonly GameManager gameManager;
		[SerializeField] private readonly Player player;

		public static Capsule Create(CapsuleType type, float speed)
		{
			var o = new GameObject("Capsule").AddComponent<Capsule>();
			o.Type = type;
			o.Speed = speed;
			return o;
		}

		protected void Start()
		{
			rotation = new Vector3(15, 30, 45);
			pickUpSfx = GetComponent<AudioSource>();
			bulletCountText = FindObjectOfType<UIManager>()?.bulletCountText;
		}

		private void Update()
		{
			//rotation
			transform.Rotate(rotation * Time.deltaTime);
			//orbit
			transform.RotateAround(Vector3.zero, Vector3.up, Speed * Time.deltaTime);
		}

		protected void OnDestroy()
		{
			if (gameManager != null)
			{
				if (!gameManager.GameOver)
				{
					switch (this.Type)
					{
						case 0:
							player.Ammo += 4;
							if (bulletCountText != null)
								bulletCountText.text = Bullet.Count.ToString();
							break;

							//TODO: Write cases for the rest of the types of capsule
					}
					pickUpSfx?.Play();
				}
				else if (gameManager.RoundWon)
				{
					gameManager.AddScore(50);
				}
			}
		}
	}
}