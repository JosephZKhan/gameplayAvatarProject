using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController2 : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;
    Vector2 moveDirection;

    public float speed = 10;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    bool isJumping;
    public float jumpVelocity = 5;
    public float gravityScale = 1.0f;

    public static float globalGravity = -9.81f;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;

    Animator animator;

    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        //controls.Player.Move.performed += ctx => SendMessage(ctx.ReadValue<Vector2>());
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Jump.performed += ctx => isJumping = true;
        controls.Player.Jump.canceled += ctx => isJumping = false;
 
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(move.x, 0.0f, move.y) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        Vector2 moveDirection = move.normalized;

        if (moveDirection != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2 (moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        float animationSpeedPercent = moveDirection.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);

        if (isJumping)
        {
            Jump();
        }

    }

    void Jump()
    {
        Debug.Log("jumped");
        rb.velocity = rb.velocity + Vector3.up * jumpVelocity;


        isJumping = false;
    }
}
