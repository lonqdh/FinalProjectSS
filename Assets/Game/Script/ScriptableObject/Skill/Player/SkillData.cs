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
    public Sprite skillIcon;
    public SkillType skillType;
    public string skillName;
    public string description;
    public float damage;
    public float cooldown;
    public float rangeRadius;
    public int castTime;

    //private float cooldownTimer = 0f;
    //private bool isOnCooldown = false;

    //public bool IsOnCooldown => isOnCooldown;
    //public Character attacker;

    public abstract void Activate(Vector3 position, Transform chargePos, Character attacker);

    // Method to activate the skill
    //public void ActivateSkill(Vector3 position, Transform chargePos, Character attacker)
    //{
    //    if (!isOnCooldown)
    //    {
    //        Activate(position, chargePos, attacker);
    //        StartCooldown();
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Skill is still on cooldown.");
    //    }
    //}

    //// Method to start the cooldown
    //private void StartCooldown()
    //{
    //    cooldownTimer = cooldown;
    //    isOnCooldown = true;
    //    CooldownRoutine());
    //}

    //// Coroutine to handle cooldown countdown
    //private IEnumerator CooldownRoutine()
    //{
    //    while (cooldownTimer > 0f)
    //    {
    //        cooldownTimer -= Time.deltaTime;
    //        yield return null;
    //    }
    //    isOnCooldown = false;
    //}

}
