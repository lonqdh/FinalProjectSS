using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Player Skills", menuName = "Player/Player Skills")]
public class PlayerSkills : Singleton<PlayerSkills>
{
    public List<SkillData> currentSkills = new List<SkillData>();
    private const int MaxSkillCount = 6;

    // Dictionary to map skills to their corresponding UI objects
    private Dictionary<SkillData, SkillCooldown> skillCooldownMap = new Dictionary<SkillData, SkillCooldown>();

    // Method to add a skill to the player's skill list
    public void AddSkill(SkillData skill)
    {
        if (currentSkills.Count < MaxSkillCount && !currentSkills.Contains(skill))
        {
            currentSkills.Add(skill);

            SkillCooldown newSkillUI = InstantiateSkillUI(skill);
            UIManager.Instance.skillCooldownUIList.Add(newSkillUI);
            skillCooldownMap.Add(skill, newSkillUI);
        }
        else if(currentSkills.Contains(skill))
        {
            UpgradeSkill(skill);
        }
    }

    public void UpgradeSkill(SkillData skill)
    {
        skill.damage += 10f;
        skill.cooldown -= 0.5f;
        skill.rangeRadius += 2f;
    }

    //Method to replace an existing skill with a new one
    public void ReplaceSkill(int index, SkillData newSkill)
    {
        if (index >= 0 && index < currentSkills.Count)
        {
            SkillData oldSkill = currentSkills[index];
            currentSkills[index] = newSkill;

            // Check if the old skill has a corresponding UI object
            if (skillCooldownMap.ContainsKey(oldSkill))
            {
                SkillCooldown oldSkillUI = skillCooldownMap[oldSkill];
                oldSkillUI.skill = newSkill;
                oldSkillUI.skillIcon.sprite = newSkill.skillIcon;

                // Update the mapping to associate the new skill with the existing UI object
                skillCooldownMap.Remove(oldSkill);
                skillCooldownMap.Add(newSkill, oldSkillUI);

                Debug.Log("Replaced Skill: " + oldSkill.skillName + " with " + newSkill.skillName);
            }
            else
            {
                Debug.LogWarning("No UI found for the replaced skill: " + oldSkill.skillName);
            }

            // Reset the skill replacement index
            SkillsChoosingContent.Instance.skillToReplaceIndex = -1;
        }
        else
        {
            Debug.LogWarning("Invalid skill index.");
        }
    }

    // Instantiate a SkillCooldown UI object for the given skill
    private SkillCooldown InstantiateSkillUI(SkillData skill)
    {
        SkillCooldown newSkillUI = Instantiate(UIManager.Instance.acquiredSkillPrefab);
        newSkillUI.transform.SetParent(UIManager.Instance.acquiredSkillGroup.transform);
        newSkillUI.skillIcon.sprite = skill.skillIcon;
        newSkillUI.skill = skill;
        return newSkillUI;
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

//public List<SkillData> currentSkills = new List<SkillData>();
//private const int MaxSkillCount = 6;

//// Dictionary to map skills to their corresponding UI objects
//private Dictionary<SkillData, SkillCooldown> skillCooldownMap = new Dictionary<SkillData, SkillCooldown>();

//// Method to add a skill to the player's skill list
//public void AddSkill(SkillData skill)
//{
//    if (currentSkills.Count < MaxSkillCount)
//    {
//        currentSkills.Add(skill);

//        SkillCooldown newSkillUI = InstantiateSkillUI(skill);
//        UIManager.Instance.skillCooldownUIList.Add(newSkillUI);
//        skillCooldownMap.Add(skill, newSkillUI);
//    }
//}

// Method to replace an existing skill with a new one
//public void ReplaceSkill(int index, SkillData newSkill)
//{
//    if (index >= 0 && index < currentSkills.Count)
//    {
//        SkillData oldSkill = currentSkills[index];
//        currentSkills[index] = newSkill;

//        // Check if the old skill has a corresponding UI object
//        if (skillCooldownMap.ContainsKey(oldSkill))
//        {
//            SkillCooldown oldSkillUI = skillCooldownMap[oldSkill];
//            oldSkillUI.skill = newSkill;
//            oldSkillUI.skillIcon.sprite = newSkill.skillIcon;

//            // Update the mapping to associate the new skill with the existing UI object
//            skillCooldownMap.Remove(oldSkill);
//            skillCooldownMap.Add(newSkill, oldSkillUI);

//            Debug.Log("Replaced Skill: " + oldSkill.skillName + " with " + newSkill.skillName);
//        }
//        else
//        {
//            Debug.LogWarning("No UI found for the replaced skill: " + oldSkill.skillName);
//        }

//        // Reset the skill replacement index
//        SkillsChoosingContent.Instance.skillToReplaceIndex = -1;
//    }
//    else
//    {
//        Debug.LogWarning("Invalid skill index.");
//    }
//}

//// Instantiate a SkillCooldown UI object for the given skill
//private SkillCooldown InstantiateSkillUI(SkillData skill)
//{
//    SkillCooldown newSkillUI = Instantiate(UIManager.Instance.acquiredSkillPrefab);
//    newSkillUI.transform.SetParent(UIManager.Instance.acquiredSkillGroup.transform);
//    newSkillUI.skillIcon.sprite = skill.skillIcon;
//    newSkillUI.skill = skill;
//    return newSkillUI;
//}

//// Method to activate a skill by index
//public void ActivateSkill(int index)
//{
//    if (index >= 0 && index < currentSkills.Count)
//    {
//        SkillData skill = currentSkills[index];
//        // Implement skill activation logic here
//        Debug.Log("Activating skill: " + skill.skillName);
//    }
//    else
//    {
//        Debug.LogWarning("Invalid skill index.");
//    }
//}