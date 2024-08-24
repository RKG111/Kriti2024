using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    //collectible items counter
    public int itemCounter = 0;
    public float sprintSpeedMultiplier = 2.0f;
    public float staminaMax = 100.0f;
    public float staminaRegenRate = 10.0f;
    public float sprintCostPerSecond = 20.0f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    private float currentStamina;
    private bool isSprinting;

    private CharacterStats stats;

    [SerializeField]
    private Light2D fov;

    private UIManager uIManager;
    private Dictionary<Vector2, float> fovDirection = new Dictionary<Vector2, float>
    {
        { Vector2.up, 0f },        // Up
        { Vector2.down, 180f },    // Down
        { Vector2.left, 90f },    // Left
        { Vector2.right, -90f },     // Right
        {new Vector2(1,1),-45f},     // up-right
        {new Vector2(-1,1),45f},     // up-left
        {new Vector2(1,-1),-135f},     // down-right
        {new Vector2(-1,-1),135f},     // down-left
    };
    // private Dictionary<Vector2, float> direction = new Dictionary<Vector2, float>
    // {
    //     { Vector2.up, 0f },        // Up
    //     { Vector2.down, 180f },    // Down
    //     { Vector2.left, 90f },    // Left
    //     { Vector2.right, -90f }    // Right
    //  // down-left
    // };
    void OnEnable()
    {   
        uIManager = GameObject.FindObjectOfType<UIManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();
        staminaMax = stats.stamina;
        currentStamina = staminaMax;
    }

    void Update()
    {
        HandleMovementInput();

        // Update animator parameters
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        // Move the player based on the movement input
        MovePlayer();

        // Handle sprinting
        HandleSprinting();

        
        RegenerateStamina();
        
        
    }

    private void HandleMovementInput()
    {
        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void MovePlayer()
    {
        // Apply movement with or without sprinting
        float speed = isSprinting ? movementSpeed * sprintSpeedMultiplier : movementSpeed;
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    private void HandleSprinting()
    {
        // Deduct stamina while sprinting
        if (isSprinting & movement.magnitude > 0.01)
        {
            float sprintCost = sprintCostPerSecond * Time.fixedDeltaTime;
            currentStamina = Mathf.Max(0, currentStamina - sprintCost);
            uIManager.staminaBar.value = currentStamina;
        }
    }

    private void RegenerateStamina()
    {
        // Regenerate stamina over time
        if (!isSprinting && currentStamina < staminaMax)
        {
            currentStamina = Mathf.Min(staminaMax, currentStamina + staminaRegenRate * Time.fixedDeltaTime);
            uIManager.staminaBar.value = currentStamina;
        }
    }
    private void RotateFOV()
    {
        // Debug.Log(movement);
        if (fovDirection.ContainsKey(movement))
        {

            fov.transform.rotation = Quaternion.Euler(0f, 0f, fovDirection[movement]);
        }
    }

    
}
