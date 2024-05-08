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
            LevelManager.Instance.nextLevelOptionCheck = false;
            UIManager.Instance.FinishMatch();
        }

    }
}
