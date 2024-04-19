using UnityEngine;

public class Projectile : Skill
{
    //public ProjectileSkillData projectileSkillData;
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [Header("Adjust if not using Sphere Collider")]
    public float colliderRadius = 1f;
    [Range(0f, 1f)]
    public float collideOffset = 0.15f;

    public Rigidbody rb;
    private SphereCollider sphereCollider;
    private RaycastHit hit;

    private float rad;
    private float dist;
    private Vector3 dir;
    private int experienceLayerNum = 8;
    private int skillLayerNum = 7;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        if (projectileParticle != null)
        {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.parent = transform;
        }

        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f);
        }

        //rb.velocity = transform.forward * projectileSpeed;

    }

    //public void OnInit(Character attacker)
    //{
    //    transform.position = attacker.chargeSkillPos.position;

    //    // Reset any other necessary properties of the projectile
    //    rb.velocity = Vector3.zero;
    //}

    void FixedUpdate()
    {
        if (sphereCollider)
            rad = sphereCollider.radius;
        else
            rad = colliderRadius;

        dir = rb.velocity;

        if (rb.useGravity)
            dir += Physics.gravity * Time.deltaTime;
        dir = dir.normalized;

        dist = rb.velocity.magnitude * Time.deltaTime;

        if (Physics.SphereCast(transform.position, rad, dir, out hit, dist))
        {
            if (hit.collider.gameObject.layer != attacker.gameObject.layer && hit.collider.gameObject.layer != experienceLayerNum && hit.collider.gameObject.layer != skillLayerNum)
            {
                transform.position = hit.point + (hit.normal * collideOffset);

                GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;


                Destroy(projectileParticle, 3f);
                Destroy(impactP, 5.0f);

                Destroy(gameObject);
                //dung despawn se loi~
                //LeanPool.Despawn(projectileParticle, 3f);
                //LeanPool.Despawn(impactP, 3f);
                //LeanPool.Despawn(gameObject, 3f);
                //else if (dist >= projectileSkillData.maxDistance)
                //{
                //    // If the projectile reaches its maximum distance without hitting anything, despawn it
                //    Destroy(projectileParticle, 3f);
                //    Destroy(gameObject);
                //    //LeanPool.Despawn(projectileParticle, 3f);
                //    //LeanPool.Despawn(gameObject);
                //}
            }
        }
    }
}


