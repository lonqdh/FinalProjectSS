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
    [NonSerialized] public SkillData skillData;
    public Image skillImage;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;


    private void Start()
    {
        skillButton.onClick.AddListener(SkillButtonOnClick);
    }

    private void SkillButtonOnClick()
    {
        if(skillData != null && PlayerSkills.Instance.currentSkills.Count < 6)
        {
            Debug.Log("Skill Button Clicked!");
            PlayerSkills.Instance.AddSkill(skillData);
            UIManager.Instance.CloseLevelUpUI();
        }
        //else
        //{
        //    int skillindexinlist = PlayerSkills.Instance.currentSkills.IndexOf(skillData);
        //    replaceskill(skillindexinlist, skill);
        //    playerskills.instance.replaceskill(skilldata);
        //}

    }
}
