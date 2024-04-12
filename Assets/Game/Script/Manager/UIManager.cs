using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //Panels
    public GameObject levelUpUI;
    //public GameObject replaceSkillUI;
    public GameObject openingGameUI;
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject finishGameUI;

    //SkillAcquiredIconCooldown
    //public List<Sprite> acquiredSkillImageList;
    public SkillCooldown acquiredSkillPrefab;
    public GameObject acquiredSkillGroup;

    //Buttons
    public Button openingStartGameButton;
    public Button startGameButton;
    public Button retryGameButton;
    public Button replaceSkillButton;

    //Texts
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI totalKillText;
    public TextMeshProUGUI goldEarnedText;
    public TextMeshProUGUI addOrReplaceSkillsText;

    //Effects
    //public GameObject mainMenuBackgroundEffect;
    public List<SkillCooldown> skillCooldownUIList;


    private void Start()
    {
        EnterMainMenuUI();
        openingStartGameButton.onClick.AddListener(EnterMainMenuUI);
        startGameButton.onClick.AddListener(EnterMatch);
        retryGameButton.onClick.AddListener(RestartMatch);
        replaceSkillButton.onClick.AddListener(ChangeToReplaceSkillUI);
    }

    public void OpenLevelUpUI()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        levelUpUI.SetActive(true);
        addOrReplaceSkillsText.text = "Choose Skill To Acquire";
        SkillsChoosingContent.Instance.SpawnSkills();
    }

    public void ChangeToReplaceSkillUI()
    {
        //levelUpUI.SetActive(false);
        //replaceSkillUI.SetActive(true);
        addOrReplaceSkillsText.text = "Choose Skill To Replace";
        SkillsChoosingContent.Instance.replacingSkillSession = true;
        SkillsChoosingContent.Instance.SpawnCurrentSkillsCanBeReplaced();
    }

    public void CloseLevelUpUI()
    {
        levelUpUI.SetActive(false);
        GameManager.Instance.ChangeState(GameState.Gameplay);
        SkillsChoosingContent.Instance.replacingSkillSession = false;
    }

    public void EnterMainMenuUI()
    {
        openingGameUI.SetActive(false);
        mainMenuUI.SetActive(true);
        //mainMenuBackgroundEffect.SetActive(true);
    }

    public void EnterMatch()
    {
        GameManager.Instance.ChangeState(GameState.Gameplay);
        mainMenuUI.SetActive(false);
        finishGameUI.SetActive(false);
        gameplayUI.SetActive(true);
    }

    public void FinishMatch()
    {
        GameManager.Instance.ChangeState(GameState.Finish);
        LevelManager.Instance.FinishGameCalculations();
        
        gameplayUI.SetActive(false);
        finishGameUI.SetActive(true);
    }
    
    private void RestartMatch()
    {
        LevelManager.Instance.RestartGame();
    }


}
