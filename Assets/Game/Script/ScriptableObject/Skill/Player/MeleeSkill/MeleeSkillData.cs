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
            Melee melee = LeanPool.Spawn(meleeSkill, chargePos.position, meleeSkill.transform.rotation);
            melee.attacker = attacker;

            base.Activate(position, chargePos, attacker);
        }

    }
}

