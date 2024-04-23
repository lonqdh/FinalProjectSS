using Lean.Pool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Character
{
    [SerializeField] internal CharacterData characterData;

    public PlayerSkills playerSkills;

    public GameObject characterModelInstance;

    private InputHandler _input;

    [SerializeField]
    private bool RotateTowardMouse;

    [SerializeField]
    private float RotationSpeed;

    public Camera Camera;

    [SerializeField] private float gravityScale;

    private Dictionary<SkillData, float> skillCooldowns = new Dictionary<SkillData, float>();

    public PlayerExperience playerExperience;

    public HealthBar healthBar;

    public ExperienceBar experienceBar;

    public int currentMaxHealth;

    //public List<SkillCooldown> skillCooldownUIList = new List<SkillCooldown>();


    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        OnInit();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && GameManager.Instance.IsState(GameState.Gameplay))
        {
            //Ray ray = Camera.ScreenPointToRay(_input.MousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            if (isAlive)
            {
                // Get the input vector from the input handler
                var inputVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

                // Move the player toward the input direction
                var movementVector = MoveTowardTarget(inputVector);

                // Rotate the player based on the input or mouse direction
                if (!RotateTowardMouse)
                {
                    RotateTowardMovementVector(movementVector);
                }
                else
                {
                    RotateFromMouseVector();
                }

                // Check if the player is moving
                if (movementVector.magnitude > 0)
                {
                    // If moving, trigger the run animation
                    //ChangeAnim("IsRun");
                    UpdateAnimations(inputVector);
                }
                else // Player is idle
                {
                    ChangeAnim("IsIdle");
                }
                //UpdateAnimations();
                // Update skill cooldowns and activate available skills
                UpdateSkillCooldowns();
                ActivateAvailableSkills();
            }
        }

    }

    public void OnInit()
    {
        base.OnInit();
        playerSkills = LevelManager.Instance.currentSkillsList;
        ChangeAnim("IsIdle");
        ChangeCharacter();
        ChangeStats();
        healthBar.UpdateHealthBar(characterData.health, this.health);
        experienceBar.UpdateUI();
    }

    protected override void OnHit(int damage, Vector3 attackerPosition)
    {
        base.OnHit(damage, attackerPosition);
        //health -= damage;
        Debug.Log("Player's health: " + health);
        healthBar.UpdateHealthBar(characterData.health, this.health);
        if(health <= 0)
        {
            //this.GetComponent<Collider>().enabled = false;
            GameObject newDeathVfx = LeanPool.Spawn(deathVfx, transform);
            newDeathVfx.transform.position = transform.position;
            LeanPool.Despawn(newDeathVfx, 5f);
            isAlive = false;
            ChangeAnim("IsDead");
            Invoke("OnDespawn", 5f);
            Debug.Log("Player has died!");
        }
    }

    protected override void OnDespawn()
    {
        //base.OnDespawn();
        Destroy(this.gameObject);
        UIManager.Instance.FinishMatch();
    }

    private void RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);
        Vector3 targetVector;
        Quaternion targetRotation;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            targetVector = new(hitInfo.point.x, transform.position.y, hitInfo.point.z);
            targetRotation = Quaternion.LookRotation(targetVector - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = movementSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0)
        {
            return;
        }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }

    private void ChangeCharacter()
    {
        characterData = DataManager.Instance.GetCharacterData((CharacterType)GameManager.Instance.UserData.EquippedCharacter);
        if (characterModelInstance == null)
        {
            characterModelInstance = Instantiate(characterData.characterModelPrefab, transform.position, transform.rotation);

            characterModelInstance.GetComponent<Animator>().runtimeAnimatorController = anim.runtimeAnimatorController;
            //characterData.characterModelPrefab.GetComponent<Animator>().runtimeAnimatorController = anim.runtimeAnimatorController;
            anim = characterModelInstance.GetComponent<Animator>();
            characterModelInstance.transform.parent = transform;
        }

        LevelManager.Instance.InitializePlayerSkills();
    }

    private void ChangeStats()
    {
        this.health = characterData.health;
        this.movementSpeed = characterData.movementSpeed;
        this.damage = characterData.damage;

        currentMaxHealth = this.health;
    }

    public void LevelUp()
    {
        currentMaxHealth += playerExperience.level * 10; // tang stats theo level
        this.health = currentMaxHealth; // len level la mau tu set thanh max cua maxhealth hien tai
        this.movementSpeed += 1f;
        this.damage += playerExperience.level * 2;
    }

    private void UpdateSkillCooldowns()
    {
        // Update cooldown for each skill
        foreach (var skillCooldown in skillCooldowns.ToList())
        {
            if (skillCooldown.Value > 0f)
            {
                skillCooldowns[skillCooldown.Key] -= Time.deltaTime;
            }
        }
    }

    //private void ActivateAvailableSkills()
    //{
    //    // Iterate through the player's skills
    //    for (int i = 0; i < playerSkills.currentSkills.Count; i++)
    //    {
    //        SkillData skill = playerSkills.currentSkills[i];

    //        // Check if the skill is not on cooldown
    //        if (!IsSkillOnCooldown(skill))
    //        {
    //            // Get the direction from the player to the mouse cursor
    //            Vector3 mouseDirection = GetMouseDirection();

    //            if (skill.skillType == SkillType.Projectile)
    //            {
    //                // Calculate the casting position based on the player's position and the mouse direction
    //                Vector3 castPosition = transform.position + mouseDirection.normalized * skill.rangeRadius;

    //                // Activate the skill at the calculated position
    //                skill.Activate(castPosition, chargeSkillPos.transform, this);
    //                skillCooldowns[skill] = skill.cooldown;
    //            }
    //            else if (skill.skillType == SkillType.AreaOfEffect)
    //            {
    //                float distanceToMouse = mouseDirection.magnitude;

    //                if (distanceToMouse <= skill.rangeRadius)
    //                {
    //                    // Calculate the casting position based on the player's position and the mouse direction
    //                    Vector3 castPosition = transform.position + mouseDirection.normalized * distanceToMouse;

    //                    // Activate the skill at the calculated position
    //                    skill.Activate(castPosition, chargeSkillPos.transform, this);
    //                    skillCooldowns[skill] = skill.cooldown;
    //                }

    //                //Ray groundRay = Camera.ScreenPointToRay(_input.MousePosition);// dung cai nay de k bi cast tren khong trung khi o tren tang cao hon noi muon cast
    //                //if (Physics.Raycast(groundRay, out RaycastHit groundHit, Mathf.Infinity) && distanceToMouse <= skill.rangeRadius)
    //                //{
    //                //    // Calculate the casting position based on the ground position
    //                //    Vector3 castPosition = groundHit.point;

    //                //    // Activate the skill at the calculated position
    //                //    skill.Activate(castPosition, chargeSkillPos.transform, this);
    //                //    skillCooldowns[skill] = skill.cooldown;
    //                //}
    //            }


    //        }
    //    }
    //}

    //private Vector3 GetMouseDirection()
    //{
    //    Ray ray = Camera.ScreenPointToRay(_input.MousePosition);
    //    Vector3 mouseDirection = Vector3.zero;

    //    if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
    //    {
    //        // Get the collider bounds of the hit object
    //        Collider collider = hitInfo.collider;
    //        Bounds bounds = collider.bounds;

    //        // Calculate the lowest point of the collider (the feet)
    //        Vector3 lowestPoint = bounds.center - Vector3.up * bounds.extents.y; //cach nay k dung min thi do lower performance khi minh tinh thu cong

    //        // Set the mouse direction to the lowest point of the collider
    //        mouseDirection = lowestPoint - transform.position;

    //        // Ensure that the direction is horizontal (ignoring the vertical component)
    //        mouseDirection.y = 0f;
    //    }
    //    else
    //    {
    //        // If the raycast doesn't hit anything, just use the direction from the player to the mouse cursor
    //        mouseDirection = _input.MousePosition - Camera.WorldToScreenPoint(transform.position);
    //    }

    //    return mouseDirection;
    //}

    private void ActivateAvailableSkills()
    {
        // Iterate through the player's skills
        for (int i = 0; i < playerSkills.currentSkills.Count; i++)
        {
            SkillData skill = playerSkills.currentSkills[i];

            // Check if the skill is not on cooldown
            if (!IsSkillOnCooldown(skill))
            {
                if (skill.skillType == SkillType.Projectile || skill.skillType == SkillType.Melee)
                {
                    // Get the direction from the player to the mouse cursor
                    Vector3 mouseDirection = GetMouseDirection();

                    // Calculate the casting position based on the player's position and the mouse direction
                    Vector3 castPosition = transform.position + mouseDirection.normalized * skill.rangeRadius;

                    // Activate the skill at the calculated position
                    skill.Activate(castPosition, chargeSkillPos.transform, this);
                    skillCooldowns[skill] = skill.cooldown;

                    SkillCooldown cooldownUI = FindSkillCooldownUI(skill);
                    if (cooldownUI != null)
                    {
                        cooldownUI.StartCooldown(skill.cooldown);
                    }
                }
                else if (skill.skillType == SkillType.AreaOfEffect)
                {
                    float distanceToMouse = GetMouseDirectionAoE().magnitude;

                    if (distanceToMouse <= skill.rangeRadius)
                    {
                        // Calculate the casting position based on the player's position and the mouse direction
                        Vector3 castPosition = transform.position + GetMouseDirectionAoE().normalized * distanceToMouse;

                        // Activate the skill at the calculated position
                        skill.Activate(castPosition, chargeSkillPos.transform, this);
                        skillCooldowns[skill] = skill.cooldown;

                        SkillCooldown cooldownUI = FindSkillCooldownUI(skill);
                        if (cooldownUI != null)
                        {
                            cooldownUI.StartCooldown(skill.cooldown);
                        }
                    }
                }
            }
        }
    }

    private Vector3 GetMouseDirection()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);
        Vector3 mouseDirection = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            //// Check if the hit collider belongs to the player
            //if (hitInfo.collider.CompareTag("Player"))
            //{
            //    // Calculate the direction in front of the character
            //    mouseDirection = transform.forward;
            //}
            //else
            //{
            //    // Get the collider bounds of the hit object
            //    Collider collider = hitInfo.collider;
            //    Bounds bounds = collider.bounds;

            //    // Calculate the lowest point of the collider (the feet)
            //    Vector3 lowestPoint = bounds.center - Vector3.up * bounds.extents.y;

            //    // Set the mouse direction to the lowest point of the collider
            //    mouseDirection = lowestPoint - transform.position;

            //    // Ensure that the direction is horizontal (ignoring the vertical component)
            //    mouseDirection.y = 0f;

            //    //mouseDirection = transform.forward;

            //}

            mouseDirection = transform.forward;

        }
        else
        {
            // If the raycast doesn't hit anything, just use the direction from the player to the mouse cursor
            mouseDirection = _input.MousePosition - Camera.WorldToScreenPoint(transform.position);
        }

        return mouseDirection;
    }


    private Vector3 GetMouseDirectionAoE()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);
        Vector3 mouseDirection = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            // Check if the hit collider belongs to the player
            if (hitInfo.collider.CompareTag("Player"))
            {
                // Calculate the direction in front of the character
                mouseDirection = transform.forward;
            }
            else
            {
                // Get the collider bounds of the hit object
                Collider collider = hitInfo.collider;
                Bounds bounds = collider.bounds;

                // Calculate the lowest point of the collider (the feet)
                Vector3 lowestPoint = bounds.center - Vector3.up * bounds.extents.y;

                // Set the mouse direction to the lowest point of the collider
                mouseDirection = lowestPoint - transform.position;

                // Ensure that the direction is horizontal (ignoring the vertical component)
                mouseDirection.y = 0f;
            }
        }
        else
        {
            // If the raycast doesn't hit anything, just use the direction from the player to the mouse cursor
            mouseDirection = _input.MousePosition - Camera.WorldToScreenPoint(transform.position);
        }

        return mouseDirection;
    }




    bool IsSkillOnCooldown(SkillData skill)
    {
        // Check if the skill exists in the cooldown dictionary
        if (skillCooldowns.ContainsKey(skill))
        {
            // Check if the cooldown time for the skill has elapsed
            return skillCooldowns[skill] > 0f;
        }
        else
        {
            // If the skill is not found in the cooldown dictionary, it's not on cooldown
            return false;
        }
    }

    public void AddSkill()
    {
        UIManager.Instance.OpenLevelUpUI();
    }

    private void UpdateAnimations(Vector3 inputVector)
    {
        // Calculate the angle between the player's forward direction and the movement direction
        float angle = Vector3.SignedAngle(transform.forward, inputVector, Vector3.up);

        // Set animation parameters based on the angle
        if (angle > 45f && angle < 135f)
        {
            // Move right animation
            ChangeAnim("IsRunRight");
        }
        else if (angle < -45f && angle > -135f)
        {
            // Move left animation
            ChangeAnim("IsRunLeft");
        }
        else if (angle >= 135f || angle <= -135f)
        {
            // Move backward animation
            ChangeAnim("IsRunBackward");
        }
        else
        {
            ChangeAnim("IsRun");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        ExperienceSphere exp = other.GetComponent<ExperienceSphere>();

        if (exp != null)
        {
            playerExperience.AddExp(exp.experienceAmount);
            experienceBar.UpdateUI();
            LeanPool.Despawn(other);
        }
    }

    private SkillCooldown FindSkillCooldownUI(SkillData skill)
    {
        // Find the associated SkillCooldownUI component based on the skill
        foreach (SkillCooldown cooldownUI in UIManager.Instance.skillCooldownUIList)
        {
            if (cooldownUI.skill == skill)
            {
                //Debug.Log("Found");
                return cooldownUI;
            }
        }
        return null; // Return null if no associated SkillCooldown component is found
    }

}
