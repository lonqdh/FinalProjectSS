using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public SkillDatabase skillDatabase;
    public Player player;
    public EnemyDataSO enemyDataSO;
    public int killCount;

    public float spawnIntervalReduction = 0.1f;
    public float initialSpawnInterval = 5f;
    public float minSpawnInterval = 1f;
    [SerializeField] private float currentSpawnInterval;
    public int minEnemyLevel = 1;
    public int maxEnemyLevel = 5;
    [SerializeField] private List<EnemyData> eligibleEnemies;
    private int enemyLevel = 0;

    private NavMeshTriangulation navMeshData; // for spawning enemies

    private void Start()
    {
        killCount = 0;
        StartCoroutine(CalculatePlayerProgressRoutine());
        StartCoroutine(SpawnEnemiesRoutine());
        CalculateEligibleEnemies();

        // Cache NavMesh triangulation data
        navMeshData = NavMesh.CalculateTriangulation();
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        currentSpawnInterval = initialSpawnInterval;

        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);
            SpawnEnemies();
            ReduceSpawnInterval();
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
                Debug.Log("Spawned Enemy: " + enemyToSpawn.enemyType);
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
            yield return new WaitForSeconds(10f); // Adjust the interval as needed
            CalculatePlayerProgress();
        }
    }

    private void ReduceSpawnInterval()
    {
        if (killCount % 100 == 0)
        {
            float newSpawnInterval = initialSpawnInterval - spawnIntervalReduction * killCount;
            currentSpawnInterval = Mathf.Max(newSpawnInterval, minSpawnInterval);
        }
    }

    private void CalculatePlayerProgress()
    {
        if (killCount % 100 == 0)
        {
            enemyLevel++;
            CalculateEligibleEnemies();
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

    private void Update()
    {
        
    }

    public void InitializePlayerSkills()
    {
        player.playerSkills.currentSkills.Clear();
        player.playerSkills.AddSkill(player.characterData.basicSkill);
    }
}



