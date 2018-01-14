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
    [Range(-6,6)]
    public int Damage;
    public AudioSource AttackSFX;

    /// <summary>
    /// Multiplier of the divider of the chance fraction.
    /// 1/(2*n), n=0 is 100%
    /// </summary>
    [Tooltip("1/(2*n), n=0 is 100%")]
    private int SpecialAttackChance;

	protected virtual void Start()
	{
		ActiveCount++;
		Count++;
	}

	protected virtual void OnDestroy()
    {
        if (!gameController.gameOver) gameController.AddScore(PointsValue);
		ActiveCount--;
	}

	protected virtual void MoveTo(Vector3 point)
	{
		/*TODO:
		 * Write code here.
		 * Use Bezier Curves (maybe) to move around the player. */
	}

	protected virtual void Attack()
    {
        AttackSFX.Play();
        gameController.DamagePlayer(Damage);
        if (Random.Range(0, SpecialAttackChance) == SpecialAttackChance) SpecialAttack();
    }

    protected virtual void SpecialAttack() { }
}