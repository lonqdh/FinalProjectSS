using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //Cameras
    public GameObject mainCamera;

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
    public GameObject messageUIPanel;
    public GameObject achievementUIPanel;
    public GameObject settingUIPanel;

    public GameObject bossEncounterWarning;


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
    public Button changeProjectileCastButton;
    public Button menuMessageButton;
    public Button menuAchievementButton;
    public Button menuSettingButton;
    public Button closePanelsButton;
    public Button restartMatchButton;
    public Button finishMatchToMenuButton;



    //Texts
    public TextMeshProUGUI nextLevelText;
    //public TextMeshProUGUI killCountText;
    public Text currentLevelText;
    public Text killCountText;
    public Text currentCoinText;
    public Text projectileCastTypeText;
    public TextMeshProUGUI scoreText;
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
        changeProjectileCastButton.onClick.AddListener(ChangeProjectileCastType);
        menuAchievementButton.onClick.AddListener(OpenAchivementMenuPanel);
        menuMessageButton.onClick.AddListener(OpenMessageMenuPanel);
        menuSettingButton.onClick.AddListener(OpenSettingMenuPanel);
        restartMatchButton.onClick.AddListener(RestartMatch);
        //heroShopButton.onClick.AddListener(OpenHeroShopUITest);
        closePanelsButton.onClick.AddListener(ClosePanels);
        closeHeroShopButton.onClick.AddListener(CloseHeroShopUI);
        finishMatchToMenuButton.onClick.AddListener(EnterMainMenuUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.IsState(GameState.Gameplay) || GameManager.Instance.IsState(GameState.Pause))
            {
                TogglePause();
            }
        }
    }

    private void ClosePanels()
    {
        if (settingUIPanel.activeSelf == true)
        {
            settingUIPanel.SetActive(false);
            closePanelsButton.gameObject.SetActive(false);
        }
        else if (messageUIPanel.activeSelf == true)
        {
            messageUIPanel.SetActive(false);
            closePanelsButton.gameObject.SetActive(false);
        }
        else if (achievementUIPanel.activeSelf == true)
        {
            achievementUIPanel.SetActive(false);
            closePanelsButton.gameObject.SetActive(false);
        }
    }

    private void OpenSettingMenuPanel()
    {
        settingUIPanel.SetActive(true);
        closePanelsButton.gameObject.SetActive(true);
        closePanelsButton.transform.parent = settingUIPanel.transform;
    }

    private void OpenMessageMenuPanel()
    {
        messageUIPanel.SetActive(true);
        closePanelsButton.gameObject.SetActive(true);
        closePanelsButton.transform.parent = messageUIPanel.transform;
    }

    private void OpenAchivementMenuPanel()
    {
        achievementUIPanel.SetActive(true);
        closePanelsButton.gameObject.SetActive(true);
        closePanelsButton.transform.parent = achievementUIPanel.transform;
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
        replaceSkillButton.gameObject.SetActive(true);
        addOrReplaceSkillsText.text = "Choose Skill To Acquire";
        SkillsChoosingContent.Instance.SpawnSkills();
    }

    public void OpenHeroShopUI()
    {
        heroShopUI.SetActive(true);
        currentCoinText.text = GameManager.Instance.UserData.CurrentCoins.ToString();
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
        replaceSkillButton.gameObject.SetActive(false);
        addOrReplaceSkillsText.text = "Choose Skill To Replace";
        SkillsChoosingContent.Instance.replacingSkillSession = true;
        SkillsChoosingContent.Instance.SpawnCurrentSkillsCanBeReplaced();
    }

    private void ChangeProjectileCastType()
    {
        if (LevelManager.Instance.player.projectileCastType == 0)
        {
            LevelManager.Instance.player.projectileCastType = 1;
            projectileCastTypeText.text = "All Directions Type";

        }
        else
        {
            LevelManager.Instance.player.projectileCastType = 0;
            projectileCastTypeText.text = "Cone Type";
        }
    }

    public void CloseLevelUpUI()
    {
        levelUpUI.SetActive(false);
        GameManager.Instance.ChangeState(GameState.Gameplay);
        SkillsChoosingContent.Instance.replacingSkillSession = false;
    }

    public void EnterMainMenuUI()
    {
        if (GameManager.Instance.IsState(GameState.Pause) || GameManager.Instance.IsState(GameState.Finish))
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            finishGameUI.SetActive(false);
            gameplayUI.SetActive(false);
            LevelManager.Instance.EndGame();
        }

        GameManager.Instance.ChangeState(GameState.MainMenu);
        openingGameUI.SetActive(false);
        mainMenuUI.SetActive(true);
        //LevelManager.Instance.camera.enabled = true;
        //mainMenuBackgroundEffect.SetActive(true);
    }

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

        GameManager.Instance.ChangeState(GameState.Gameplay);
    }

    public void FinishMatch()
    {
        GameManager.Instance.ChangeState(GameState.Finish);
        //LevelManager.Instance.FinishGameCalculations();

        totalKillText.SetText(LevelManager.Instance.killCount.ToString());
        goldEarnedText.SetText((LevelManager.Instance.killCount * 10).ToString());

        // Calculate final score with random value within a range
        int minScore = 100; // Adjust these values as needed
        int maxScore = 500;
        int finalScore = Random.Range(minScore, maxScore * LevelManager.Instance.killCount);

        scoreText.SetText(finalScore.ToString());
        //currentLevelText.text = "LEVEL " + LevelManager.Instance.player.playerExperience.level.ToString();

        //gameplayUI.SetActive(false);
        finishGameUI.SetActive(true);
    }

    private void StopGameToMenu()
    {

    }

    private void RestartMatch()
    {
        LevelManager.Instance.RestartGame();
        pauseMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(StartGame());
        ResumeGame();
    }

    public void LoadingNextLevel()
    {
        mainMenuUI.SetActive(false);
        finishGameUI.SetActive(false);
        gameplayUI.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(StartGame());
    }

    public void DestroySkillRow()
    {
        foreach (SkillRow skillrow in skillRowList)
        {
            Destroy(skillrow.gameObject);
        }

        skillRowList.Clear();
    }
}
