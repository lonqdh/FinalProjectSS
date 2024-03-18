using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New AoE Skill", menuName = "Skills/AoE Skill")]
//[Serializable]
public class AreaSkillData : SkillData
{
    //public GameObject visualEffectPrefab; // Visual effect to instantiate when the skill is activated
    //public float radius; // Radius of the area of effect
    //public string skillName;
    //public SkillType skillType;
    //public int damage;
    //public float cooldown;

    public AreaOfEffect aoeSkill;
    public float radius;

    public override void Activate(Vector3 position, Transform chargePos)
    {
        // Implement area of effect skill activation logic here
        if (aoeSkill != null)
        {
            //if(chargeEffectPrefab != null)
            //{
            //    LeanPool.Spawn(chargeEffectPrefab.gameObject, chargePos.position, chargePos.rotation);
            //}

            AreaOfEffect aoe = LeanPool.Spawn(aoeSkill, position,aoeSkill.transform.rotation);
            
            //Instantiate(visualEffectPrefab, position, Quaternion.identity);
        }
        // Implement area of effect logic (e.g., apply damage in an area)
        Debug.Log("Area of effect skill activated!");
    }
}
