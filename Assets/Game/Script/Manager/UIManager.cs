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
    public GameObject openingGameUI;
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject finishGameUI;

    //SkillAcquiredIconCooldown
    public List<Sprite> acquiredSkillImageList;
    public Image acquiredSkillPrefab;
    public GameObject acquiredSkillGroup;

    //Buttons
    public Button openingStartGameButton;
    public Button startGameButton;
    public Button retryGameButton;

    //Texts
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI totalKillText;
    public TextMeshProUGUI goldEarnedText;

    //Effects
    //public GameObject mainMenuBackgroundEffect;

    private void Start()
    {
        openingStartGameButton.onClick.AddListener(EnterMainMenuUI);
        startGameButton.onClick.AddListener(EnterMatch);
        retryGameButton.onClick.AddListener(RestartMatch);
    }

    public void OpenLevelUpUI()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        levelUpUI.SetActive(true);
        SkillsChoosingContent.Instance.SpawnSkills();
    }

    public void CloseLevelUpUI()
    {
        levelUpUI.SetActive(false);
        GameManager.Instance.ChangeState(GameState.Gameplay);
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
