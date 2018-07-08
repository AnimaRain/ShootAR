using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{

	public class Capsule : SpawnableObject
	{
		private Vector3 rotation;
		private AudioSource pickUpSfx;

		private GameManager gameManager;
		private Text bulletCountText;

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
			transform.RotateAround(Vector3.zero, Vector3.up, speed * Time.deltaTime);
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
					Bullet.count += 4;
					bulletCountText.text = Bullet.count.ToString();
					pickUpSfx.Play();
				}
				else if (gameManager.roundWon)
				{
					gameManager.AddScore(50);
				}
			}
		}
	}
}