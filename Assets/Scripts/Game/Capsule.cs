using UnityEngine;

public class Capsule : SpawnableObject
{
    private Vector3 rotation;

    protected void Start()
    {
        rotation = new Vector3(15, 30, 45);
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
        if (gameController != null)
        {
            /* If a capsule is destroyed the player gains bullets.
             * If it is is not destroyed at the end of the round, the game-
             * controller destroys it and the player gains 50 points.*/
            if (!gameController.gameOver)
            {
                Bullet.Count += 4;
                gameController.CountText.text = Bullet.Count.ToString();
            }
            else if(gameController.roundWon)
            {
                gameController.AddScore(50);
            }
        }
    }
}