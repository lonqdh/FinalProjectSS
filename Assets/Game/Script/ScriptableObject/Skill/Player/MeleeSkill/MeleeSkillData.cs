using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Skill", menuName = "Skills/Melee Skill")]
public class MeleeSkillData : SkillData
{
    public Melee meleeSkill;
    public float radius;
    public override void Activate(Vector3 position, Transform chargePos, Character attacker)
    {
        // Implement area of effect skill activation logic here
        if (meleeSkill != null)
        {
            //Melee melee = LeanPool.Spawn(meleeSkill, chargePos.position, meleeSkill.transform.rotation);
            //Melee melee = LeanPool.Spawn(meleeSkill, chargePos.position, attacker.transform.rotation);
            Quaternion adjustedRotation = Quaternion.Euler(0, 180, 0) * attacker.transform.rotation;
            Melee melee = LeanPool.Spawn(meleeSkill, chargePos.position, adjustedRotation, attacker.transform);
            //melee.transform.SetParent(attacker.transform);
            melee.attacker = attacker;

            //base.Activate(position, chargePos, attacker);
        }

    }
}

