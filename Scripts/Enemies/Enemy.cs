using UnityEngine;

/// <summary>
/// Parent class of all types of enemies.
/// </summary>
public partial class Enemy : SpawnableObject
{

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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (!gameController.GameOver) gameController.AddScore(PointsValue);
    }


    protected virtual void Attack()
    {
        AttackSFX.Play();
        gameController.DamagePlayer(Damage);
        if (Random.Range(0, SpecialAttackChance) == SpecialAttackChance) SpecialAttack();
    }

    protected virtual void SpecialAttack() { }
}