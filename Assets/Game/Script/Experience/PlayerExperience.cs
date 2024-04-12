using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    //[SerializeField] private Player player;
    public int level = 1;
    public int currentExp = 0;
    public int expToLevelUp = 1;

    public void AddExp(int expAmount)
    {
        currentExp += expAmount;
        if (currentExp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        LevelManager.Instance.player.AddSkill();
        UIManager.Instance.nextLevelText.text = level.ToString();
        expToLevelUp = ExperienceForNextLevel();
        currentExp = 0;
    }

    private int ExperienceForNextLevel()
    {
        return expToLevelUp * level;
    }

    public void ResetLevel()
    {
        level = 1;
        currentExp = 0;
        expToLevelUp = 10;
    }
}
