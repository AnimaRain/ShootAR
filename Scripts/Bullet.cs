using UnityEngine;

public class Bullet : SpawnableObject
{
    private static TVScript TVScreen;

    protected override void Awake()
    {
        base.Awake();
                		
        if (TVScreen == null)
            TVScreen = GameObject.FindGameObjectWithTag("TVScreen").GetComponent<TVScript>();
    }

    protected override void Start()
    {
        base.Start();

        transform.rotation = Camera.main.transform.rotation;
        transform.position = Camera.main.transform.position;
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;

        gameController.Bullets--;
        gameController.CountText.text = gameController.Bullets.ToString();
    }

    private void OnCollisionEnter(Collision col)
    {
        GameObject other = col.gameObject;
        if (other.CompareTag("Enemy") || other.CompareTag("Capsule"))
        {
            Destroy(other);
            Destroy(gameObject);
        }
        else if (other.tag == "Remote")
        {
            if (TVScreen.tvon)
            {
                TVScreen.CloseTV();
            }
            else
            {
                TVScreen.StartTV();
            }
        }
    }
}