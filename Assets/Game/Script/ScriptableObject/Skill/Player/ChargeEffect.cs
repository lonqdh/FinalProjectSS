using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffect : MonoBehaviour
{
    private void Start()
    {

    }

    private void OnEnable()
    {
        this.transform.parent = LevelManager.Instance.player.chargeSkillPos;
        Invoke("OnDespawn", 2.0f);
    }

    private void OnDespawn()
    {
        LeanPool.Despawn(this);
    }
}
