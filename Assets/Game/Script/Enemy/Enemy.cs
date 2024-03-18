using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : Character
{
    [SerializeField] internal EnemyData enemyData;
    private IState<Enemy> currentState;
    [SerializeField] private Player target;
    [SerializeField] private float enemyAttackRange;
    public NavMeshAgent agent;
    public bool isAttacking;


    // Start is called before the first frame update
    void Start()
    {
        target = LevelManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDespawn()
    {
        LevelManager.Instance.killCount++;
    }

    //Called where the SpawnEnemies method is called so once
    //spawned the enemy will call the OnInit method
    public void OnInit(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        this.enemyData.enemyType = enemyData.enemyType;
        this.enemyData.damage = enemyData.damage;
        this.enemyData.health = enemyData.health;
        //this.enemyData.enemyModelPrefab = enemyData.enemyModelPrefab;
        this.enemyData.movementSpeed = enemyData.movementSpeed;
        this.enemyData.enemySkill = enemyData.enemySkill;

        ChangeState(new ChaseState());
    }

    //protected override void Attack(Transform target)
    //{
    //    base.Attack(target);
    //    StartCoroutine(ResumePatrolling());
    //}

    //private IEnumerator ResumePatrolling()
    //{
    //    yield return new WaitForSeconds(2f);

    //    isAttacking = false;
    //    if (!IsDead)
    //    {
    //        agent.isStopped = false;
    //        ChangeState(new PatrolState());
    //    }
    //}

    public void ChangeState(IState<Enemy> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
}
