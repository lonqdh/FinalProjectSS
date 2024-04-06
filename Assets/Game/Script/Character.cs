using Lean.Pool;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform chargeSkillPos;
    public int health;
    public float damage;
    public float movementSpeed;
    [SerializeField] internal Rigidbody rb;
    [SerializeField] internal LayerMask enemyLayer;

    [SerializeField] protected Animator anim;
    protected string currentAnimName = "";
    //[SerializeField] internal Collider collider;
    [SerializeField] internal bool isAlive;

    private void Start()
    {
        //OnInit();
    }

    protected virtual void OnInit()
    {
        //collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        isAlive = true;
        //this.GetComponent<Collider>().enabled = true;
        //anim = GetComponent<Animator>();
    }

    protected virtual void OnHit(int damage)
    {
        health -= damage;
    }
    protected virtual void OnDespawn()
    {
        LeanPool.Despawn(this);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (isAlive)
        {
            // Check if the current GameObject's layer is not the enemy layer
            // Retrieve the Skill component from the colliding GameObject
            Skill skill = other.GetComponent<Skill>();
            // Check if a Skill component is found and if the attacker is not the current GameObject
            if (skill != null && skill.attacker != this && skill.attacker.gameObject.layer != this.gameObject.layer) // For the skill not to damage the attacker himself and attack those whos on the same side as the attacker
            {
                if (skill.attacker.gameObject.layer != enemyLayer)
                {
                    // Process the collision by calling the OnHit method
                    OnHit((int)(skill.attacker.damage + skill.skillData.damage));
                }
            }
        }
        else
        {
            return;
        }

    }

    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(currentAnimName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
}
