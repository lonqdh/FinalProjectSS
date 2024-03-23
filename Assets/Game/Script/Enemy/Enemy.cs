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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!IsDead && GameManager.Instance.IsState(GameState.Gameplay))
        //{
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
            //transform.LookAt(target.transform);
        }

        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

        //thang duoi nay de freeze bot until vao game
        //if (currentState != null && GameManager.Instance.IsState(GameState.Gameplay))
        //{
        //    currentState.OnExecute(this);
        //}

        //DetectEnemies();
        //}
    }

    void OnDespawn()
    {
        LeanPool.Despawn(this);
        LevelManager.Instance.killCount++;
    }

    //Called where the SpawnEnemies method is called so once
    //spawned the enemy will call the OnInit method
    public void OnInit(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        this.health = enemyData.health;
        this.damage = enemyData.damage;
        this.movementSpeed = enemyData.movementSpeed;
        skill = enemyData.enemySkill;
        ChangeState(new ChaseState());
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
}
