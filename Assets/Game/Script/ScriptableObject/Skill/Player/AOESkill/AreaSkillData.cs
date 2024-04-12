using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New AoE Skill", menuName = "Skills/AoE Skill")]
//[Serializable]
public class AreaSkillData : SkillData
{
    public AreaOfEffect aoeSkill;
    public float radius;

    public override void Activate(Vector3 position, Transform chargePos, Character attacker)
    {
        // Implement area of effect skill activation logic here
        if (aoeSkill != null)
        {
            AreaOfEffect aoe = LeanPool.Spawn(aoeSkill, position, aoeSkill.transform.rotation);
            aoe.attacker = attacker;
            
            base.Activate(position, chargePos, attacker);
        }
        
    }
}

