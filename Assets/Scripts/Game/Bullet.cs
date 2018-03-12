using UnityEngine;

public class Bullet : SpawnableObject
{
	/// <summary>
	/// Total count of spawned enemies during the current round.
	/// </summary>
	public static int Count;
	/// <summary>
	/// Count of currently active enemies.
	/// </summary>
	public static int ActiveCount;
	private static TVScript TVScreen;
    private GameSounds gameSounds;

    protected override void Awake()
    {
        base.Awake();

		if (TVScreen == null)
			TVScreen = GameObject.FindGameObjectWithTag("TVScreen").GetComponent<TVScript>();
		if (gameSounds == null)
			gameSounds = GameObject.Find("GameController").GetComponent<GameSounds>();
		if (Count < 0) Destroy(this);
	}

    protected void Start()
    {
        transform.rotation = Camera.main.transform.rotation;
		transform.position = Vector3.zero;
		GetComponent<Rigidbody>().velocity = transform.forward * Speed;

		Count--;
		ActiveCount++;
		gameController.CountText.text = Count.ToString();
	}

	/**<TODO>
	 * Check 'OnCollisionEnter' and 'OnTriggerEnter' and choose one
	 * or probably a merged version of both of them.
	 * </TODO> */
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

	private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Capsule")) gameSounds.BulletPickupSound();
        if (col.CompareTag("Enemy"))
        {
            GameObject Explosion = Instantiate(Resources.Load("Effect_02", typeof(GameObject))) as GameObject;
            Explosion.transform.position = transform.position;
            gameSounds.EnemyDestroyFunction();
            Destroy(Explosion, 2);
        }
        if (col.CompareTag("Enemy") || col.CompareTag("Capsule"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "Remote")
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

	protected void OnDestroy()
	{
		ActiveCount--;
	}
}