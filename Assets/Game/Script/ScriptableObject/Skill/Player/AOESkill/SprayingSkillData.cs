using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AoE Skill", menuName = "Skills/Breathing Skill")]
public class SprayingSkillData : SkillData
{
    public SprayingSkill spraySkill;
    public float sprayDuration = 5f; // Duration for which spray remains active
    public float damagePerSecond = 10f;

    public override void Activate(Vector3 position, Transform chargePos, Character attacker)
    {
        Vector3 direction = (position - chargePos.position).normalized;

        // Ensure the vertical component of the direction is not negative (pointing downward)
        //direction.y = Mathf.Max(direction.y, 0f);

        direction.y = 0; // set cho projectile luon luon bay 1 duong thang neu projectile khong useGravity

        // Normalize the direction vector
        direction.Normalize();

        SprayingSkill newSpraySkill = Instantiate(spraySkill, chargePos.position, Quaternion.LookRotation(direction));

        newSpraySkill.transform.SetParent(chargePos.transform);

        newSpraySkill.attacker = attacker;


    }
}
