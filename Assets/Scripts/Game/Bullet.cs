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
    private static AudioSource ShotSfx;

    protected override void Awake()
    {
		if (Count < 0) Destroy(this);

		base.Awake();

		if (TVScreen == null)
			TVScreen = GameObject.FindGameObjectWithTag("TVScreen").GetComponent<TVScript>();
		if (ShotSfx == null)
			ShotSfx = GetComponent<AudioSource>();
	}

    protected void Start()
    {
        transform.rotation = Camera.main.transform.rotation;
		transform.position = Vector3.zero;
		ShotSfx.Play();
		GetComponent<Rigidbody>().velocity = transform.forward * Speed;

		Count--;
		ActiveCount++;
		gameController.CountText.text = Count.ToString();
	}

	/**TODO:
	 * Check 'OnCollisionEnter' and 'OnTriggerEnter' and choose one
	 * or probably a merged version of both of them. */
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