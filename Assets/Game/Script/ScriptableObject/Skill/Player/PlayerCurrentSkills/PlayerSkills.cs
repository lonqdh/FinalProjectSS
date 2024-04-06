using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Player Skills", menuName = "Player/Player Skills")]
public class PlayerSkills : Singleton<PlayerSkills>
{
    public List<SkillData> currentSkills = new List<SkillData>();
    private const int MaxSkillCount = 6;

    // Method to add a skill to the player's skill list
    public void AddSkill(SkillData skill)
    {
        if (currentSkills.Count < MaxSkillCount)
        {
            currentSkills.Add(skill);

            //UIManager.Instance.acquiredSkillImageList.Add(skill.skillIcon);
            var newSkill = Instantiate(UIManager.Instance.acquiredSkillPrefab);
            newSkill.transform.SetParent(UIManager.Instance.acquiredSkillGroup.transform);
            newSkill.sprite = skill.skillIcon;
            //newSkill.GetComponent<SkillCooldown>().skill = skill;
            //newSkill.GetComponent<SkillCooldown>().skillIcon.sprite = newSkill.sprite;
            //newSkill.GetComponent<SkillCooldown>().cooldownTime = skill.cooldown;


            //var newSkillCooldownUI = newSkill.GetComponent<SkillCooldown>();
            //newSkillCooldownUI.skillIcon.sprite = newSkill.sprite;
            //newSkillCooldownUI.cooldownTime = skill.cooldown;
        }
        //else
        //{
        //    int skillIndexInList = currentSkills.IndexOf(skill);
        //    ReplaceSkill(skillIndexInList, skill);
        //}
    }

    // Method to replace an existing skill with a new one
    public void ReplaceSkill(int index, SkillData newSkill)
    {
        if (index >= 0 && index < currentSkills.Count)
        {
            currentSkills[index] = newSkill;
        }
        else
        {
            Debug.LogWarning("Invalid skill index.");
        }
    }

    // Method to activate a skill by index
    public void ActivateSkill(int index)
    {
        if (index >= 0 && index < currentSkills.Count)
        {
            SkillData skill = currentSkills[index];
            // Implement skill activation logic here
            Debug.Log("Activating skill: " + skill.skillName);
        }
        else
        {
            Debug.LogWarning("Invalid skill index.");
        }
    }

    
}
