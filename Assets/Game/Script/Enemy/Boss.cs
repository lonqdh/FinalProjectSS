using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Boss : Character
{
    [SerializeField] internal BossData bossData;
    private IState<Boss> currentState;
    public Player target;
    public NavMeshAgent agent;
    //public bool isAttacking;
    internal SkillData[] skill;
    //internal SkillData skill1;
    //internal SkillData skill2;
    //internal SkillData skill3;
    [SerializeField] private float rotateSpeed = 5f;
    //public ExperienceSphere blueExpSphere;
    //public ExperienceSphere redExpSphere;
    private float lessExpDropRate = 0.8f;
    public bool hasSprayingSkill;

    private BossDashSkill dashSkill;

    private void Start()
    {
        //dashSkill = new BossDashSkill(this);
    }

    //public void ActivateDash()
    //{
    //    //dashSkill.ActivateDash();
    //}

    void Update()
    {
        if (isAlive && GameManager.Instance.IsState(GameState.Gameplay) && target.isAlive)
        {
            //base.Update();
            if (target != null)
            {
                // Calculate the direction vector from the enemy to the target
                Vector3 targetDirection = (target.transform.position - transform.position).normalized;

                // Ignore the y-component to keep the enemy's rotation level
                targetDirection.y = 0f;

                // Rotate the enemy towards the target direction
                if (targetDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                }

                if (currentState != null)
                {
                    currentState.OnExecute(this);
                }
                transform.LookAt(target.transform);
            }

            //thang duoi nay de freeze bot until vao game
            //if (currentState != null && GameManager.Instance.IsState(GameState.Gameplay))
            //{
            //    currentState.OnExecute(this);
            //}

            //DetectEnemies();
        }
        else
        {
            agent.SetDestination(transform.position);
            //agent.isStopped = true; k dung isStopped vi minh dung isStopped cho AttackState hoat dong intentionally
        }

    }
    
    public void OnInit(BossData bossData)
    {
        this.bossData = bossData;
        this.health = bossData.health;
        this.damage = bossData.damage;
        this.movementSpeed = bossData.movementSpeed;
        skill = bossData.bossSkills;
        isAlive = true;
        ChangeState(new BossChaseState());
    }

    protected override void OnDespawn()
    {
        LeanPool.Despawn(this);
        LevelManager.Instance.killCount++;
        UIManager.Instance.killCountText.SetText(LevelManager.Instance.killCount.ToString());
    }


    protected override void OnHit(int damage)
    {
        //base.OnHit(damage);

        health -= damage;

        if (health <= 0)
        {
            //Die();
            isAlive = false;
            ChangeAnim("IsDead");
            agent.SetDestination(transform.position);
            //DropExperienceSphere();
            StopAllCoroutines();
            Invoke("OnDespawn", 3f);
            //this.GetComponent<Collider>().enabled = false;
        }
        Debug.Log(this.name + "'s health : " + this.health);
    }

    public void ChangeState(IState<Boss> state)
    {
        //switch (bossData.bossType)
        //{
        //    case BossType.Melee:
        //        state = new MeleeBossChaseState();
        //        break;
        //    case BossType.Ranged:
        //        state = new RangedBossChaseState();
        //        break;
        //    default:
        //        break;
        //}

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

    //private void DropExperienceSphere()
    //{
    //    float expDropChance = Random.value;
    //    if (expDropChance < lessExpDropRate)
    //    {
    //        LeanPool.Spawn(blueExpSphere, transform.position, Quaternion.identity);
    //    }
    //    else
    //    {
    //        LeanPool.Spawn(redExpSphere, transform.position, Quaternion.identity);
    //    }
    //    // GameObject expSpherePrefabToDrop = Random.value < lowExpDropChance ? lowExpSpherePrefab : highExpSpherePrefab;
    //}

}
