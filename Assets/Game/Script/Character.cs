using Lean.Pool;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform chargeSkillPos;
    public int health;
    public float damage;
    public float movementSpeed;
    [SerializeField] internal Rigidbody rb;
    [SerializeField] internal LayerMask enemyLayer;
    [SerializeField] internal GameObject hitVfx;
    [SerializeField] internal GameObject deathVfx;
    [SerializeField] protected Animator anim;
    protected string currentAnimName = "";
    //[SerializeField] internal Collider collider;
    [SerializeField] internal bool isAlive;

    public int pushbackForce;

    // Smooth knockback variables
    public float knockbackDuration = 0.5f;
    private Vector3 knockbackDirection;
    private bool isKnockbackActive = false;

    public float knockbackCooldown = 2.0f; // Cooldown period between knockbacks
    private bool isKnockbackCooldown = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnInit()
    {
        //collider = GetComponent<Collider>();
        isAlive = true;
        //this.GetComponent<Collider>().enabled = true;
        //anim = GetComponent<Animator>();
    }

    //protected virtual void OnHit(int damage, Vector3 attackerPosition)
    //{
    //    // Reduce health
    //    health -= damage;

    //    // Instantiate hit VFX
    //    GameObject newHitVfx = LeanPool.Spawn(hitVfx, transform.position, Quaternion.identity);
    //    LeanPool.Despawn(newHitVfx, 3f);

    //    // Calculate knockback direction
    //    knockbackDirection = (transform.position - attackerPosition).normalized;

    //    // Start smooth knockback effect
    //    StartCoroutine(StartKnockback());
    //}

    protected virtual void OnHit(int damage, Vector3 attackerPosition)
    {
        // Check if knockback is on cooldown
        //if (!isKnockbackCooldown)
        //{
            // Reduce health
            health -= damage;

            // Instantiate hit VFX
            GameObject newHitVfx = LeanPool.Spawn(hitVfx, transform.position, Quaternion.identity);
            LeanPool.Despawn(newHitVfx, 3f);

            //// Calculate knockback direction
            //knockbackDirection = (transform.position - attackerPosition).normalized;

            //// Start smooth knockback effect
            //StartCoroutine(StartKnockback());

            //// Start knockback cooldown
            //StartCoroutine(StartKnockbackCooldown());
        //}
    }

    //protected IEnumerator StartKnockbackCooldown()
    //{
    //    // Set knockback cooldown flag
    //    isKnockbackCooldown = true;

    //    // Wait for knockback cooldown duration
    //    yield return new WaitForSeconds(knockbackCooldown);

    //    // Reset knockback cooldown flag
    //    isKnockbackCooldown = false;
    //}

    //protected IEnumerator StartKnockback()
    //{
    //    // Activate knockback flag
    //    isKnockbackActive = true;

    //    // Apply knockback force
    //    rb.velocity = knockbackDirection * pushbackForce;

    //    // Wait for knockback duration
    //    yield return new WaitForSeconds(knockbackDuration);

    //    // Deactivate knockback flag
    //    isKnockbackActive = false;
    //}

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
                    OnHit((int)(skill.attacker.damage + skill.skillData.damage), skill.attacker.transform.position);
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
