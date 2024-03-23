using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AttackState : IState<Enemy>
{
    private bool canAttack = true;
    private bool isCasting = false; // Flag to track if casting coroutine is running

    public void OnEnter(Enemy enemy)
    {
        // Start attacking behavior here
        enemy.agent.isStopped = true;
    }

    public void OnExecute(Enemy enemy)
    {
        // Execute attacking behavior here
        if (canAttack && enemy.target != null && !isCasting) // Check if not already casting
        {
            // Check if the enemy is fully stopped
            if (enemy.agent.velocity.magnitude <= 0.1f)
            {
                // Check if the target is within attack range
                if (IsTargetWithinAttackRange(enemy))
                {
                    // Use the enemy skill to attack the target
                    if (enemy.enemyData.enemySkill != null)
                    {
                        // Start the casting process if not already casting
                        enemy.StartCoroutine(CastSkillWithDelay(enemy.enemyData.enemySkill, enemy));
                    }
                }
                else
                {
                    // Target is out of attack range, switch back to ChaseState
                    enemy.ChangeState(new ChaseState());
                }
            }
            else
            {
                return;
            }
        }
    }

    private IEnumerator CastSkillWithDelay(SkillData skillData, Enemy enemy)
    {
        isCasting = true; // Set flag to indicate casting
        // Wait for the casting time before activating the skill
        yield return new WaitForSeconds(skillData.castTime);

        // Cast the enemy skill at the target's position
        enemy.enemyData.enemySkill.Activate(enemy.target.transform.position, enemy.chargeSkillPos, enemy);
        

        // Start the attack cooldown
        enemy.StartCoroutine(StartAttackCooldown(enemy.enemyData.enemySkill.cooldown));

        isCasting = false; // Reset flag after casting is done
    }

    public void OnExit(Enemy enemy)
    {
        // Exit attacking behavior here
    }

    private bool IsTargetWithinAttackRange(Enemy enemy)
    {
        if (enemy.target == null)
            return false;

        float distance = Vector3.Distance(enemy.transform.position, enemy.target.transform.position);
        return distance <= enemy.enemyData.attackRange;
    }

    private IEnumerator StartAttackCooldown(float cooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}

