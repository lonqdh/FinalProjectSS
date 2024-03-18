using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Skills", menuName = "Player/Player Skills")]
public class PlayerSkills : ScriptableObject
{
    public List<SkillData> skills = new List<SkillData>();
    private const int MaxSkillCount = 6;

    // Method to add a skill to the player's skill list
    public void AddSkill(SkillData skill)
    {
        if (skills.Count < MaxSkillCount)
        {
            skills.Add(skill);
        }
        else
        {
            Debug.LogWarning("Cannot add more skills. Maximum skill count reached.");
        }
    }

    // Method to replace an existing skill with a new one
    public void ReplaceSkill(int index, SkillData newSkill)
    {
        if (index >= 0 && index < skills.Count)
        {
            skills[index] = newSkill;
        }
        else
        {
            Debug.LogWarning("Invalid skill index.");
        }
    }

    // Method to activate a skill by index
    public void ActivateSkill(int index)
    {
        if (index >= 0 && index < skills.Count)
        {
            SkillData skill = skills[index];
            // Implement skill activation logic here
            Debug.Log("Activating skill: " + skill.skillName);
        }
        else
        {
            Debug.LogWarning("Invalid skill index.");
        }
    }
}
