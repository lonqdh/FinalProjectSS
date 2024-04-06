using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;
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
        healthbarSprite.fillAmount = Mathf.MoveTowards(healthbarSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
        //healthbarSprite.fillAmount = currentHealth / maxHealth;
    }
}
