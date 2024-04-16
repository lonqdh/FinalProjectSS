using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : IState<Boss>
{
    public void OnEnter(Boss boss)
    {
        if (boss.target.isAlive == true)
        {
            boss.agent.isStopped = false;
            boss.ChangeAnim("IsIdle");
            //boss.agent.SetDestination(boss.target.transform.position);

        }
    }

    public void OnExecute(Boss boss)
    {
        if (boss.isAlive)
        {
            // Calculate the direction to the target
            Vector3 moveDirection = (boss.target.transform.position - boss.transform.position).normalized;

            // Set the boss's velocity based on its movement speed
            boss.agent.velocity = moveDirection * boss.movementSpeed;

            if (Vector3.Distance(boss.transform.position, boss.target.transform.position) <= boss.bossData.attackRange)
            {
                // Change state to AttackState
                //boss.ChangeState(new AttackState());
            }
            else
            {
                boss.agent.SetDestination(boss.target.transform.position);
                boss.ChangeAnim("IsRun");
            }
        }
    }

    public void OnExit(Boss boss)
    {

    }
}
