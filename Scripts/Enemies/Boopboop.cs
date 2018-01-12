﻿using UnityEngine;

/// <summary>
/// Short-Ranged class of Enemy
/// </summary>
public class Boopboop : Enemy
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Attack();
    }


    /// <summary>
    /// Enemy rotates and moves towards player.
    /// </summary>
    protected void MoveToPlayer()
    {
        transform.LookAt(new Vector3(0, 0, 0));
        transform.forward = transform.position;
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }
}