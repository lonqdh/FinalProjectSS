using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : Skill
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

}


