using Lean.Pool;
using System.Collections;
using UnityEngine;

public class AreaOfEffect : Skill
{
    public CapsuleCollider capsuleCollider;
    public float delayBeforeActivation = 2.0f;

    private void OnEnable()
    {
        capsuleCollider.enabled = false; // Disable collider initially
        StartCoroutine(ActivateColliderWithDelay());
        Invoke("OnDespawn", 3.0f);

    }

    private void Start()
    {

    }

    private IEnumerator ActivateColliderWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeActivation);

        // Enable the collider after the delay
        capsuleCollider.enabled = true;
    }


    void OnDespawn()
    {

        LeanPool.Despawn(this);
    }

}


