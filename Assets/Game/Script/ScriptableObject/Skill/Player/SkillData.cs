using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum SkillType
{
    Projectile,
    AreaOfEffect,
    Spray
}


public abstract class SkillData : ScriptableObject
{
    //public GameObject visualEffectPrefab;
    public ChargeEffect chargeEffectPrefab;
    public Sprite skillIcon;
    public SkillType skillType;
    public string skillName;
    public string description;
    public float damage;
    public float cooldown;
    public float rangeRadius;
    public int castTime;

    public event Action<SkillData> SkillActivated;

    public virtual void Activate(Vector3 position, Transform chargePos, Character attacker)
    {
        SkillActivated?.Invoke(this);
    }

}
