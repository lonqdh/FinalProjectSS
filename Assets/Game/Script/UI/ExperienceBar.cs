using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    //[SerializeField] private Slider experienceSlider;
    public Player player;

    //public void UpdateUI()
    //{
    //    experienceSlider.value = (float)player.playerExperience.currentExp / player.playerExperience.expToLevelUp;
    //}

    [SerializeField] private Image experienceSprite;
    [SerializeField] private float reduceSpeed = 1;
    private float target = 1;
    //private Camera cam;

    private void Start()
    {
        //cam = Camera.main;
    }

    private void Update()
    {
        //transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        experienceSprite.fillAmount = Mathf.MoveTowards(experienceSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
    }

    public void UpdateUI()
    {
        target = (float)player.playerExperience.currentExp / player.playerExperience.expToLevelUp;
        //healthbarSprite.fillAmount = currentHealth / maxHealth;
    }
}
