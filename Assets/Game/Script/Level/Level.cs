using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform bossSpawnPoint;
    public int killRequiredToNextLevel;
    public EndGamePortal endGamePortal;
    public NextLevelPortal nextLevelPortal;
    [SerializeField] private Transform endPortalPos;
    [SerializeField] private Transform nextLevelPortalPos;


    public void SpawnPortals()
    {
        LevelManager.Instance.nextLevelOptionCheck = true;
        NextLevelPortal nextLevel = LeanPool.Spawn(nextLevelPortal, nextLevelPortalPos.transform.position, Quaternion.identity);
        EndGamePortal endLevel = LeanPool.Spawn(endGamePortal, endPortalPos.transform.position, Quaternion.identity);
    }
}
