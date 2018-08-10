using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{

	public class Capsule : MonoBehaviour, ISpawnable
	{
		public CapsuleBase Base { get; private set; }

		private Vector3 rotation;
		private AudioSource pickUpSfx;	//TODO: move the sound effect of picking up bonuses to the player

		private Text bulletCountText;
		[SerializeField] private readonly GameManager gameManager;
		[SerializeField] private readonly Player player;

		public static Capsule Create(CapsuleBase.CapsuleType type, float speed)
		{
			var o = new GameObject("Capsule").AddComponent<Capsule>();
			o.Base = new CapsuleBase(type, speed);
			return o;
		}

		protected void Start()
		{
			rotation = new Vector3(15, 30, 45);
			pickUpSfx = GetComponent<AudioSource>();
			bulletCountText = FindObjectOfType<UIManager>().bulletCountText;
		}

		private void Update()
		{
			//rotation
			transform.Rotate(rotation * Time.deltaTime);
			//orbit
			transform.RotateAround(Vector3.zero, Vector3.up, Base.Speed * Time.deltaTime);
		}

		protected void OnDestroy()
		{
			if (gameManager != null)
			{
				if (!gameManager.gameOver)
				{
					switch (Base.Type)
					{
						case 0:
							player.Ammo += 4;
							bulletCountText.text = Bullet.Count.ToString();
							break;

						//TODO: Write cases for the rest of the types of capsule
					}
					pickUpSfx?.Play();
				}
				else if (gameManager.roundWon)
				{
					gameManager.AddScore(50);
				}
			}
		}
	}
}