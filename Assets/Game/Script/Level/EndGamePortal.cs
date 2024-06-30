using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePortal : MonoBehaviour
{
    public Collider collider;

    private void OnTriggerEnter(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();
        if (newPlayer != null)
        {
            if (LevelManager.Instance.enemyList.Count > 0)
            {
                Debug.Log("Slay all the remaining enemies before ending match !");
                return;
            }
            LevelManager.Instance.nextLevelOptionCheck = false;
            UIManager.Instance.FinishMatch();
        }

    }
}
