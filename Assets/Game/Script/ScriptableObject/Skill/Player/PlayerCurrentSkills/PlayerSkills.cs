using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Player Skills", menuName = "Player/Player Skills")]
public class PlayerSkills : Singleton<PlayerSkills>
{
    public List<SkillData> currentSkills = new List<SkillData>();
    private const int MaxSkillCount = 6;

    // Dictionary to map skills to their corresponding UI objects
    private Dictionary<SkillData, SkillCooldown> skillCooldownMap = new Dictionary<SkillData, SkillCooldown>();

    public List<SkillCooldown> skillCooldownGameObjects = new List<SkillCooldown>();

    // Method to add a skill to the player's skill list
    public void AddSkill(SkillData skill)
    {
        Debug.Log("Start Add Skill");
        if (currentSkills.Count < MaxSkillCount /*&& !currentSkills.Contains(skill) */&& !currentSkills.Exists(s => s.skillName.Equals(skill.skillName)))
        {
            currentSkills.Add(skill);
            Debug.Log("Added");
            SkillCooldown newSkillUI = InstantiateSkillUI(skill);
            UIManager.Instance.skillCooldownUIList.Add(newSkillUI);
            skillCooldownMap.Add(skill, newSkillUI);
        }
        else if (/*currentSkills.Contains(skill) && */currentSkills.Exists(s => s.skillName.Equals(skill.skillName)))
        {
            UpgradeSkill(skill);
            Debug.Log("Run");
        }
        else
        {
            Debug.Log("Nothing Happens");
            return;
        }
    }

    public void ResetSkill()
    {
        currentSkills.Clear();
        UIManager.Instance.skillCooldownUIList.Clear();
        foreach (SkillCooldown skillCooldown in skillCooldownGameObjects)
        {
            Destroy(skillCooldown.gameObject);
        }
        skillCooldownGameObjects.Clear();

        skillCooldownMap.Clear();
    }

    //public void UpgradeSkill(SkillData skill)
    //{
    //    // Create a new instance of the skill data
    //    SkillData upgradedSkillData = Instantiate(skill);

    //    upgradedSkillData.skillName = skill.skillName;
    //    upgradedSkillData.skillIcon = skill.skillIcon;
    //    upgradedSkillData.damage = skill.damage + 10f;
    //    upgradedSkillData.cooldown = skill.cooldown - 0.5f;
    //    upgradedSkillData.rangeRadius = skill.rangeRadius + 2f;
    //    upgradedSkillData.castTime = skill.castTime;

    //    // Replace the original skill data with the upgraded one
    //    int oldIndex = currentSkills.IndexOf(skill);
    //    if (oldIndex != -1 && oldIndex < currentSkills.Count)
    //    {
    //        currentSkills[oldIndex] = upgradedSkillData;
    //        Debug.Log("Skill Upgraded: " + upgradedSkillData.skillName);
    //    }
    //    else
    //    {
    //        Debug.LogError("Invalid index for skill upgrade.");
    //    }

    //    // Update the UI for the upgraded skill
    //    for (int i = 0; i < UIManager.Instance.skillRowList.Count; i++)
    //    {
    //        if (UIManager.Instance.skillRowList[i].skillRowData.skillName.Equals(skill.skillName))
    //        {
    //            UIManager.Instance.skillRowList[i].skillRowData = upgradedSkillData;
    //            UIManager.Instance.skillRowList[i].skillRowName.text = upgradedSkillData.skillName;
    //            UIManager.Instance.skillRowList[i].skillRowDescription.text = "Damage : " + upgradedSkillData.damage + " - Cooldown : " + upgradedSkillData.cooldown;
    //            Debug.Log("Skill Row Updated: " + upgradedSkillData.skillName);
    //            break;
    //        }
    //    }
    //}

    public void UpgradeSkill(SkillData skill)
    {
        // Find the index of the skill in the currentSkills list
        int index = currentSkills.FindIndex(s => s.skillName.Equals(skill.skillName));

        // Check if the skill exists in the currentSkills list
        if (index != -1)
        {
            // Create a copy of the original skill data
            SkillData upgradedSkillData = Instantiate(skill);

            // Modify the copied skill data
            upgradedSkillData.damage += 10f * LevelManager.Instance.player.playerExperience.level; // phai lam the nay, vi minh dang tao 1 ban copy tu skill base, cho nen moi khi tao 1 thg clone no se lai lay lai stats cua skill base cho nen gay ra bug la up mai up mai thi no van chi + 10f stats so voi ban dau ( vi du base dmg = 60 --> up 1 ti lan thi van la dmg = 70 )
            upgradedSkillData.cooldown -= 0.25f;
            upgradedSkillData.rangeRadius += 2f;

            // Replace the original skill data with the upgraded one
            currentSkills[index] = upgradedSkillData;

            Debug.Log("Skill Upgraded: " + upgradedSkillData.skillName);

            // Update the UI for the upgraded skill
            for (int i = 0; i < UIManager.Instance.skillRowList.Count; i++)
            {
                if (UIManager.Instance.skillRowList[i].skillRowData.skillName.Equals(skill.skillName))
                {
                    UIManager.Instance.skillRowList[i].skillRowData = upgradedSkillData;
                    UIManager.Instance.skillRowList[i].skillRowName.text = upgradedSkillData.skillName;
                    UIManager.Instance.skillRowList[i].skillRowDescription.text = "Damage : " + upgradedSkillData.damage + " - Cooldown : " + upgradedSkillData.cooldown;
                    Debug.Log("Skill Row Updated: " + upgradedSkillData.skillName);
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Skill not found in the currentSkills list: " + skill.skillName);
        }
    }


    //Method to replace an existing skill with a new one
    public void ReplaceSkill(int index, SkillData newSkill)
    {
        Debug.Log("Replacing Skills");
        if (index >= 0 && index < currentSkills.Count)
        {
            SkillData oldSkill = currentSkills[index];
            //currentSkills.Add(newSkill);
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
        SkillCooldown newSkillUI = Instantiate(UIManager.Instance.acquiredSkillPrefab, UIManager.Instance.acquiredSkillGroup.transform);
        skillCooldownGameObjects.Add(newSkillUI);
        //newSkillUI.transform.SetParent(UIManager.Instance.acquiredSkillGroup.transform);
        newSkillUI.skillIcon.sprite = skill.skillIcon;
        newSkillUI.skill = skill;


        SkillRow skillItem = Instantiate(UIManager.Instance.skillRowPrefab, UIManager.Instance.acquiredSkillRowGroup.transform);
        skillItem.skillRowData = skill;
        skillItem.skillRowName.text = skill.skillName;
        skillItem.skillRowDescription.text = "Damage : " + skill.damage + " - Cooldown : " + skill.cooldown;
        skillItem.skillRowIcon.sprite = skill.skillIcon;

        if (GameManager.Instance.IsState(GameState.MainMenu) && UIManager.Instance.skillRowList.Count > 0)
        {
            Destroy(UIManager.Instance.skillRowList[0].gameObject);
            Debug.Log("Destroyed :" + UIManager.Instance.skillRowList[0]);
            UIManager.Instance.skillRowList.RemoveAt(0);

            //return newSkillUI;
        }

        UIManager.Instance.skillRowList.Add(skillItem);

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

