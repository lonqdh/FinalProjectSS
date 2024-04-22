using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Skill
{
    public BoxCollider boxCollider;

    private void Start()
    {
        //boxCollider = GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        boxCollider.enabled = true;
        Invoke("OnDespawn", 1f);
    }

    void OnDespawn()
    {
        boxCollider.enabled = false;
        LeanPool.Despawn(this);
    }


}
