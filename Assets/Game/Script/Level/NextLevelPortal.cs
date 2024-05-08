using UnityEngine;

public class NextLevelPortal : MonoBehaviour
{
    public Collider collider;

    private void OnTriggerEnter(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();
        if (newPlayer != null)
        {
            //LevelManager.Instance.proceedToNextLevel = true;
            LevelManager.Instance.nextLevelOptionCheck = false;
            LevelManager.Instance.LoadNextLevel();
        }

    }
}
