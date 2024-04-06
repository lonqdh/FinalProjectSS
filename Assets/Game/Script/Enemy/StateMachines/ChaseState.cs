using UnityEngine;

public class ChaseState : IState<Enemy>
{

    public void OnEnter(Enemy enemy)
    {
        // Start chasing the target
        if (enemy.target.isAlive == true)
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
            else
            {
                // Continue chasing the target
                enemy.agent.SetDestination(enemy.target.transform.position);
                enemy.ChangeAnim("IsRun");
            }
        }
        // Check if the target is within attack range

    }

    public void OnExit(Enemy enemy)
    {
        //enemy.agent.isStopped = true;
    }
}
