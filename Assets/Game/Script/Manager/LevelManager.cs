using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public SkillDatabase skillDatabase;
    public PlayerSkills playerSkills;
    public Player player;
    [NonSerialized] public List<SkillData> currentSkills;
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

    //public int maxEnemies = 50;
    //private List<Enemy> activeEnemies = new List<Enemy>();

    public Transform[] spawnPoints; // Array of available spawn points
    private int currentSpawnPointIndex = 0; // Index of the current spawn point

    private void Start()
    {
        killCount = 0;
        StartCoroutine(CalculatePlayerProgressRoutine());
        StartCoroutine(SpawnEnemiesRoutine());
        CalculateEligibleEnemies();
    }

    private void Update()
    {
        currentSkills = playerSkills.skills;
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

        // Get the current spawn point
        Transform spawnPoint = GetNextSpawnPoint();

        // Spawn the selected enemy at the spawn point
        if (spawnPoint != null)
        {
            Enemy newEnemy = LeanPool.Spawn(enemyToSpawn.enemy, spawnPoint.position, Quaternion.identity);
            if (newEnemy != null)
            {
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
            Debug.LogWarning("Failed to find a valid spawn point for the enemy.");
        }
    }

    private Transform GetNextSpawnPoint()
    {
        // Ensure there are available spawn points
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return null;
        }

        // Get the current spawn point and increment the index
        Transform spawnPoint = spawnPoints[currentSpawnPointIndex];
        currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnPoints.Length;

        return spawnPoint;
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
        Debug.Log("CalculatedPlayerProgress");
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



    public void InitializePlayerSkills()
    {
        playerSkills.skills.Clear();
        playerSkills.AddSkill(player.characterData.basicSkill);

    }
}


//private Vector3 GetRandomSpawnPoint()
//{
//    // Define the spawn area within the playable bounds of the map
//    Bounds spawnBounds = new Bounds(/* Define spawn area center */, /* Define spawn area size */);

//    Vector3 spawnPoint = Vector3.zero;
//    int maxAttempts = 10;
//    int attempts = 0;

//    // Try finding a valid spawn point within the spawn area
//    do
//    {
//        // Generate a random spawn point within the defined spawn area
//        spawnPoint = spawnBounds.center + new Vector3(
//            Random.Range(-spawnBounds.extents.x, spawnBounds.extents.x),
//            0,
//            Random.Range(-spawnBounds.extents.z, spawnBounds.extents.z)
//        );

//        // Perform a raycast to check if the spawn point is obstructed
//        RaycastHit hit;
//        if (Physics.Raycast(spawnPoint + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, /* Layer mask for obstacles */))
//        {
//            if (hit.collider.CompareTag(/* Tag of obstacles */))
//            {
//                // Spawn point is obstructed, try again
//                spawnPoint = Vector3.zero;
//            }
//        }

//        attempts++;
//    } while (spawnPoint == Vector3.zero && attempts < maxAttempts);

//    return spawnPoint;
//}

//private void SpawnEnemies()
//{
//    // Randomly select an enemy to spawn
//    EnemyData enemyToSpawn = eligibleEnemies[Random.Range(0, eligibleEnemies.Count)];

//    // Get a valid spawn point
//    //Vector3 spawnPosition = GetRandomSpawnPoint();

//    // Spawn the selected enemy at the valid spawn point
//    if (spawnPosition != Vector3.zero)
//    {
//        Enemy newEnemy = LeanPool.Spawn(enemyToSpawn.enemy, spawnPosition, Quaternion.identity);
//        if (newEnemy != null)
//        {
//            newEnemy.OnInit(enemyToSpawn);
//            Debug.Log("Spawned Enemy: " + enemyToSpawn.enemyType);
//        }
//        else
//        {
//            Debug.LogWarning("Enemy GameObject is missing Enemy script.");
//        }
//    }
//    else
//    {
//        Debug.LogWarning("Failed to find a valid spawn point for the enemy.");
//    }
//}