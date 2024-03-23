using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ChaseState : IState<Enemy>
{

    public void OnEnter(Enemy enemy)
    {
        // Start chasing the target
        enemy.agent.isStopped = false;
        enemy.agent.SetDestination(enemy.target.transform.position);
    }

    public void OnExecute(Enemy enemy)
    {
        // Check if the target is within attack range
        if (Vector3.Distance(enemy.transform.position, enemy.target.transform.position) <= enemy.enemyData.attackRange)
        {
            // Change state to AttackState
            enemy.ChangeState(new AttackState());
        }
        else
        {
            // Continue chasing the target
            enemy.agent.SetDestination(enemy.target.transform.position);
        }
    }

    public void OnExit(Enemy enemy)
    {
        //enemy.agent.isStopped = true;
    }
}
