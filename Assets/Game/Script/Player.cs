using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] internal CharacterData characterData;

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

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        OnInit();
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

        //TestCastSkill
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            ActivateBasicSkill();
        }

        if (Input.GetMouseButtonDown(1)) // Check for left mouse button click
        {
            AddSkill();
        }

    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

    protected override void OnInit()
    {
        ChangeAnim("Idle");
        ChangeCharacter();
    }

    private void ChangeCharacter()
    {
        characterData = DataManager.Instance.GetCharacterData((CharacterType)GameManager.Instance.UserData.EquippedCharacter);
        if(characterModelInstance == null)
        {
            characterModelInstance = Instantiate(characterData.characterModelPrefab, transform.position, transform.rotation);
            characterModelInstance.transform.parent = transform;
        }
        
        LevelManager.Instance.InitializePlayerSkills();
    }

    void ActivateBasicSkill()
    {
        // Ensure the character has a basic skill assigned
        if (characterData != null && characterData.basicSkill != null)
        {
            // Cast an AOE skill at the position of the mouse
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Activate the basic skill at the hit point
                //characterData.basicSkill.Activate(hit.point, chargeSkillPos);
                characterData.basicSkill.Activate(hit.point, chargeSkillPos);

                LeanPool.Spawn(characterData.basicSkill.chargeEffectPrefab, chargeSkillPos.position, chargeSkillPos.rotation);

            }
        }
        else
        {
            Debug.LogWarning("No basic skill assigned to the character.");
        }
    }

    void AddSkill()
    {
        LevelManager.Instance.playerSkills.AddSkill(LevelManager.Instance.skillDatabase.allSkillsList[1]);
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
