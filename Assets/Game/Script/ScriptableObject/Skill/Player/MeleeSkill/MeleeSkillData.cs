using Lean.Pool;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Skill", menuName = "Skills/Melee Skill")]
public class MeleeSkillData : SkillData
{
    public Melee meleeSkill;
    public float radius;
    //public int numberOfStrikes;
    //public float strikeDelay;

    public override void Activate(Vector3 position, Transform chargePos, Character attacker)
    {
        // Implement area of effect skill activation logic here
        if (meleeSkill != null)
        {
            //Melee melee = LeanPool.Spawn(meleeSkill, chargePos.position, meleeSkill.transform.rotation);
            //Melee melee = LeanPool.Spawn(meleeSkill, chargePos.position, attacker.transform.rotation);
            Quaternion adjustedRotation = Quaternion.Euler(0, 180, 0) * attacker.transform.rotation;
            Melee melee = LeanPool.Spawn(meleeSkill, chargePos.position, adjustedRotation, attacker.transform);
            //melee.transform.SetParent(attacker.transform);
            melee.attacker = attacker;

            //base.Activate(position, chargePos, attacker);
        }
    }

    public override void PlayerActivate(Vector3 position, Transform chargePos, Character attacker)
    {
        Quaternion adjustedRotation = Quaternion.Euler(0, 180, 0) * attacker.transform.rotation;
        Melee melee = LeanPool.Spawn(meleeSkill, chargePos.position, adjustedRotation, attacker.transform);
        //melee.transform.SetParent(attacker.transform);
        melee.attacker = attacker;
    }

    public override void BossActivate(Vector3 position, Transform chargePos, Boss attacker)
    {
        Vector3 spawnPosition = attacker.transform.position + Vector3.up * 1f;
        // Instantiate the melee strike at the boss's position
        Melee newMelee = LeanPool.Spawn(meleeSkill, spawnPosition, Quaternion.identity, attacker.transform);

        newMelee.attacker = attacker;
    }

    public override void BossActivate2(Vector3 position, Transform chargePos, Boss attacker)
    {
        Debug.Log("BossMeleeAttack2");
        Vector3 spawnPosition = attacker.transform.position + Vector3.up * 1f;
        // Instantiate the melee strike at the boss's position
        Melee newMelee = LeanPool.Spawn(meleeSkill, spawnPosition, Quaternion.identity, attacker.transform);

        newMelee.attacker = attacker;
    }


}

