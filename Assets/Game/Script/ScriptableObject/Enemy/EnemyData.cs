using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Melee = 0,
    Ranged = 1,
}

[Serializable]
public class EnemyData
{
    public EnemyType enemyType;
    //public GameObject enemyModelPrefab;
    public Enemy enemy;
    public int health;
    public float damage;
    public float movementSpeed;
    public SkillData enemySkill;
    public int enemyLevel; // de thiet lap spawn dua tren progress cua match
    public float attackRange;

    //public SkillData basicSkill;
}
