using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        Invoke("OnDespawn", 3.0f);
    }

    void OnDespawn()
    {
        
        LeanPool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the enemy
        //Enemy enemy = other.GetComponent<Enemy>();
        //if (enemy != null)
        //{
        //    // Decrease the enemy's health
        //    enemy.TakeDamage(damageAmount);
        //}

    }

}
