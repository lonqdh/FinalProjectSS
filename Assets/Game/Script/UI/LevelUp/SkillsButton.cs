using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillsButton : MonoBehaviour
{
    [SerializeField] private Button skillButton;
    [SerializeField] public SkillData skillData;
    public Image skillImage;
    //public TextMeshProUGUI skillName;
    public Text skillName;
    //public TextMeshProUGUI skillDescription;
    public Text skillDescription;


    private void Start()
    {
        skillButton.onClick.AddListener(SkillButtonOnClick);
    }

    private void SkillButtonOnClick()
    {
        if(skillData != null && SkillsChoosingContent.Instance.replacingSkillSession == false/* && PlayerSkills.Instance.currentSkills.Count < 6*/)
        {
            Debug.Log("Skill Button Clicked!");
            PlayerSkills.Instance.AddSkill(skillData);
            UIManager.Instance.CloseLevelUpUI();
        }
        else if(skillData != null && SkillsChoosingContent.Instance.replacingSkillSession == true && SkillsChoosingContent.Instance.skillToReplaceIndex == -1)
        {
            SkillsChoosingContent.Instance.skillToReplaceIndex = PlayerSkills.Instance.currentSkills.IndexOf(skillData);
            Debug.Log("Skills Want To Replace Is : " + PlayerSkills.Instance.currentSkills[SkillsChoosingContent.Instance.skillToReplaceIndex].skillName);

            //if (SkillsChoosingContent.Instance.skillToReplaceIndex != -1)
            //{
            SkillsChoosingContent.Instance.SpawnSkillsToReplace();

            //}
            //else
            //{
            //Debug.LogWarning("SkillData not found in currentSkills list.");
            //}
        }
        else if(skillData != null && SkillsChoosingContent.Instance.skillToReplaceIndex != -1 && SkillsChoosingContent.Instance.replacingSkillSession == true)
        {
            PlayerSkills.Instance.ReplaceSkill(SkillsChoosingContent.Instance.skillToReplaceIndex, skillData);
            UIManager.Instance.CloseLevelUpUI();
            //SkillsChoosingContent.Instance.replacingSkillSession = false;
        }
        else
        {
            Debug.Log("THE PROBLEM IS THE BUTTON");
        }
        //else
        //{
        //    int skillindexinlist = PlayerSkills.Instance.currentSkills.IndexOf(skillData);
        //    replaceskill(skillindexinlist, skill);
        //    playerskills.instance.replaceskill(skilldata);
        //}

    }
}
