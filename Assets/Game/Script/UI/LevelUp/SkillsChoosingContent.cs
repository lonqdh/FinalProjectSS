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
            //}
        }
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
