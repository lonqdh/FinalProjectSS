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

    //For boss
    public int numberOfProjectiles; // Number of projectiles to spawn
    public float spreadAngle; // Spread angle for the cone (in degrees)

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

        //base.Activate(position, chargePos, attacker);
    }

    public override void BossActivate(Vector3 position, Transform chargePos, Boss attacker)
    {
        // Calculate direction towards the target position
        Vector3 direction = (position - chargePos.position).normalized;
        direction.y = 0f; // Ensure the vertical component is 0 (if projectiles don't use gravity)

        // Calculate the angle between each projectile
        float angleStep = spreadAngle / (numberOfProjectiles - 1);

        // Calculate the initial rotation of the first projectile
        Quaternion initialRotation = Quaternion.LookRotation(direction);

        // Iterate to spawn each projectile
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the rotation for this projectile
            Quaternion spreadRotation = Quaternion.AngleAxis(-spreadAngle / 2f + angleStep * i, Vector3.up);
            Quaternion finalRotation = initialRotation * spreadRotation;

            // Instantiate the projectile with the correct rotation
            Projectile newProjectile = Instantiate(projectile, chargePos.position, finalRotation);

            // Set the attacker of the projectile
            newProjectile.attacker = attacker;

            // Apply force to the projectile in the modified direction
            newProjectile.rb.AddForce(finalRotation * Vector3.forward * projectileSpeed);
        }

        //base.Activate(position, chargePos, attacker);
    }

    public override void BossActivate2(Vector3 position, Transform chargePos, Boss attacker)
    {
        // Calculate direction towards the target position
        Vector3 direction = (position - chargePos.position).normalized;
        direction.y = 0f; // Ensure the vertical component is 0 (if projectiles don't use gravity)

        // Calculate the initial position of the projectiles
        Vector3 initialPosition = chargePos.position;

        // Calculate the angle between each projectile
        float angleStep = 360f / numberOfProjectiles + 10;

        // Iterate to spawn each projectile
        for (int i = 0; i < numberOfProjectiles + 10; i++)
        {
            // Calculate the launch direction for this projectile
            Quaternion launchRotation = Quaternion.Euler(0f, i * angleStep, 0f);
            Vector3 launchDirection = launchRotation * direction;

            // Instantiate the projectile with the correct rotation and position
            //Projectile newProjectile = LeanPool.Spawn(projectile, initialPosition, Quaternion.LookRotation(launchDirection));
            Projectile newProjectile = Instantiate(projectile, initialPosition, Quaternion.LookRotation(launchDirection));

            // Set the attacker of the projectile
            newProjectile.attacker = attacker;

            // Apply the projectile speed to the launch direction
            Vector3 launchVelocity = launchDirection * 5;

            // Apply the launch velocity to the projectile
            newProjectile.rb.velocity = launchVelocity;
        }
    }



    //private IEnumerator Despawn(Projectile projectile)
    //{
    //    yield return new WaitForSeconds(3f);
    //    LeanPool.Despawn(projectile);
    //}

}
