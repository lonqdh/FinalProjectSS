using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Character
{
    [SerializeField] internal CharacterData characterData;

    public PlayerSkills playerSkills;

    public GameObject characterModelInstance;

    [SerializeField] private Animator anim;
    protected string currentAnimName = "";

    private InputHandler _input;

    [SerializeField]
    private bool RotateTowardMouse;

    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private Camera Camera;

    private Rigidbody rb;

    [SerializeField] private float gravityScale;

    private Dictionary<SkillData, float> skillCooldowns = new Dictionary<SkillData, float>();

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        OnInit();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);



        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);
        }
        else
        {
            RotateFromMouseVector();
        }

        if (movementVector.magnitude > 0.1f) // Player is moving
        {
            ChangeAnim("Run");
        }
        else // Player is idle
        {
            ChangeAnim("Idle");
        }

        UpdateSkillCooldowns();
        ActivateAvailableSkills();

    }

    public void OnInit()
    {
        ChangeAnim("Idle");
        ChangeCharacter();
        ChangeStats();
    }

    protected override void OnHit(int damage)
    {
        base.OnHit(damage);
        Debug.Log("player's health : " + this.health);
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
        var speed = characterData.movementSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        ChangeAnim("Run");
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }

    private void ChangeCharacter()
    {
        characterData = DataManager.Instance.GetCharacterData((CharacterType)GameManager.Instance.UserData.EquippedCharacter);
        if (characterModelInstance == null)
        {
            characterModelInstance = Instantiate(characterData.characterModelPrefab, transform.position, transform.rotation);
            characterModelInstance.transform.parent = transform;
        }

        LevelManager.Instance.InitializePlayerSkills();
    }

    private void ChangeStats()
    {
        this.health = characterData.health;
        this.movementSpeed = characterData.movementSpeed;
        this.damage = characterData.damage;
    }

    //void ActivateSkill()
    //{
    //    // Ensure the character has a basic skill assigned
    //    if (characterData != null && characterData.basicSkill != null)
    //    {
    //        // Cast an AOE skill at the position of the mouse
    //        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            // Activate the basic skill at the hit point
    //            //characterData.basicSkill.Activate(hit.point, chargeSkillPos);




    //            //characterData.basicSkill.Activate(hit.point, chargeSkillPos);

    //            //LeanPool.Spawn(characterData.basicSkill.chargeEffectPrefab, chargeSkillPos.position, chargeSkillPos.rotation);

    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("No basic skill assigned to the character.");
    //    }
    //}

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

    private void ActivateAvailableSkills()
    {
        // Iterate through the player's skills
        for (int i = 0; i < playerSkills.currentSkills.Count; i++)
        {
            SkillData skill = playerSkills.currentSkills[i];

            // Check if the skill is not on cooldown
            if (!IsSkillOnCooldown(skill))
            {
                // Activate the skill
                //playerSkills.skills[i].skillData.Activate( , ,this);
                //enemy.enemyData.enemySkill.Activate(enemy.target.transform.position, enemy.chargeSkillPos, enemy);

                // Get the direction from the player to the mouse cursor
                Vector3 mouseDirection = GetMouseDirection();

                // Calculate the casting position based on the player's position and the mouse direction
                Vector3 castPosition = transform.position + mouseDirection.normalized * skill.rangeRadius;

                // Activate the skill at the calculated position
                skill.Activate(castPosition, chargeSkillPos.transform, this);
                skillCooldowns[skill] = skill.cooldown;
            }
        }
    }

    private Vector3 GetMouseDirection()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);
        Vector3 mouseDirection = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            mouseDirection = hitInfo.point - transform.position;
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

    void AddSkill()
    {
        //playerSkills.AddSkill();
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
