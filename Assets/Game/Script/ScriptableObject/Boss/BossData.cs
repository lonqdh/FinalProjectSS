using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    Melee = 0,
    Ranged = 1,
}

[Serializable]
public class BossData
{
    public Boss boss;
    public BossType bossType;
    public int health;
    public float damage;
    public float movementSpeed;
    public SkillData[] bossSkills;
    public GameObject hitVfx;
    public GameObject deathVfx;
    //public SkillData bossSkill1;
    //public SkillData bossSkill2;
    //public SkillData bossSkill3;

    //public int enemyLevel; // de thiet lap spawn dua tren progress cua match
    public float attackRange;
}
