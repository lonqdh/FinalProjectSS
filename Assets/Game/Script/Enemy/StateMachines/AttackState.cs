using System.Collections;
using UnityEngine;

public class AttackState : IState<Enemy>
{
    private bool canAttack = true;
    private bool isCasting = false; // Flag to track if casting coroutine is running
    //private Coroutine castingCoroutine;

    public void OnEnter(Enemy enemy)
    {
        // Start attacking
        enemy.agent.isStopped = true;
        enemy.ChangeAnim("IsIdle");
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.isAlive)
        {
            if (canAttack && enemy.target != null && !isCasting) //!isCasting used to Check if not already casting to prevent unlimited skill casting
            {
                // Check if the enemy is fully stopped
                if (enemy.agent.velocity.magnitude <= 0.1f)
                {
                    // Check if the target is within attack range
                    if (IsTargetWithinAttackRange(enemy) && enemy.enemyData.enemySkill != null)
                    {
                        enemy.StartCoroutine(CastSkillWithDelay(enemy.enemyData.enemySkill, enemy));
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
        // Execute attacking
    }

    private IEnumerator CastSkillWithDelay(SkillData skillData, Enemy enemy)
    {
        isCasting = true;
        enemy.ChangeAnim("IsAttack");
        yield return new WaitForSeconds(skillData.castTime);

        if (enemy.isAlive)
        {
            if (IsTargetWithinAttackRange(enemy))
            {
                // Cast the enemy skill at the target's position

                enemy.enemyData.enemySkill.Activate(enemy.target.transform.position, enemy.chargeSkillPos, enemy);
                yield return enemy.StartCoroutine(StartAttackCooldown(enemy.enemyData.enemySkill.cooldown, enemy)); // can phai co thang yield return o day, no co tac dung la bat buoc coroutine nay phai doi den khi coroutine startattackcooldown xong het moi tiep tuc execute ( de tranh bot spam cast skill, giup cooldown cho skill hoat dong )
            }
            else
            {
                // Target moved out of attack range during casting, switch back to ChaseState
                enemy.ChangeState(new ChaseState());
            }
            //enemy.ChangeState(new ChaseState());
            enemy.ChangeAnim("IsIdle");
            isCasting = false;
        }
    }

    public void OnExit(Enemy enemy)
    {
        //if (castingCoroutine != null)
        //{
        //    enemy.StopCoroutine(castingCoroutine);
        //    isCasting = false;
        //    castingCoroutine = null;
        //}
    }

    private bool IsTargetWithinAttackRange(Enemy enemy)
    {
        if (enemy.target == null)
            return false;

        float distance = Vector3.Distance(enemy.transform.position, enemy.target.transform.position);
        return distance <= enemy.enemyData.attackRange;
    }

    private IEnumerator StartAttackCooldown(float cooldown, Enemy enemy)
    {
        canAttack = false;
        enemy.ChangeAnim("IsIdle");
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}

