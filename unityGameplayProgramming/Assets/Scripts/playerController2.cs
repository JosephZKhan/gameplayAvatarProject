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

    bool isJumping;
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

    private LayerMask groundLayer;

    bool isRunning;

    bool hoverButtonPressed;
    bool isHovering;
    bool canStartHover;
    public float hoverTimeSeconds = 5.0f;

    float hoverStartTime;

    bool isPunching;

    bool freezeWalking;
    bool freezeJumping;


    

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        //controls.Player.Move.performed += ctx => SendMessage(ctx.ReadValue<Vector2>());
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Jump.performed += ctx => isJumping = true;
        controls.Player.Jump.canceled += ctx => isJumping = false;

        controls.Player.Run.performed += ctx => isRunning = true;
        controls.Player.Run.canceled += ctx => isRunning = false;

        controls.Player.Hover.performed += ctx => hoverButtonPressed = true;
        controls.Player.Hover.canceled += ctx => hoverButtonPressed = false;

        controls.Player.Punch.performed += ctx => isPunching = true;
        controls.Player.Punch.canceled += ctx => isPunching = false;
 
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        coll = GetComponent<CapsuleCollider>();
        punchColl = GetComponent<BoxCollider>();

        punchColl.enabled = false;

        groundLayer = LayerMask.NameToLayer("Walkable");

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

    void FixedUpdate()
    {
        //if player can walk
        if (!freezeWalking)
        {
            //use isRunning bool to set movement speed. scaled with move magnitude
            float speed = ((isRunning && isGrounded) ? runSpeed : walkSpeed) * move.magnitude;

            //update movement
            Vector3 movement = new Vector3(move.x, 0.0f, move.y) * speed * Time.deltaTime;
            transform.Translate(movement, Space.World);

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

        //start a jump if player is on ground/jump button pressed/jumping isn't frozen
        if (isJumping && isGrounded && !freezeJumping)
        {
            Jump();
        }

        //start a hover if hover button pressed/player is falling
        if (hoverButtonPressed && !isGrounded && rb.velocity.y < 0)
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

        //re-enable hovering when player lands
        if (isGrounded)
        {
            canStartHover = true;
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
            animator.SetBool("isPunching", true);
            freezeWalking = true;
            freezeJumping = true;
        }

    }


    void Jump()
    {
        rb.velocity = rb.velocity + Vector3.up * jumpVelocity;

        animator.SetBool("isJumping", true);

        isJumping = false;
    }

    void startHover()
    {
        rb.velocity = new Vector3(0, -1.0f, 0);
        canStartHover = false;
        isHovering = true;
        hoverStartTime = Time.time;
        //Debug.Log(hoverStartTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == groundLayer)
        {
            isGrounded = true;

            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.collider.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        }
    }

    public void punchDamageActivate()
    {
        punchColl.enabled = true;
        //Debug.Log("punch is lethal!");
    }

    public void endPunch()
    {
        animator.SetBool("isPunching", false);
        //animator.ResetTrigger("punchTrigger");
        freezeWalking = false;
        freezeJumping = false;
        punchColl.enabled = false;
    }
}
