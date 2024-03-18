using System;
using System.Collections;
using System.Collections.Generic;
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

    public abstract void Activate(Vector3 position, Transform chargePos);

}
