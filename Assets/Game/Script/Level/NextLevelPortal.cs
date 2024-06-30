using UnityEngine;

public class NextLevelPortal : MonoBehaviour
{
    public Collider collider;

    private void OnTriggerEnter(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();
        if (newPlayer != null)
        {
            if(LevelManager.Instance.enemyList.Count > 0)
            {
                Debug.Log("Slay all the remaining enemies first !");
                return;
            }
            //LevelManager.Instance.proceedToNextLevel = true;
            LevelManager.Instance.nextLevelOptionCheck = false;
            LevelManager.Instance.LoadNextLevel();
        }

    }
}
