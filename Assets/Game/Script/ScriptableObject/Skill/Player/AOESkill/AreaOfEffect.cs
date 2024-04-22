using Lean.Pool;
using System.Collections;
using UnityEngine;

public class AreaOfEffect : Skill
{
    public CapsuleCollider capsuleCollider;
    public float delayBeforeActivation = 1f;
    public ParticleSystem vfx;

    private void OnEnable()
    {
        capsuleCollider.enabled = false; // Disable collider initially

        // Enable collider after a delay
        StartCoroutine(ActivateColliderWithDelay());

        // Start coroutine to despawn after VFX duration
        StartCoroutine(DespawnAfterVFX());
    }

    private IEnumerator ActivateColliderWithDelay()
    {
        //if(vfx.main.duration > 2)
        //{
        //    yield return new WaitForSeconds(vfx.main.duration - 1f);
        //}
        //else
        //{
        //    yield return new WaitForSeconds(vfx.main.duration - 0.5f);
        //}

        yield return new WaitForSeconds(vfx.main.simulationSpeed - 0.25f);


        // Enable the collider after the delay
        capsuleCollider.enabled = true;
    }

    private IEnumerator DespawnAfterVFX()
    {
        // Wait for the duration of the VFX
        yield return new WaitForSeconds(vfx.main.duration);

        // Despawn the AreaOfEffect object
        LeanPool.Despawn(this);
    }

}


