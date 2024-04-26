using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Skill
{
    public BoxCollider boxCollider;
    public ParticleSystem vfx;

    private void OnEnable()
    {
        StartCoroutine(DespawnAfterVFX());
    }

    private IEnumerator DespawnAfterVFX()
    {
        // Wait for the duration of the VFX
        yield return new WaitForSeconds(vfx.main.duration);

        // Despawn the AreaOfEffect object
        LeanPool.Despawn(this);
    }



}
