using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController2 : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;
    Vector2 moveDirection;

    public float walkSpeed = 10;
    public float runSpeed = 20;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    bool jumpButtonPressed;
    public float jumpVelocity = 5;
    public float gravityScale = 1.0f;
    public float hoverGravityScale = 0.5f;

    public static float globalGravity = -9.81f;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;

    bool isGrounded;

    Animator animator;

    Rigidbody rb;

    Collider coll;
    Collider punchColl;

    bool isRunning;

    bool hoverButtonPressed;
    bool isHovering;
    bool canStartHover;
    public float hoverTimeSeconds = 5.0f;

    float hoverStartTime;

    bool isPunching;

    bool freezeWalking;
    bool freezeJumping;

    bool isSpeedBoosted;
    public float speedBoostMagnitude = 2.0f;

    float distanceToGround;

    public float speedBoostDuration = 5.0f;
    float speedBoostStartTime;

    public static bool gamePaused;
    bool pauseButtonPressed;

    ParticleSystem punchParticles;

    int jumpsLeft = 1;

    bool hasDoubleJump;
    public float doubleJumpDuration = 5.0f;
    float doubleJumpStartTime;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        //controls.Player.Move.performed += ctx => SendMessage(ctx.ReadValue<Vector2>());

        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Jump.started += ctx => jumpButtonPressed = true;
        controls.Player.Jump.canceled += ctx => jumpButtonPressed = false;

        controls.Player.Run.performed += ctx => isRunning = true;
        controls.Player.Run.canceled += ctx => isRunning = false;

        controls.Player.Hover.performed += ctx => hoverButtonPressed = true;
        controls.Player.Hover.canceled += ctx => hoverButtonPressed = false;

        controls.Player.Punch.started += ctx => isPunching = true;
        controls.Player.Punch.canceled += ctx => isPunching = false;

        controls.Player.Pause.started += ctx => pauseButtonPressed = true;
        controls.Player.Pause.canceled += ctx => pauseButtonPressed = false;
 
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        coll = GetComponent<CapsuleCollider>();
        punchColl = GetComponent<BoxCollider>();

        punchColl.enabled = false;

        distanceToGround = coll.bounds.extents.y - 1.25f;

        punchParticles = GetComponentInChildren<ParticleSystem>();

        //punchDuration = animator.GetCurrentAnimatorStateInfo(0).length * 0.66f;

        

    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void SendMessage(Vector2 coordinates)
    {
        Debug.Log("Thumb-stick coordinates = " + coordinates);
    }

    void Update()
    {
        if (pauseButtonPressed)
        {
            pauseButtonPressed = false;
            gamePaused = !gamePaused;
            pauseGame();
        }
    }

    void FixedUpdate()
    {
        //if player can walk
        if (!freezeWalking)
        {
            //use isRunning bool to set movement speed. scaled with move magnitude
            float speed = ((isRunning && isGrounded) ? runSpeed : walkSpeed) * move.magnitude;

            if (isSpeedBoosted)
            {
                speed = speed * speedBoostMagnitude;
                isRunning = true;
            }

            //update movement
            Vector3 movement = new Vector3(move.x, 0.0f, move.y) * speed * Time.deltaTime;
            rb.AddForce(movement, ForceMode.VelocityChange);

            //normalize movement for rotation
            Vector2 moveDirection = move.normalized;

            

            //if the user moves player
            if (moveDirection != Vector2.zero)
            {
                //rotate the player to face forwards
                float targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            }

            //update blend tree to determing walking/running animation
            float animationSpeedPercent = ((isRunning) ? 1 : 0.5f) * move.magnitude;
            animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        }

        //set player gravity depending on hover status
        Vector3 gravity = ((isHovering) ? hoverGravityScale : gravityScale) * globalGravity * Vector3.up;

        //accelerate player downwards by gravity
        rb.AddForce(gravity, ForceMode.Acceleration);

        //update isGrounded
        isGrounded = Physics.Raycast(coll.transform.position, -Vector3.up, distanceToGround + 0.5f);
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            canStartHover = true;
            jumpsLeft = 1;
            if (hasDoubleJump)
            {
                jumpsLeft = 2;
            }
        }

        //start a jump if player is on ground/jump button pressed/jumping isn't frozen
        if (jumpButtonPressed && jumpsLeft > 0 && !freezeJumping)
        {
            Jump();
        }

        

        if (rb.velocity.y < 0)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
                animator.SetBool("isFalling", false);
            }
            else
            {
                if (animator.GetBool("isJumping") == false)
                {
                    animator.SetBool("isFalling", true);
                    jumpsLeft = 0;
                }

                if (jumpsLeft == 0)
                {
                    if (hoverButtonPressed)
                    {

                        if (canStartHover)
                        {
                            startHover();
                        }
                    }
                    else
                    {
                        isHovering = false;
                    }
                }

            }
        }


        //end hovering if player exceeds hover time limit
        if (isHovering)
        {
            if (Time.time - hoverStartTime >= hoverTimeSeconds)
            {
                isHovering = false;
            }
        }

        //punch if punch button pressed/player not airborne
        if (isPunching && isGrounded)
        {
            startPunch();
        }

        if (isSpeedBoosted)
        {
            if (Time.time - speedBoostStartTime >= speedBoostDuration)
            {
                isSpeedBoosted = false;
                isRunning = false;
                
                //update blend tree to determing walking/running animation
                float animationSpeedPercent = ((isRunning) ? 1 : 0.5f) * move.magnitude;
                animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
            }
        }

        if (hasDoubleJump)
        {
            if (Time.time - doubleJumpStartTime >= doubleJumpDuration)
            {
                hasDoubleJump = false;
            }
        }

    }

    //first frame of jump
    void Jump()
    {
        //send player upwards
        rb.velocity = rb.velocity + Vector3.up * jumpVelocity;

        //trigger jump animation
        animator.SetBool("isJumping", true);

        //prevent repeated jumps in air
        jumpButtonPressed = false;

        jumpsLeft -= 1;
    }

    //first frame of hover
    void startHover()
    {
        //slow player's descent
        rb.velocity = new Vector3(0, -1.0f, 0);

        //prevent multiple hover triggers at once
        canStartHover = false;

        //update hover status
        isHovering = true;

        //record start time of hover for time limit
        hoverStartTime = Time.time;
    }

    void startPunch()
    {
        animator.SetBool("isPunching", true);
        freezeWalking = true;
        freezeJumping = true;
        isPunching = false;
    }

    //function for activating punch hitbox
    //triggered by animation event
    public void punchDamageActivate()
    {
        punchColl.enabled = true;
    }

    //function for ending punch
    //triggered by animation event
    public void endPunch()
    {
        animator.SetBool("isPunching", false);
        freezeWalking = false;
        freezeJumping = false;
        punchColl.enabled = false;
    }

    public void speedPowerUp()
    {
        isSpeedBoosted = true;
        speedBoostStartTime = Time.time;
    }

    void pauseGame()
    {
        if (gamePaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void triggerPunchEffect()
    {
        punchParticles.Play();
    }

    public void doubleJumpPowerUp()
    {
        hasDoubleJump = true;
        doubleJumpStartTime = Time.time;
    }

}
