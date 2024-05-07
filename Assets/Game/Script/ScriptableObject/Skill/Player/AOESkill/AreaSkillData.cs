using Lean.Pool;
using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "New AoE Skill", menuName = "Skills/AoE Skill")]
//[Serializable]
public class AreaSkillData : SkillData
{
    public AreaOfEffect aoeSkill;
    public float radius;

    //private bool isCastingSkills = false;

    public override void Activate(Vector3 position, Transform chargePos, Character attacker)
    {
        // Implement area of effect skill activation logic here
        if (aoeSkill != null)
        {
            AreaOfEffect aoe = LeanPool.Spawn(aoeSkill, position, aoeSkill.transform.rotation);
            aoe.attacker = attacker;

            //base.Activate(position, chargePos, attacker);
        }
    }

    public override void BossActivate(Vector3 position, Transform chargePos, Boss attacker)
    {
        //Debug.Log("BossActivate called.");
        // Start casting skills sequentially
        attacker.StartCoroutine(CastSequentialSkills(attacker));
    }

    public override void BossActivate2(Vector3 position, Transform chargePos, Boss attacker)
    {
        //Debug.Log("BossActivate2 called.");
        // Start casting skills simultaneously around the boss
        attacker.StartCoroutine(CastSimultaneousSkills(attacker));
    }

    private IEnumerator CastSimultaneousSkills(Boss attacker)
    {
        //Debug.Log("Starting simultaneous skill casting.");

        // Set the number of skills to cast
        int numberOfSkills = 10; // Adjust the number of skills as needed

        // Set the radius of the circle around the boss where skills will be cast
        float radius = 5f; // Adjust the radius as needed

        // Calculate the angle increment for each skill position
        float angleIncrement = 360f / numberOfSkills;

        // Loop to cast skills simultaneously
        for (int i = 0; i < numberOfSkills; i++)
        {
            // Calculate the angle for the current skill position
            float angle = i * angleIncrement;

            // Calculate the position around the boss using polar coordinates
            Vector3 skillPosition = attacker.transform.position + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;

            AreaOfEffect aoe = LeanPool.Spawn(aoeSkill, skillPosition, aoeSkill.transform.rotation);

            if (i != 0)
            {
                //aoe.audioSource.playOnAwake = false;
                aoe.audioSource.mute = true;
            }
            //else
            //{
            //    aoe.GetComponent<AudioSource>().Play();
            //}

            aoe.attacker = attacker;

            //Debug.Log("Skill casted at position: " + skillPosition);

            yield return null;
        }

        //Debug.Log("Simultaneous skill casting ended.");
    }

    private IEnumerator CastSequentialSkills(Boss attacker)
    {
        //Debug.Log("Starting sequential skill casting.");

        // Set the number of skills to cast
        int numberOfSkills = 10; // Adjust the number of skills as needed
        int skillsCastCount = 0;

        // Set the delay between each skill cast
        float delayBetweenSkills = 1f; // Adjust the delay as needed

        // Loop to continuously cast skills
        while (skillsCastCount < numberOfSkills)
        {
            // Get the current position of the player
            Vector3 playerPosition = attacker.target.transform.position;

            // Spawn the AoE skill at the calculated position
            AreaOfEffect aoe = LeanPool.Spawn(aoeSkill, playerPosition, aoeSkill.transform.rotation);
            aoe.attacker = attacker;

            //Debug.Log("Skill casted. Count: " + skillsCastCount);

            // Increment the skills cast counter
            skillsCastCount++;

            // Wait for the specified delay before casting the next skill
            yield return new WaitForSeconds(delayBetweenSkills);
        }

        //Debug.Log("Sequential skill casting ended.");
    }
}

