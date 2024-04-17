using System.Collections;
using UnityEngine;

public class BossAttackState : IState<Boss>
{
    private bool canAttack = true;
    private bool isCasting = false; // Flag to track if casting coroutine is running

    public void OnEnter(Boss boss)
    {
        // Start attacking
        boss.agent.isStopped = true;
        boss.ChangeAnim("IsIdle");
    }

    public void OnExecute(Boss boss)
    {
        if (boss.isAlive)
        {
            if (canAttack && boss.target != null && !isCasting) //!isCasting used to Check if not already casting to prevent unlimited skill casting
            {
                // Check if the boss is fully stopped
                if (boss.agent.velocity.magnitude <= 0.1f)
                {
                    //// Check if the target is within attack range
                    //if (IsTargetWithinAttackRange(boss) && boss.bossData.bossSkills.Length > 0)
                    //{
                    //    // Cast each skill in the bossSkills array
                    //    foreach (var skill in boss.bossData.bossSkills)
                    //    {
                    //        boss.StartCoroutine(CastSkillWithDelay(skill, boss));
                    //    }
                    //}
                    if (IsTargetWithinAttackRange(boss) && boss.bossData.bossSkills.Length > 0)
                    {
                        // Randomly select a skill and its activation type
                        int randomIndex = Random.Range(0, boss.bossData.bossSkills.Length);
                        var skill = boss.bossData.bossSkills[randomIndex];
                        bool useBossActivate2 = Random.value < 0.5f;

                        // Cast the selected skill with the chosen activation type
                        boss.StartCoroutine(CastSkillWithDelay(skill, boss, useBossActivate2));
                    }
                    else
                    {
                        // Target is out of attack range, switch back to ChaseState
                        boss.ChangeState(new BossChaseState());
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

    //private IEnumerator CastSkillWithDelay(SkillData skillData, Boss boss)
    //{
    //    isCasting = true;
    //    boss.ChangeAnim("IsAttack");
    //    yield return new WaitForSeconds(skillData.castTime);

    //    if (boss.isAlive)
    //    {
    //        if (IsTargetWithinAttackRange(boss))
    //        {
    //            // Cast the boss skill at the target's position
    //            skillData.BossActivate2(boss.target.transform.position, boss.chargeSkillPos, boss);
    //            yield return boss.StartCoroutine(StartAttackCooldown(skillData.cooldown, boss)); // can phai co thang yield return o day, no co tac dung la bat buoc coroutine nay phai doi den khi coroutine startattackcooldown xong het moi tiep tuc execute ( de tranh bot spam cast skill, giup cooldown cho skill hoat dong )
    //        }
    //        else
    //        {
    //            // Target moved out of attack range during casting, switch back to ChaseState
    //            boss.ChangeState(new BossChaseState());
    //        }
    //        boss.ChangeAnim("IsIdle");
    //        isCasting = false;
    //    }
    //}

    private IEnumerator CastSkillWithDelay(SkillData skillData, Boss boss, bool useBossActivate2)
    {
        isCasting = true;
        //boss.ChangeAnim("IsAttack");
        if (useBossActivate2 == true)
        {
            boss.ChangeAnim("IsBossSpellAttack");
        }
        else
        {
            boss.ChangeAnim("IsBossSpellAttack2");
        }

        yield return new WaitForSeconds(skillData.castTime);

        if (boss.isAlive)
        {
            if (IsTargetWithinAttackRange(boss))
            {
                // Cast the boss skill with the chosen activation type
                if (useBossActivate2)
                {
                    skillData.BossActivate2(boss.target.transform.position, boss.chargeSkillPos, boss);
                }
                else
                {
                    skillData.BossActivate(boss.target.transform.position, boss.chargeSkillPos, boss);
                }

                yield return boss.StartCoroutine(StartAttackCooldown(skillData.cooldown, boss)); // can phai co thang yield return o day, no co tac dung la bat buoc coroutine nay phai doi den khi coroutine startattackcooldown xong het moi tiep tuc execute ( de tranh bot spam cast skill, giup cooldown cho skill hoat dong )
            }
            else
            {
                // Target moved out of attack range during casting, switch back to ChaseState
                boss.ChangeState(new BossChaseState());
            }
            //boss.ChangeAnim("IsIdle");
            isCasting = false;
        }
    }

    public void OnExit(Boss boss)
    {
        // Clean up any necessary resources or variables
    }

    private bool IsTargetWithinAttackRange(Boss boss)
    {
        if (boss.target == null)
            return false;

        float distance = Vector3.Distance(boss.transform.position, boss.target.transform.position);
        return distance <= boss.bossData.attackRange;
    }

    private IEnumerator StartAttackCooldown(float cooldown, Boss boss)
    {
        canAttack = false;
        boss.ChangeAnim("IsIdle");
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

}
