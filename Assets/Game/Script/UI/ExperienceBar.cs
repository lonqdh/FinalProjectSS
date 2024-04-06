using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private Slider experienceSlider;
    public Player player;

    public void UpdateUI()
    {
        experienceSlider.value = (float)player.playerExperience.currentExp / player.playerExperience.expToLevelUp;
    }
}
