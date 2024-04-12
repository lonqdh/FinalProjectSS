using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillsChoosingContent : Singleton<SkillsChoosingContent>
{
    public SkillDatabase skillDatabase;
    [SerializeField] private SkillsButton skillBtnPrefab;
    [SerializeField] private Transform skillItemParent;
    public List<SkillData> skillsList = new List<SkillData>();
    public List<SkillsButton> skillsButtonList = new List<SkillsButton>();
    public bool replacingSkillSession = false;
    public int skillToReplaceIndex;

    private void Start()
    {
        //skillToReplaceIndex = 10;
    }

    public void SpawnSkills()
    {
        GetRandomSkills();
        
        for (int i = 0; i < 3; i++)
        {
            //if (PlayerSkills.Instance.currentSkills.Contains(skillsList[i]))
            //{
            //    //PlayerSkills.Instance
            //    Debug.Log("Upgrade Skills Stats");
            //}
            //else
            //{
                SkillsButton skillButton = Instantiate(skillBtnPrefab, skillItemParent);
                skillButton.skillData = skillsList[i];
                skillButton.skillImage.sprite = skillsList[i].skillIcon;
                skillButton.skillName.text = skillsList[i].skillName;
                skillsButtonList.Add(skillButton);
            //Debug.Log("Replacable Skills Button Added : " +  skillButton.name);
            //}
        }
    }

    public void SpawnCurrentSkillsCanBeReplaced()
    {
        foreach (SkillsButton btn in skillsButtonList)
        {
            Destroy(btn.gameObject);
        }
        skillsButtonList.Clear();
        skillsList.Clear();
        
        foreach (SkillData skillData in PlayerSkills.Instance.currentSkills)
        {
            SkillsButton skillButton = Instantiate(skillBtnPrefab, skillItemParent);
            skillButton.skillData = skillData;
            skillButton.skillImage.sprite = skillData.skillIcon;
            skillButton.skillName.text = skillData.skillName;
            skillsButtonList.Add(skillButton);
            skillsList.Add(skillData);
            //Debug.Log("Replacable Skills Button Added for : " + skillData.skillName);
        }
    }

    public void SpawnSkillsToReplace()
    {
        GetReplacableSkills();
        for (int i = 0; i < 3; i++)
        {
            SkillsButton skillButton = Instantiate(skillBtnPrefab, skillItemParent);
            skillButton.skillData = skillsList[i];
            skillButton.skillImage.sprite = skillsList[i].skillIcon;
            skillButton.skillName.text = skillsList[i].skillName;
            skillsButtonList.Add(skillButton);
            Debug.Log("Replacable Skills Button Added for : " + skillsList[i].skillName);
        }
        Debug.Log("Current Replace Skill Session Is : " + replacingSkillSession);
        Debug.Log("Current Replace Skill Index is : " + skillToReplaceIndex);
    }

    private void GetReplacableSkills()
    {
        foreach (SkillsButton btn in skillsButtonList)
        {
            Destroy(btn.gameObject);
        }
        skillsButtonList.Clear();
        skillsList.Clear();
        Debug.Log("Old buttons destroyed ");
        int skillIndex;
        for (int i = 0; i < 3; i++)
        {
            do
            {
                skillIndex = Random.Range(0, skillDatabase.allSkillsList.Count);
            } while (PlayerSkills.Instance.currentSkills.Contains(skillDatabase.allSkillsList[skillIndex]));

            skillsList.Add(skillDatabase.allSkillsList[skillIndex]);
            Debug.Log("Replacable Skill Added : " + skillDatabase.allSkillsList[skillIndex].name);
        }
        Debug.Log("Get Replacable Skills Finished!");
    }

    private void GetRandomSkills()
    {
        foreach(SkillsButton btn in skillsButtonList)
        {
            Destroy(btn.gameObject);
        }
        skillsButtonList.Clear();
        skillsList.Clear();
        int skillIndex;
        for (int i = 0; i < 3; i++)
        {
            do
            {
                skillIndex = Random.Range(0, skillDatabase.allSkillsList.Count);
            } while (skillsList.Contains(skillDatabase.allSkillsList[skillIndex]));

            skillsList.Add(skillDatabase.allSkillsList[skillIndex]);

        }
    }
}
