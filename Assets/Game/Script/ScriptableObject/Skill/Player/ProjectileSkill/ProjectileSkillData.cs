using Lean.Common;
using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Skill", menuName = "Skills/Projectile Skill")]
//[Serializable]
public class ProjectileSkillData : SkillData
{
    public Projectile projectile;
    public float maxDistance;
    public float projectileSpeed;

    public override void Activate(Vector3 position, Transform chargePos, Character attacker)
    {
        Vector3 direction = (position - chargePos.position).normalized;

        // Ensure the vertical component of the direction is not negative (pointing downward)
        //direction.y = Mathf.Max(direction.y, 0f);

        direction.y = 0; // set cho projectile luon luon bay 1 duong thang neu projectile khong useGravity

        // Normalize the direction vector
        direction.Normalize();

        // Instantiate the projectile with the correct rotation
        Projectile newProjectile = Instantiate(projectile, chargePos.position, Quaternion.LookRotation(direction));

        // Set the attacker of the projectile
        newProjectile.attacker = attacker;
        //newProjectile.shooter = attacker;


        // Apply force to the projectile in the modified direction
        newProjectile.rb.AddForce(direction * projectileSpeed);
    }
    
}
