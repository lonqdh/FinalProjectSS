using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum SkillType
{
    Projectile,
    AreaOfEffect
}


public abstract class SkillData : ScriptableObject
{
    //public GameObject visualEffectPrefab;
    public ChargeEffect chargeEffectPrefab;
    public SkillType skillType;
    public string skillName;
    public string description;
    public float damage;
    public float cooldown;
    public float rangeRadius;
    public int castTime;
    //public Character attacker;

    public abstract void Activate(Vector3 position, Transform chargePos, Character attacker);

    public void OnInit()
    {

    }

}
