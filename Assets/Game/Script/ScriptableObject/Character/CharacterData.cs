using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Knight = 0,
    Sorcerer = 1,
    Gunner = 2,
}

[Serializable]
public class CharacterData
{
    public CharacterType characterType;
    public GameObject characterModelPrefab;
    public int health;
    public float damage;
    public float movementSpeed;
    public int characterCost;

    public SkillData basicSkill;
}
