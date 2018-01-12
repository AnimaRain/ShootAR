using UnityEngine;

public class Capsule : SpawnableObject
{
    private Vector3 rotation;

    protected override void Start()
    {
        rotation = new Vector3(15, 30, 45);
    }

    private void Update()
    {
        //rotation
        transform.Rotate(rotation * Time.deltaTime);
        //orbit
        transform.RotateAround(Camera.main.transform.position, Vector3.up, 20 * Time.deltaTime);

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (gameController != null)
        {
            /* If a capsule is destroyed the player gains bullets.
             * If it is is not destroyed at the end of the round, the game-
             * controller destroys it and the player gains 50 points.*/
            if (!gameController.GameOver)
            {
                gameController.Bullets += 4;
                gameController.CountText.text = gameController.Bullets.ToString();
            }
            else if(gameController.RoundWon)
            {
                gameController.AddScore(50);
            }
        }
    }
}