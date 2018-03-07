using UnityEngine;

/// <summary>
/// Parent class of spawnable objects.
/// </summary>
public class SpawnableObject : MonoBehaviour
{
    /// <summary>
    /// The speed at which this object is moving.
    /// </summary>
    public float Speed;

    protected GameController gameController;

    protected virtual void Awake()
    {
        if (gameController == null)
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

}
