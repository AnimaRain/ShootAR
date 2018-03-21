using UnityEngine;

/// <summary>
/// Parent class of all types of enemies.
/// </summary>
public partial class Enemy : SpawnableObject
{

	/// <summary>
	/// Total count of spawned enemies during the current round.
	/// </summary>
	public static int Count;
	/// <summary>
	/// Count of currently active enemies.
	/// </summary>
	public static int ActiveCount;
	/// <summary>
	/// The amount of points added to the player's score when destroyed.
	/// </summary>
	public int PointsValue;
	/// <summary>
	/// The amount of damage the player recieves from this object's attack.
	/// </summary>
	[Range(-6, 6)]
	public int Damage;
	[SerializeField] protected AudioClip AttackSfx;
	[SerializeField] protected GameObject Explosion;

	protected AudioSource sfx;


	protected override void Awake()
	{
		base.Awake();

		//Create an audio source to play the audio clips
		sfx = gameObject.AddComponent<AudioSource>();
		sfx.clip = AttackSfx;
		sfx.volume = 0.3f;
		sfx.playOnAwake = false;
		sfx.maxDistance = 10f;
	}

	protected virtual void Start()
	{
		ActiveCount++;
		Count++;
	}

	protected virtual void FixedUpdate()
	{
		OrbitAround(Vector3.zero);
	}

	protected virtual void OnDestroy()
	{
		if (!gameController.gameOver)
		{
			gameController.AddScore(PointsValue);

			//Explosion special effects
			Instantiate(Explosion, transform.position, transform.rotation);
		}
		ActiveCount--;
	}

}