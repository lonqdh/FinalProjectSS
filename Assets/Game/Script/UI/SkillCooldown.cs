using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{
    public Image skillIcon;
    public SkillData skill;
    private bool isCoolingDown = false;
    public float cooldownTime; // Adjust this value as needed
    public float cooldownTimer = 0;


    void Update()
    {
        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                // Cooldown finished
                isCoolingDown = false;
                skillIcon.fillAmount = 1f; // Reset fill amount to full
            }
            else
            {
                skillIcon.fillAmount = cooldownTimer / skill.cooldown;
            }
        }
    }

    // Call this method to start the cooldown
    public void StartCooldown()
    {
        cooldownTimer = skill.cooldown;
        isCoolingDown = true;
    }
}
