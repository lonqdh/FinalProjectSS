using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Skill", menuName = "Skills/Projectile Skill")]
//[Serializable]
public class ProjectileSkillData : SkillData
{
    //public GameObject impactParticle;
    //public GameObject projectileParticle;
    //public GameObject muzzleParticle;
    //public GameObject[] trailParticles;
    //public GameObject projectilePrefab;


    //public Projectile projectile;
    //[Header("Adjust if not using Sphere Collider")]
    //public float colliderRadius = 1f;
    //[Range(0f, 1f)]
    //public float collideOffset = 0.15f;
    //public string skillName;
    //public SkillType skillType;
    //public int damage;
    //public float cooldown;


    public Projectile projectile;
    public float projectileSpeed;

    public override void Activate(Vector3 position, Transform chargePos)
    {
        throw new NotImplementedException();
    }



    //public override void Activate(Vector3 position)
    //{
    //    // Implement projectile skill activation logic here
    //    if (visualEffectPrefab != null)
    //    {
    //        Instantiate(visualEffectPrefab, position, Quaternion.identity);
    //        Instantiate(projectile, position, Quaternion.identity);
    //    }
    //    // Implement projectile logic (e.g., instantiate and launch a projectile)
    //    Debug.Log("Projectile skill activated!");
    //}
}
