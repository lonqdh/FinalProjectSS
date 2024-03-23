using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform chargeSkillPos;
    public int health;
    public float damage;
    public float movementSpeed;

    //protected virtual void OnInit()
    //{
    //    // Initialization logic for the character
    //}

    protected virtual void OnHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Skill skill = other.GetComponent<Skill>(); // can lop skill de getcomponent va check trigger vi k the get component 1 scriptable object
        if (skill != null)
        {
            if (skill.attacker != this)
            {
                OnHit((int)(skill.attacker.damage + skill.skillData.damage));
            }
        }
    }
}
