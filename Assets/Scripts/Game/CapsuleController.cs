using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{

	public class CapsuleController : MonoBehaviour, ISpawnable
	{
		public Capsule Base { get; private set; }

		private Vector3 rotation;
		private AudioSource pickUpSfx;	//TODO: move the sound effect of picking up bonuses to the player

		private Text bulletCountText;
		[SerializeField] private GameManager gameManager;

		public static CapsuleController Create(Capsule.CapsuleType type, float speed)
		{
			var o = new GameObject("Capsule").AddComponent<CapsuleController>();
			o.Base = new Capsule(type, speed);
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
				/* If a capsule is destroyed, the player gains bullets.
				 * If it is is not destroyed at the end of the round, the GameController
				 * destroys it and the player gains 50 points.*/
				if (!gameManager.gameOver)
				{
					switch (Base.Type)
					{
						case 0:
							Bullet.Count += 4;
							bulletCountText.text = Bullet.Count.ToString();
							break;
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