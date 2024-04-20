using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //Cameras
    public GameObject mainCamera;
    public GameObject heroShopCamera;

    //Panels
    public GameObject levelUpUI;
    public GameObject openingGameUI;
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject finishGameUI;
    public GameObject heroShopUI;

    public GameObject heroShopUITest;

    //SkillAcquiredIconCooldown
    public SkillCooldown acquiredSkillPrefab;
    public SkillRow skillRowPrefab;
    public GameObject acquiredSkillGroup;
    public GameObject acquiredSkillRowGroup;


    //Buttons
    public Button openingStartGameButton;
    public Button startGameButton;
    public Button retryGameButton;
    public Button replaceSkillButton;
    public Button heroShopButton;
    public Button closeHeroShopButton;

    //Texts
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI totalKillText;
    public TextMeshProUGUI goldEarnedText;
    public TextMeshProUGUI addOrReplaceSkillsText;

    //Effects
    //public GameObject mainMenuBackgroundEffect;
    public List<SkillCooldown> skillCooldownUIList;
    public List<SkillRow> skillRowList;



    private void Start()
    {
        EnterMainMenuUI();
        openingStartGameButton.onClick.AddListener(EnterMainMenuUI);
        startGameButton.onClick.AddListener(EnterMatch);
        retryGameButton.onClick.AddListener(RestartMatch);
        replaceSkillButton.onClick.AddListener(ChangeToReplaceSkillUI);
        heroShopButton.onClick.AddListener(OpenHeroShopUI);
        //heroShopButton.onClick.AddListener(OpenHeroShopUITest);

        closeHeroShopButton.onClick.AddListener(CloseHeroShopUI);
    }

    public void OpenLevelUpUI()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        levelUpUI.SetActive(true);
        addOrReplaceSkillsText.text = "Choose Skill To Acquire";
        SkillsChoosingContent.Instance.SpawnSkills();
    }

    public void OpenHeroShopUI()
    {
        heroShopUI.SetActive(true);
        HeroShopContent.Instance.SpawnCharacters();
    }

    public void CloseHeroShopUI()
    {
        heroShopUI.SetActive(false);
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
        GameManager.Instance.ChangeState(GameState.MainMenu);
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

    public void OpenHeroShopUITest()
    {
        mainCamera.SetActive(false);
        heroShopUITest.SetActive(true);
        heroShopCamera.SetActive(true);
        mainMenuUI.SetActive(false);
        //HeroShopContent.Instance.SpawnCharacters();
    }

    public void CloseHeroShopUITest()
    {
        heroShopUITest.SetActive(false);
        heroShopCamera.SetActive(false);
        mainCamera.SetActive(true);
    }


}
