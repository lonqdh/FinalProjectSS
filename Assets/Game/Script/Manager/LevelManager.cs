using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public SkillDatabase skillDatabase;
    public Player player;
    public Player playerPrefab;
    public EnemyDataSO enemyDataSO;
    public BossDataSO bossDataSO;
    public CameraFollow camera;
    public int killCount;

    public List<Level> levelList = new List<Level>();
    public Level currentLevel;
    public int level = 1;

    public float spawnIntervalReduction = 0.1f;
    public float initialSpawnInterval = 3f;
    public float minSpawnInterval = 1f;
    [SerializeField] private float currentSpawnInterval;
    public int minEnemyLevel = 1;
    public int maxEnemyLevel = 5;
    [SerializeField] private List<EnemyData> eligibleEnemies;
    [SerializeField] private int enemyLevel = 1;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private ExperienceBar experienceBar;
    public PlayerSkills currentSkillsList;
    //[SerializeField] private Transform playerSpawnPosition;
    public bool nextLevelOptionCheck = false;
    public bool proceedToNextLevel = false;

    public bool bossSpawned = false;

    private Coroutine spawnEnemiesCoroutine;
    private Coroutine playerProgressCoroutine;


    private NavMeshTriangulation navMeshData; // for spawning enemies

    private void Start()
    {
        //OnInit();
    }

    private void Update()
    {
        if (GameManager.Instance.IsState(GameState.Gameplay))
        {
            if (killCount % 50 == 0 && bossSpawned == false)
            {
                SpawnBoss();
            }

            if (killCount >= currentLevel.killRequiredToNextLevel && nextLevelOptionCheck == false)
            {
                // Move to the next level
                //nextLevelOptionCheck = true;
                //if (spawnEnemiesCoroutine != null)
                //{
                //    StopCoroutine(spawnEnemiesCoroutine);
                //}
                //if (playerProgressCoroutine != null)
                //{
                //    StopCoroutine(playerProgressCoroutine);
                //}
                currentLevel.SpawnPortals();
                level++;
            }
        }

        //if (killCount == 2)
        //{
        //    SpawnBoss();
        //}
    }

    public void OnInit()
    {
        LoadLevel(level);
        //player = LeanPool.Spawn(playerPrefab);
        //player.transform.position = currentLevel.spawnPoint.position;
        player = Instantiate(playerPrefab);
        player.transform.position = currentLevel.spawnPoint.position;

        // Cache NavMesh triangulation data
        //navMeshData = NavMesh.CalculateTriangulation();
        player.healthBar = this.healthBar;
        player.experienceBar = this.experienceBar;
        player.experienceBar.player = this.player;


        camera.target = player.transform;
        player.Camera = camera.GetComponent<Camera>();
        player.OnInit();

        SpawnBoss();
    }
    
    private void LoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevel = Instantiate(levelList[level - 1]);
        navMeshData = NavMesh.CalculateTriangulation();
        spawnEnemiesCoroutine = StartCoroutine(SpawnEnemiesRoutine());
        playerProgressCoroutine = StartCoroutine(CalculatePlayerProgressRoutine());
        CalculateEligibleEnemies();

        //StartCoroutine(BakeNavMeshCoroutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        currentSpawnInterval = initialSpawnInterval;

        while (true)
        {
            if (GameManager.Instance.IsState(GameState.Gameplay)/* && killCount < 10*/)
            {
                yield return new WaitForSeconds(currentSpawnInterval);
                SpawnEnemies();
            }
            else
            {
                // Wait for a short duration before checking the game state again
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void SpawnEnemies()
    {
        // Randomly select an enemy to spawn
        EnemyData enemyToSpawn = eligibleEnemies[Random.Range(0, eligibleEnemies.Count)];

        // Get a random point on the NavMesh
        Vector3 spawnPoint = GetRandomNavMeshPoint();

        // Spawn the selected enemy at the spawn point
        if (spawnPoint != Vector3.zero)
        {
            Enemy newEnemy = LeanPool.Spawn(enemyToSpawn.enemy, spawnPoint, Quaternion.identity);
            if (newEnemy != null)
            {
                newEnemy.target = player;
                newEnemy.OnInit(enemyToSpawn);
                //Debug.Log("Spawned Enemy: " + enemyToSpawn.enemyType);
            }
            else
            {
                Debug.LogWarning("Enemy GameObject is missing Enemy script.");
            }
        }
        else
        {
            Debug.LogWarning("Failed to find a valid spawn point on the NavMesh for the enemy.");
        }
    }

    private Vector3 GetRandomNavMeshPoint()
    {
        int randomIndex = Random.Range(0, navMeshData.indices.Length / 3);
        Vector3 point = (navMeshData.vertices[navMeshData.indices[randomIndex * 3]] +
                         navMeshData.vertices[navMeshData.indices[randomIndex * 3 + 1]] +
                         navMeshData.vertices[navMeshData.indices[randomIndex * 3 + 2]]) / 3;

        return point;
    }

    private IEnumerator CalculatePlayerProgressRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            CalculatePlayerProgress();
        }
    }

    private void ReduceSpawnInterval()
    {
        float newSpawnInterval = initialSpawnInterval - spawnIntervalReduction * killCount;
        currentSpawnInterval = Mathf.Max(newSpawnInterval, minSpawnInterval);
    }

    private void CalculatePlayerProgress()
    {
        if (killCount % 20 == 0)
        {
            enemyLevel++;
            CalculateEligibleEnemies();
            ReduceSpawnInterval();
        }
    }

    private void SpawnBoss()
    {
        // Randomly select an enemy to spawn
        //BossData bossToSpawn = bossDataSO.bossDataList[Random.Range(0, bossDataSO.bossDataList.Count)];
        BossData bossToSpawn = bossDataSO.bossDataList[0];


        // Get a random point on the NavMesh
        //Vector3 spawnPoint = GetRandomNavMeshPoint();
        Vector3 spawnPoint = currentLevel.bossSpawnPoint.transform.position;

        // Spawn the selected enemy at the spawn point
        if (spawnPoint != Vector3.zero)
        {
            Boss newBoss = LeanPool.Spawn(bossToSpawn.boss, spawnPoint, Quaternion.identity);
            if (newBoss != null)
            {
                newBoss.target = player;
                newBoss.OnInit(bossToSpawn);

                bossSpawned = true;
                Debug.Log("Boss Spawned");
                //Debug.Log("Spawned Enemy: " + enemyToSpawn.enemyType);
            }
            else
            {
                Debug.LogWarning("Enemy GameObject is missing Enemy script.");
            }
        }
        else
        {
            Debug.LogWarning("Failed to find a valid spawn point on the NavMesh for the enemy.");
        }
    }

    public void LoadNextLevel()
    {
        GameManager.Instance.ChangeState(GameState.ChangingLevel);
        UIManager.Instance.LoadingNextLevel();

        // Destroy the current level
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (spawnEnemiesCoroutine != null)
        {
            StopCoroutine(spawnEnemiesCoroutine);
        }
        if (playerProgressCoroutine != null)
        {
            StopCoroutine(playerProgressCoroutine);
        }

        LeanPool.DespawnAll();


        // Instantiate the next level
        LoadLevel(level);

        /*navMeshData = NavMesh.CalculateTriangulation();*/ //calculate triangulation lai de spawn o tren navmesh cua map moi, khong thi se van spawn tren navmesh cua level cu~

        //LeanPool.Spawn(playerPrefab);
        // Move the player to the spawn point

        // Set player's health and experience
        player.health = player.currentMaxHealth;
        player.playerExperience.currentExp = 0;
        //player.playerExperience.ResetLevel();

        // Update UI
        healthBar.UpdateHealthBar(player.characterData.health, playerPrefab.health);
        experienceBar.UpdateUI();

        // Set the player's position to the spawn point of the new level
        player.transform.position = currentLevel.spawnPoint.position;

        // Reset player's kill count
        killCount = 1;

        //spawnEnemiesCoroutine = StartCoroutine(SpawnEnemiesRoutine());
        //playerProgressCoroutine = StartCoroutine(CalculatePlayerProgressRoutine());

        nextLevelOptionCheck = false;
        GameManager.Instance.ChangeState(GameState.Gameplay);
        Debug.Log("Next level loaded: " + level);
    }

    private void CalculateEligibleEnemies()
    {
        //eligibleEnemies.Clear();
        //foreach (EnemyData enemyData in enemyDataSO.enemyDataList)
        //{
        //    if (enemyData.enemyLevel <= enemyLevel)
        //    {
        //        eligibleEnemies.Add(enemyData);
        //    }
        //}

        for (int i = 0; i < enemyDataSO.enemyDataList.Count; i++)
        {
            if (!eligibleEnemies.Contains(enemyDataSO.enemyDataList[i]) && enemyDataSO.enemyDataList[i].enemyLevel <= enemyLevel)
            {
                eligibleEnemies.Add(enemyDataSO.enemyDataList[i]);
            }
        }
    }

    public void FinishGameCalculations()
    {
        ResetGameProgress();
    }

    public void ResetGameProgress()
    {
        //LeanPool.DespawnAll();
        //Destroy(player.gameObject);
        
        //level = 1;
        //killCount = 1;
        //enemyLevel = 1;
    }

    public void RestartGame()
    {
        killCount = 1;
        UIManager.Instance.totalKillText.SetText(killCount.ToString());

        // Despawn all
        LeanPool.DespawnAll();

        //InitializePlayerSkills();

        enemyLevel = minEnemyLevel;
        currentSpawnInterval = initialSpawnInterval;

        eligibleEnemies.Clear();

        CalculateEligibleEnemies();

        StopAllCoroutines();

        //// Restart spawning enemies routine
        //StartCoroutine(SpawnEnemiesRoutine());

        //// Restart player progress calculation routine
        //StartCoroutine(CalculatePlayerProgressRoutine());

        spawnEnemiesCoroutine = StartCoroutine(SpawnEnemiesRoutine());
        playerProgressCoroutine = StartCoroutine(CalculatePlayerProgressRoutine());

        RespawnPlayer();

        //UIManager.Instance.EnterMatch();
    }


    private void RespawnPlayer()
    {
        if (currentLevel.spawnPoint.position != null)
        {
            //Destroy(player.gameObject);
            //LeanPool.Spawn(playerPrefab);
            player = Instantiate(playerPrefab);
            player.transform.position = currentLevel.spawnPoint.position;
            player.healthBar = this.healthBar;
            player.experienceBar = this.experienceBar;
            player.experienceBar.player = this.player;
            camera.target = player.transform;
            player.Camera = camera.GetComponent<Camera>();

            // Move the player to the spawn point

            // Reset player's health and experience
            //player.health = player.characterData.health;
            player.OnInit();
            player.playerExperience.ResetLevel();
            //InitializePlayerSkills();

            UIManager.Instance.killCountText.text = killCount.ToString();
            UIManager.Instance.currentLevelText.text = "LEVEL " + player.playerExperience.level.ToString();
            UIManager.Instance.finishGameUI.SetActive(false);
            GameManager.Instance.ChangeState(GameState.Gameplay);
            // Update UI elements
            //healthBar.UpdateHealthBar(player.characterData.health, playerPrefab.health);
            //experienceBar.UpdateUI();
        }
        else
        {
            Debug.LogWarning("Player spawn point is not assigned.");
        }
    }

    //private IEnumerator BakeNavMeshCoroutine()
    //{
    //    // Wait for a short delay to ensure level objects are instantiated
    //    yield return new WaitForSeconds(0.5f);
    //    BakeNavMesh(currentLevel);
    //}

    //private void BakeNavMesh(Level level)
    //{
    //    NavMeshSurface navMeshSurface = level.GetComponent<NavMeshSurface>();
    //    if (navMeshSurface != null)
    //    {
    //        navMeshSurface.BuildNavMesh();
    //    }
    //    else
    //    {
    //        Debug.LogWarning("NavMeshSurface not found in the current level.");
    //    }
    //}

    public void EndGame()
    {
        level = 1;
        enemyLevel = minEnemyLevel;
        CalculateEligibleEnemies();
        LeanPool.DespawnAll();
        StopAllCoroutines();
        Destroy(currentLevel.gameObject);
        if(player != null)
        {
            Destroy(player.gameObject);
        }
    }

    public void InitializePlayerSkills()
    {
        player.playerSkills.ResetSkill();
        player.playerSkills.AddSkill(player.characterData.basicSkill);
    }
}



