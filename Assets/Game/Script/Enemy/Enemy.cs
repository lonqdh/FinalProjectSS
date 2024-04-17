using Lean.Pool;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] internal EnemyData enemyData;
    private IState<Enemy> currentState;
    public Player target;
    public NavMeshAgent agent;
    //public bool isAttacking;
    internal SkillData skill;
    [SerializeField] private float rotateSpeed = 5f;
    public ExperienceSphere blueExpSphere;
    public ExperienceSphere redExpSphere;
    private float lessExpDropRate = 0.8f;
    public bool hasSprayingSkill;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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

    protected override void OnDespawn()
    {
        LeanPool.Despawn(this);
        LevelManager.Instance.killCount++;
        UIManager.Instance.killCountText.SetText(LevelManager.Instance.killCount.ToString());
    }

    //Called where the SpawnEnemies method is called so once
    //spawned the enemy will call the OnInit method
    public void OnInit(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        this.health = enemyData.health;
        this.damage = enemyData.damage;
        this.movementSpeed = enemyData.movementSpeed;
        this.hitVfx = enemyData.hitVfx;
        this.deathVfx = enemyData.deathVfx;
        skill = enemyData.enemySkill;
        if(skill.skillType == SkillType.Spray)
        {
            hasSprayingSkill = true;
        }
        isAlive = true;
        ChangeState(new ChaseState());
    }

    protected override void OnHit(int damage)
    {
        base.OnHit(damage);

        //health -= damage;

        if (health <= 0)
        {
            //Die();
            isAlive = false;
            GameObject newDeathVfx = LeanPool.Spawn(deathVfx, transform);
            newDeathVfx.transform.position = transform.position;
            LeanPool.Despawn(newDeathVfx, 5f);
            ChangeAnim("IsDead");
            agent.SetDestination(transform.position);
            DropExperienceSphere();
            StopAllCoroutines();
            Invoke("OnDespawn", 3f);
            //this.GetComponent<Collider>().enabled = false;
        }
        Debug.Log(this.name + "'s health : " + this.health);
    }

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

    private void DropExperienceSphere()
    {
        float expDropChance = Random.value;
        if (expDropChance < lessExpDropRate)
        {
            LeanPool.Spawn(blueExpSphere, transform.position, Quaternion.identity);
        }
        else
        {
            LeanPool.Spawn(redExpSphere, transform.position, Quaternion.identity);
        }
        // GameObject expSpherePrefabToDrop = Random.value < lowExpDropChance ? lowExpSpherePrefab : highExpSpherePrefab;
    }
}
