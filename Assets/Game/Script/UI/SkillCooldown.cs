using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{
    public Image skillIcon;
    public SkillData skill;
    private bool isCoolingDown = false;
    private float cooldownTime;
    private float cooldownTimer = 0;

    private void Start()
    {
        // Subscribe to the SkillActivated event of the skill
        //skill.SkillActivated += OnSkillActivated;
        //skillIcon = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the SkillActivated event to avoid memory leaks
        //skill.SkillActivated -= OnSkillActivated;
    }

    private void Update()
    {
        if (isCoolingDown)
        {
            //Debug.Log("Dang update");
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                // Cooldown finished
                isCoolingDown = false;
                skillIcon.fillAmount = 1f; // Reset fill amount to full
                GetComponent<Image>().fillAmount = 1f;
            }
            else
            {
                skillIcon.fillAmount = cooldownTimer / cooldownTime;
                GetComponent<Image>().fillAmount = cooldownTimer / cooldownTime;
            }
        }
    }

    private void OnSkillActivated(SkillData activatedSkill)
    {
        // Check if the activated skill matches the skill associated with this UI
        if (activatedSkill == skill)
        {
            // Start the cooldown UI
            StartCooldown(activatedSkill.cooldown);
        }
    }

    // Call this method to start the cooldown
    public void StartCooldown(float cooldown)
    {
        cooldownTime = cooldown;
        cooldownTimer = cooldownTime;
        isCoolingDown = true;
        //Debug.Log("Check");
    }
}
