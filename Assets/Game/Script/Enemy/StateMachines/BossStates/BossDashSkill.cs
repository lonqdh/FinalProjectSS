using UnityEngine;

public class BossDashSkill : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    public float dashCooldown;
    private float dashCooldownTimer;

    public Boss boss;
    private Rigidbody rb;
    private bool isDashing;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Example: Trigger the dash when a condition is met (e.g., player in sight)
        if (CanDash() && dashCooldownTimer <= 0)
        {
            Dash();
            dashCooldownTimer = dashCooldown;
        }
    }

    private bool CanDash()
    {
        // Implement conditions to check if the enemy can dash (e.g., player in sight)
        if (Vector3.Distance(boss.transform.position, boss.target.transform.position) >= boss.bossData.attackRange)
        {
            return true;
        }

        return false;
    }

    private void Dash()
    {
        isDashing = true;

        // Calculate dash direction (example: towards the player)
        Vector3 dashDirection = (boss.target.transform.position - transform.position).normalized;

        // Apply dash force
        Vector3 dashForceToApply = dashDirection * dashForce + Vector3.up * dashUpwardForce;
        rb.velocity = Vector3.zero; // Reset velocity
        rb.AddForce(dashForceToApply, ForceMode.Impulse);
        Debug.Log("Dash Activated");
        
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        isDashing = false;
    }
}

