using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Skill
{
    public BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        Invoke("OnDespawn", 0.1f);
    }

    void OnDespawn()
    {
        LeanPool.Despawn(this);
    }


}
