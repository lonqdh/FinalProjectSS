using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;
    [SerializeField] private float fillSpeed = 1f;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;

    private float targetFillAmount = 1f;
    private Color targetColor;

    private void Update()
    {
        // Smoothly transition the fill amount
        healthbarSprite.fillAmount = Mathf.MoveTowards(healthbarSprite.fillAmount, targetFillAmount, fillSpeed * Time.deltaTime);

        // Smoothly transition the color
        healthbarSprite.color = Color.Lerp(healthbarSprite.color, targetColor, fillSpeed * Time.deltaTime);
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        targetFillAmount = Mathf.Clamp01(currentHealth / maxHealth);

        // Set the target color based on health percentage
        float healthPercentage = currentHealth / maxHealth;
        targetColor = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
    }
}
