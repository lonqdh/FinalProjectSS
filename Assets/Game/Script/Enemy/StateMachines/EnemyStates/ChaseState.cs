using System.Collections;
using UnityEngine;

public class ChaseState : IState<Enemy>
{
    private Coroutine speedUpCoroutine;
    private float sprintDuration = 3f; // Duration for sprinting
    private float sprintCooldown = 5f; // Cooldown between sprints
    private float sprintChance = 0.3f; // Chance of sprinting
    private bool isSprinting = false; // Flag to track if currently sprinting
    private float nextSprintTime;

    public void OnEnter(Enemy enemy)
    {
        // Start chasing the target
        if (enemy.target.isAlive)
        {
            enemy.agent.isStopped = false;
            enemy.ChangeAnim("IsIdle");
            enemy.agent.SetDestination(enemy.target.transform.position);
        }
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.isAlive)
        {
            if (Vector3.Distance(enemy.transform.position, enemy.target.transform.position) <= enemy.enemyData.attackRange)
            {
                // Change state to AttackState
                enemy.ChangeState(new AttackState());
            }
            else if (Vector3.Distance(enemy.transform.position, enemy.target.transform.position) <= enemy.enemyData.attackRange && enemy.hasSprayingSkill == true)
            {
                enemy.ChangeState(new SprayAttackState());
            }
            else
            {
                // Check if not currently sprinting, if cooldown has passed, and random chance to sprint
                if (!isSprinting && Time.time >= nextSprintTime && Random.value <= sprintChance)
                {
                    // Start sprinting
                    StartSprint(enemy);
                }
                else
                {
                    // Continue regular movement
                    enemy.agent.SetDestination(enemy.target.transform.position);
                    if (isSprinting == true)
                    {
                        enemy.ChangeAnim("IsSprint");
                    }
                    else
                    {
                        enemy.ChangeAnim("IsRun");
                    }
                }
            }
        }
    }

    public void OnExit(Enemy enemy)
    {
        // Stop sprinting if still active
        if (isSprinting && speedUpCoroutine != null)
        {
            enemy.StopCoroutine(speedUpCoroutine);
            isSprinting = false;
        }
    }

    private void StartSprint(Enemy enemy)
    {
        // Start sprinting
        //enemy.ChangeAnim("IsSprint");
        float originalSpeed = enemy.agent.speed; // Store the original speed
        enemy.agent.speed += 0.15f; // Speed up the agent

        // Start the sprint duration countdown
        speedUpCoroutine = enemy.StartCoroutine(SprintDurationCountdown(originalSpeed, sprintDuration, enemy));

        // Set the sprint cooldown timer
        nextSprintTime = Time.time + sprintCooldown;

        // Set sprinting flag
        isSprinting = true;
    }

    private IEnumerator SprintDurationCountdown(float originalSpeed, float duration, Enemy enemy)
    {
        // Wait for the sprint duration
        yield return new WaitForSeconds(duration);

        // Restore the original speed
        enemy.agent.speed = originalSpeed;

        // Clear the sprinting flag
        isSprinting = false;
    }
}
