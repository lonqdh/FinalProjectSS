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
    public GameObject pauseMenuUI;
    public GameObject gameplayUI;
    public GameObject finishGameUI;
    public GameObject heroShopUI;
    public GameObject loadingScreen;
    public GameObject spellbookUI;
    public GameObject heroShopUITest;

    //SkillAcquiredIconCooldown
    public SkillCooldown acquiredSkillPrefab;
    public SkillRow skillRowPrefab;
    public GameObject acquiredSkillGroup;
    public GameObject acquiredSkillRowGroup;


    //Buttons
    public Button openingStartGameButton;
    public Button resumeGameButton;
    public Button startGameButton;
    public Button retryGameButton;
    public Button replaceSkillButton;
    public Button heroShopButton;
    public Button closeHeroShopButton;
    public Button pauseToMenuButton;

    //Texts
    public TextMeshProUGUI nextLevelText;
    //public TextMeshProUGUI killCountText;
    public Text currentLevelText;
    public Text killCountText;
    public TextMeshProUGUI totalKillText;
    public TextMeshProUGUI goldEarnedText;
    public TextMeshProUGUI addOrReplaceSkillsText;


    //Effects
    //public GameObject mainMenuBackgroundEffect;
    public List<SkillCooldown> skillCooldownUIList;
    public List<SkillRow> skillRowList;

    public bool isPaused = false;

    private void Start()
    {
        EnterMainMenuUI();

        openingStartGameButton.onClick.AddListener(EnterMainMenuUI);
        startGameButton.onClick.AddListener(EnterMatch);
        retryGameButton.onClick.AddListener(RestartMatch);
        pauseToMenuButton.onClick.AddListener(EnterMainMenuUI);
        replaceSkillButton.onClick.AddListener(ChangeToReplaceSkillUI);
        heroShopButton.onClick.AddListener(OpenHeroShopUI);
        resumeGameButton.onClick.AddListener(ResumeGame);
        //heroShopButton.onClick.AddListener(OpenHeroShopUITest);

        closeHeroShopButton.onClick.AddListener(CloseHeroShopUI);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        GameManager.Instance.ChangeState(GameState.Pause);
        pauseMenuUI.SetActive(true);
        spellbookUI.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ChangeState(GameState.Gameplay);
        pauseMenuUI.SetActive(false);
        spellbookUI.SetActive(false);
    }

    public void OpenLevelUpUI()
    {
        GameManager.Instance.ChangeState(GameState.LevelUp);
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
        if(GameManager.Instance.IsState(GameState.Pause))
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            gameplayUI.SetActive(false);
            LevelManager.Instance.EndGame();
        }
        
        GameManager.Instance.ChangeState(GameState.MainMenu);
        openingGameUI.SetActive(false);
        mainMenuUI.SetActive(true);
        //LevelManager.Instance.camera.enabled = true;
        //mainMenuBackgroundEffect.SetActive(true);
    }

    //public void EnterMatch()
    //{
    //    GameManager.Instance.ChangeState(GameState.Gameplay);
    //    loadingScreen.SetActive(true);
    //    LevelManager.Instance.OnInit();


    //    mainMenuUI.SetActive(false);
    //    finishGameUI.SetActive(false);
    //    gameplayUI.SetActive(true);
    //}

    public void EnterMatch()
    {
        mainMenuUI.SetActive(false);
        loadingScreen.SetActive(true);
        LevelManager.Instance.OnInit();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        // Wait for a few seconds before showing the game
        yield return new WaitForSeconds(2.0f); // Adjust the delay time as needed
        
        // Initialize the level manager and other necessary components
        mainMenuUI.SetActive(false);
        finishGameUI.SetActive(false);
        loadingScreen.SetActive(false);
        gameplayUI.SetActive(true);

        // Hide the loading screen
        loadingScreen.SetActive(false);

        GameManager.Instance.ChangeState(GameState.Gameplay);
    }

    public void FinishMatch()
    {
        GameManager.Instance.ChangeState(GameState.Finish);
        LevelManager.Instance.FinishGameCalculations();
        
        //gameplayUI.SetActive(false);
        finishGameUI.SetActive(true);
    }
    
    private void RestartMatch()
    {
        LevelManager.Instance.RestartGame();
    }

    public void LoadingNextLevel()
    {
        mainMenuUI.SetActive(false);
        finishGameUI.SetActive(false);
        gameplayUI.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(StartGame());
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
