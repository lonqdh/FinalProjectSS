using System.Collections;
using UnityEngine;

public class SprayAttackState : IState<Enemy>
{
    private float moveSpeedMultiplier = 0.5f;
    private bool isCasting = false;

    public void OnEnter(Enemy enemy)
    {
        enemy.ChangeAnim("IsAttack");
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.isAlive)
        {
            enemy.skill.Activate(enemy.target.transform.position, enemy.chargeSkillPos, enemy);
            //enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation,
            //                Quaternion.LookRotation(enemy.target.transform.position - enemy.transform.position),
            //                3f);
            //enemy.movementSpeed *= moveSpeedMultiplier;
            //Exit(enemy);
        }
    }

    private IEnumerator Exit(Enemy enemy)
    {
        yield return new WaitForSeconds(2f);
        OnExit(enemy);
    }


    public void OnExit(Enemy enemy)
    {
        enemy.movementSpeed /= moveSpeedMultiplier;
        enemy.ChangeState(new ChaseState());
        // Cleanup code if needed
    }
}
