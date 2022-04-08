using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PathCreation;
using UnityEngine.SceneManagement;

public class playerController2 : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;
    Vector2 moveDirection;

    public float walkSpeed = 150;
    public float runSpeed = 300;
    public float strafeSpeed = 80;

    public float turnSmoothTime = 0.2f;
    float currentTurnSmoothTime;
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
    SphereCollider lockOnColl;

    bool isRunning;

    bool hoverButtonPressed;
    bool isHovering;
    bool canStartHover;
    public float hoverTimeSeconds = 5.0f;

    float hoverStartTime;

    bool isPunching;

    bool freezeWalking;
    bool freezeJumping;
    bool freezePunching;

    bool isSpeedBoosted;
    public float speedBoostMagnitude = 2.0f;

    float distanceToGround;

    public float speedBoostDuration = 5.0f;
    float speedBoostStartTime;

    public static bool gamePaused;
    bool pauseButtonPressed;


    int jumpsLeft = 1;

    bool hasDoubleJump;
    public float doubleJumpDuration = 5.0f;
    float doubleJumpStartTime;

    [SerializeField] fillingMeterScript hoverMeter;
    [SerializeField] fillingMeterScript doubleJumpMeter;
    [SerializeField] fillingMeterScript speedBoostMeter;
    [SerializeField] fillingMeterScript superPunchMeter;

    [SerializeField] ParticleSystem punchParticles;
    [SerializeField] ParticleSystem doubleJumpParticles;
    [SerializeField] ParticleSystem speedBoostParticles;
    [SerializeField] ParticleSystem superPunchParticles;

    int ringCount = 0;

    [SerializeField] ringCounterUI ringUI;
    [SerializeField] healthCounterUI healthUI;

    public bool hasSuperPunch;

    public float superPunchDuration = 5.0f;
    float superPunchStartTime;

    float doubleJumpParticleRate;
    float speedBoostParticleRate;
    float superPunchParticleRate;

    bool inSwitchCollider = false;
    switchBehaviour targetSwitch;

    [SerializeField] Transform cameraTransform;
    [SerializeField] controlCamera cameraScriptRef;

    bool canLockOn = false;
    GameObject lockOnTarget;

    bool lockOnButtonPressed = false;
    bool isLockedOn = false;

    PathCreator pathCreator;

    public bool onSpline = false;
    public float splinePathPoint;
    public EndOfPathInstruction end;

    bool canMoveForward = true;
    bool canMoveBackward = true;

    int health = 5;

    public bool hasMercyInvincibility = false;

    bool isDead = false;

    




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

        controls.Player.LockOn.started += ctx => lockOnButtonPressed = true;
        controls.Player.LockOn.canceled += ctx => lockOnButtonPressed = false;

        controls.Player.Freeze.started += ctx => freezePlayer();
        controls.Player.Freeze.canceled += ctx => unfreezePlayer();

        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        coll = GetComponent<CapsuleCollider>();
        punchColl = GetComponent<BoxCollider>();
        lockOnColl = GetComponent<SphereCollider>();

        punchColl.enabled = false;

        distanceToGround = coll.bounds.extents.y - 1.25f;

        doubleJumpParticles.gameObject.SetActive(false);

        speedBoostParticles.gameObject.SetActive(false);

        superPunchParticles.gameObject.SetActive(false);

        hoverMeter.setMaxValue(hoverTimeSeconds);

        doubleJumpMeter.setMaxValue(doubleJumpDuration);

        speedBoostMeter.setMaxValue(speedBoostDuration);

        superPunchMeter.setMaxValue(superPunchDuration);

        doubleJumpParticleRate = doubleJumpParticles.emissionRate;
        speedBoostParticleRate = speedBoostParticles.emissionRate;
        superPunchParticleRate = superPunchParticles.emissionRate;

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
        cameraScriptRef.setOnSpline(onSpline);

        if (onSpline)
        {
            if (pathCreator != null)
            {
                splineMovement();
            }

            else
            {
                onSpline = false;
            }

        }

        else
        {
            currentTurnSmoothTime = turnSmoothTime;

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
                //Vector3 movement = new Vector3(move.x, 0f, move.y) * speed * Time.deltaTime;
                //Vector3 movement = transform.forward * speed * Time.deltaTime;
                Vector3 movement = new Vector3();

                if (isLockedOn && lockOnTarget != null)
                {
                    if (move.y > 0 && Mathf.Abs(move.x) < 0.3)
                    {
                        movement = transform.forward * speed * Time.deltaTime;
                    }
                    if (move.y < 0 && Mathf.Abs(move.x) < 0.3)
                    {
                        if (Vector3.Distance(transform.position, lockOnTarget.transform.position) < lockOnColl.radius)
                        {
                            movement = -transform.forward * speed * Time.deltaTime;
                        }
                    }
                    if (move.x > 0 && Mathf.Abs(move.y) < 0.3)
                    {
                        movement = transform.right * strafeSpeed * Time.deltaTime;
                    }
                    if (move.x < 0 && Mathf.Abs(move.y) < 0.3)
                    {
                        movement = -transform.right * strafeSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    movement = transform.forward * speed * Time.deltaTime;
                }

                rb.AddForce(movement, ForceMode.VelocityChange);

                //normalize movement for rotation
                Vector2 moveDirection = move.normalized;

                //if the user moves player
                if (moveDirection != Vector2.zero)
                {
                    //rotate the player to face forwards
                    float targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, currentTurnSmoothTime);
                }

                //update blend tree to determing walking/running animation
                float animationSpeedPercent = ((isRunning) ? 1 : 0.5f) * move.magnitude;
                animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
            }
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
                hoverMeter.setValue(hoverTimeSeconds);
                //hoverMeter.despawnBackground();
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

            hoverMeter.setValue(Time.time - hoverStartTime);

        }
        else
        {
            hoverMeter.despawnBackground();
            hoverMeter.setValue(5.0f);
        }

        //punch if punch button pressed/player not airborne
        if (isPunching && isGrounded)
        {
            if (inSwitchCollider)
            {
                pressSwitch();
            }
            else
            {
                if (!freezePunching)
                {
                    startPunch();
                }
            }
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
            speedBoostMeter.setValue(Time.time - speedBoostStartTime);
            var emission = speedBoostParticles.emission;
            emission.rateOverTime = speedBoostParticleRate * (1 - ((Time.time - speedBoostStartTime) / speedBoostDuration));
        }
        else
        {
            speedBoostMeter.despawnBackground();
            speedBoostParticles.gameObject.SetActive(false);
            var emission = speedBoostParticles.emission;
            emission.rateOverTime = speedBoostParticleRate;
        }

        if (hasDoubleJump)
        {
            if (Time.time - doubleJumpStartTime >= doubleJumpDuration)
            {
                hasDoubleJump = false;
            }
            doubleJumpMeter.setValue(Time.time - doubleJumpStartTime);
            var emission = doubleJumpParticles.emission;
            emission.rateOverTime = doubleJumpParticleRate * (1 - ((Time.time - doubleJumpStartTime) / doubleJumpDuration));
        }
        else
        {
            doubleJumpMeter.despawnBackground();
            doubleJumpParticles.gameObject.SetActive(false);
            var emission = doubleJumpParticles.emission;
            emission.rateOverTime = doubleJumpParticleRate;
        }

        if (hasSuperPunch)
        {
            if (Time.time - superPunchStartTime >= superPunchDuration)
            {
                hasSuperPunch = false;
                animator.SetBool("hasSuperPunch", false);
                punchParticles.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            superPunchMeter.setValue(Time.time - superPunchStartTime);
            var emission = superPunchParticles.emission;
            emission.rateOverTime = superPunchParticleRate * (1 - ((Time.time - superPunchStartTime) / superPunchDuration));

        }
        else
        {
            superPunchMeter.despawnBackground();
            superPunchParticles.gameObject.SetActive(false);
            var emission = superPunchParticles.emission;
            emission.rateOverTime = superPunchParticleRate;
        }

        if (lockOnButtonPressed)
        {
            if (canLockOn && lockOnTarget != null)
            {
                lockOn();
            }
        }
        else
        {
            isLockedOn = false;
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

        //spawn hover meter background
        hoverMeter.spawnBackground();
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
        speedBoostMeter.spawnBackground();
        speedBoostParticles.gameObject.SetActive(true);
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
        //doubleJumpMeter.setValue(doubleJumpDuration);
        doubleJumpMeter.spawnBackground();
        doubleJumpParticles.gameObject.SetActive(true);
    }

    public void collectRing()
    {
        ringCount++;
        ringUI.setCounter(ringCount);
    }

    public void superPunchPowerUp()
    {
        hasSuperPunch = true;
        superPunchStartTime = Time.time;
        superPunchMeter.spawnBackground();
        animator.SetBool("hasSuperPunch", true);
        superPunchParticles.gameObject.SetActive(true);
        punchParticles.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
    }

    public void setInSwitchCollider(bool newInSwitchCollider)
    {
        inSwitchCollider = newInSwitchCollider;
    }

    public void setTargetSwitch(switchBehaviour newTargetSwitch)
    {
        targetSwitch = newTargetSwitch;
    }

    void pressSwitch()
    {
        targetSwitch.Activate();

        isPunching = false;
    }

    public void freezePlayer()
    {
        freezeJumping = true;
        freezeWalking = true;
        freezePunching = true;
    }

    public void unfreezePlayer()
    {
        freezeJumping = false;
        freezeWalking = false;
        freezePunching = false;
    }

    public void assignLockOnTarget(GameObject newLockOnTarget)
    {
        lockOnTarget = newLockOnTarget;
        canLockOn = true;
    }

    public GameObject getLockOnTarget()
    {
        if (lockOnTarget != null)
        {
            return lockOnTarget;
        }
        else
        {
            return null;
        }
        
    }

    public void disableLockOn()
    {
        canLockOn = false;
    }

    void lockOn()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Vector3 adjustedPos = lockOnTarget.transform.position;
        adjustedPos.y = transform.position.y;
        transform.LookAt(adjustedPos);
        isLockedOn = true;
        //transform.rotation.SetEulerAngles(new Vector3(0.0f, transform.rotation.y, 0.0f));
    }

    public bool getIsLockedOn()
    {
        return isLockedOn;
    }

    void splineMovement()
    {
        currentTurnSmoothTime = 0;
        move.x = 0;
        canLockOn = false;


        if (move.y > 0 && !canMoveForward)
        {
            move.y = 0;
        }

        if (move.y < 0 && !canMoveBackward)
        {
            move.y = 0;
        }

        //use isRunning bool to set movement speed. scaled with move magnitude
        float speed = ((isRunning && isGrounded) ? runSpeed : walkSpeed) * move.magnitude;

        if (isSpeedBoosted)
        {
            speed = speed * speedBoostMagnitude;
            isRunning = true;
        }

        if (!freezeWalking)
        {
            splinePathPoint += (move.y * speed / 15 * Time.deltaTime);
        }

        

        Vector3 playerPathPos = new Vector3(pathCreator.path.GetPointAtDistance(splinePathPoint, end).x, transform.position.y, pathCreator.path.GetPointAtDistance(splinePathPoint).z);
        transform.position = playerPathPos;


        //transform.position = pathCreator.path.GetPointAtDistance(splinePathPoint);

        if (move.y > 0)
        {
            transform.eulerAngles = new Vector3(0, pathCreator.path.GetRotationAtDistance(splinePathPoint, end).eulerAngles.y, 0);
            cameraScriptRef.setSplineFlipDir(1);
        }

        if (move.y < 0)
        {
            transform.eulerAngles = new Vector3(0, pathCreator.path.GetRotationAtDistance(splinePathPoint, end).eulerAngles.y - 180, 0);
            cameraScriptRef.setSplineFlipDir(-1);
        }

        /*Vector3 movement = new Vector3();
        movement = transform.forward * speed * Time.deltaTime;
        rb.AddForce(movement, ForceMode.VelocityChange);*/



        //update blend tree to determing walking/running animation
        float animationSpeedPercent = ((isRunning) ? 1 : 0.5f) * move.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        //cameraScriptRef.setSplinePoint(splinePathPoint);
    }

    public void setOnSpline(bool newOnSpline)
    {
        onSpline = newOnSpline;
        /*if (!onSpline)
        {
            splinePathPoint = 0;
        }*/
    }

    public void setTargetSpline(PathCreator newTargetSpline)
    {
        pathCreator = newTargetSpline;
        Debug.Log(pathCreator);
    }

    public void setSplinePoint(float newSplinePoint)
    {
        splinePathPoint = newSplinePoint;
    }

    public PathCreator getTargetSpline()
    {
        return pathCreator;
    }

    public Vector2 getMovement()
    {
        return move;
    }

    public void setCanMoveForward(bool newBool)
    {
        canMoveForward = newBool;
    }

    public void setCanMoveBackward(bool newBool)
    {
        canMoveBackward = newBool;
    }

    public void takeDamage(int damageAmount, Transform hazardPos, int knockbackStrength)
    {
        if (!hasMercyInvincibility)
        {
            health -= damageAmount;
            healthUI.setCounter(health);
            //Debug.Log("oof! " + health);
        
            if (health <= 0)
            {
                if (!isDead)
                {
                    die();
                }
            }
            else
            {
                Transform lookPos = hazardPos;
                lookPos.position = new Vector3(hazardPos.position.x, transform.position.y, hazardPos.position.z);
                transform.LookAt(lookPos);
                rb.AddForce(-transform.forward * knockbackStrength, ForceMode.VelocityChange);
                animator.SetBool("isHit", true);
                freezePlayer();

                StartCoroutine(mercyInvincibility());
            }

        }
        
    }

    void die()
    {
        isDead = true;
        //Debug.Log("you died!");
        animator.SetBool("isDead", true);
        freezePlayer();
        StartCoroutine(respawn());
        
    }

    public void endHit()
    {
        //Debug.Log("end hit");
        animator.SetBool("isHit", false);
        unfreezePlayer();
    }

    IEnumerator mercyInvincibility()
    {
        hasMercyInvincibility = true;
        yield return new WaitForSeconds(3.0f);
        hasMercyInvincibility = false;
    }

    IEnumerator respawn()
    {
        Debug.Log("respawning...");
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
