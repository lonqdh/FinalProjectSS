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
    public CameraFollow camera;
    public int killCount;

    public float spawnIntervalReduction = 0.1f;
    public float initialSpawnInterval = 5f;
    public float minSpawnInterval = 1f;
    [SerializeField] private float currentSpawnInterval;
    public int minEnemyLevel = 1;
    public int maxEnemyLevel = 5;
    [SerializeField] private List<EnemyData> eligibleEnemies;
    [SerializeField] private int enemyLevel = 1;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private ExperienceBar experienceBar;
    public PlayerSkills currentSkillsList;
    [SerializeField] private Transform playerSpawnPosition;

    private NavMeshTriangulation navMeshData; // for spawning enemies

    private void Start()
    {
        player = LeanPool.Spawn(playerPrefab);
        player.transform.position = playerSpawnPosition.position;
        
        StartCoroutine(CalculatePlayerProgressRoutine());
        StartCoroutine(SpawnEnemiesRoutine());
        CalculateEligibleEnemies();

        // Cache NavMesh triangulation data
        navMeshData = NavMesh.CalculateTriangulation();
        player.healthBar = this.healthBar;
        player.experienceBar = this.experienceBar;
        player.experienceBar.player = this.player;
        camera.target = player.transform;
        player.Camera = camera.GetComponent<Camera>();
        //player.OnInit();

    }

    private void Update()
    {

    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        currentSpawnInterval = initialSpawnInterval;

        while (true)
        {
            if (GameManager.Instance.IsState(GameState.Gameplay))
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
            yield return new WaitForSeconds(10f);
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
        if (killCount % 10 == 0)
        {
            enemyLevel++;
            CalculateEligibleEnemies();
            ReduceSpawnInterval();
        }
    }

    private void CalculateEligibleEnemies()
    {
        eligibleEnemies.Clear();
        foreach (EnemyData enemyData in enemyDataSO.enemyDataList)
        {
            if (enemyData.enemyLevel <= enemyLevel)
            {
                eligibleEnemies.Add(enemyData);
            }
        }
    }

    public void FinishGameCalculations()
    {
        UIManager.Instance.totalKillText.SetText(killCount.ToString());
        UIManager.Instance.goldEarnedText.SetText((killCount * 10).ToString());
    }

    public void RestartGame()
    {
        killCount = 1;
        UIManager.Instance.totalKillText.SetText(killCount.ToString());
        
        // Despawn all
        LeanPool.DespawnAll();

        InitializePlayerSkills();

        enemyLevel = minEnemyLevel;
        currentSpawnInterval = initialSpawnInterval;

        CalculateEligibleEnemies();

        StopAllCoroutines();

        // Restart spawning enemies routine
        StartCoroutine(SpawnEnemiesRoutine());

        // Restart player progress calculation routine
        StartCoroutine(CalculatePlayerProgressRoutine());

        RespawnPlayer();

        UIManager.Instance.EnterMatch();
    }


    private void RespawnPlayer()
    {
        if (playerSpawnPosition != null)
        {
            LeanPool.Spawn(playerPrefab);
            // Move the player to the spawn point
            playerPrefab.transform.position = playerSpawnPosition.position;

            // Reset player's health and experience
            player.health = player.characterData.health;
            player.playerExperience.ResetLevel();

            // Update UI elements
            healthBar.UpdateHealthBar(player.characterData.health, playerPrefab.health);
            experienceBar.UpdateUI();
        }
        else
        {
            Debug.LogWarning("Player spawn point is not assigned.");
        }
    }


    public void InitializePlayerSkills()
    {
        player.playerSkills.currentSkills.Clear();
        player.playerSkills.AddSkill(player.characterData.basicSkill);
    }
}



