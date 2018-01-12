using UnityEngine;

/// <summary>
/// Parent class of spawnable objects.
/// </summary>
public class SpawnableObject : MonoBehaviour
{
    /// <summary>
    /// The speed this object is moving.
    /// </summary>
    public float Speed;

    protected GameController gameController;

    protected virtual void Awake()
    {
        if (gameController == null)
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    protected virtual void Start()
    {
        /* If there is no counter in the game controller for the type
         * of object that is currently spawned, it is added.*/
        if (!gameController.ActiveCounter.ContainsKey(GetType()))
            gameController.ActiveCounter.Add(GetType(), 0);

        gameController.ActiveCounter[GetType()]++;
    }

    protected virtual void OnDestroy()
    {
        if (gameController.ActiveCounter.ContainsKey(GetType()))
            gameController.ActiveCounter[GetType()]--;
    }
}
