using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayingSkill : AreaOfEffect
{
    public GameObject sprayPrefab; // Prefab of the spray object
    public float sprayDuration = 5f; // Duration for which spray remains active
    public float damagePerSecond = 10f;
    private bool isSpraying = false;

    private void Start()
    {

    }


    private void Update()   
    {
        transform.rotation = Quaternion.LookRotation(attacker.transform.forward);
    }


}
