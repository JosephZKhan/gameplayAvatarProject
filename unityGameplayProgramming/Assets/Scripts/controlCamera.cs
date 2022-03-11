using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class controlCamera : MonoBehaviour
{
    CameraControls controls;
    Vector2 move;

    public float yaw;
    public float pitch;

    public float sensitivity = .75f;
    public float povSensitivity = .25f;

    public bool isInverted = false;

    [SerializeField] Transform mainTarget;
    [SerializeField] Transform povTarget;
    [SerializeField] playerController2 playerScriptRef;

    public float targetDistance = 5;
    Vector2 pitchLimits = new Vector2(-20, 55);

    Vector2 povPitchLimits = new Vector2(-40, 40);

    public float smoothTime = .12f;
    Vector3 smoothVelocity;

    Vector3 currentRotation;
    Vector3 currentPosition;

    bool freeMovement = true;
    bool inpov = false;
    bool inLockOn = false;

    GameObject lockOnTarget;


    void Awake()
    {
        controls = new CameraControls();

        controls.Camera.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Camera.Move.canceled += ctx => move = Vector2.zero;

        controls.Camera.Centre.started += ctx => resetPos(mainTarget);

        controls.Camera.SnapLeft.started += ctx => resetPosLeft(mainTarget);

        controls.Camera.SnapRight.started += ctx => resetPosRight(mainTarget);

        controls.Camera.POV.started += ctx => inpov = true;
        controls.Camera.POV.started += ctx => freeMovement = false;
        //controls.Camera.POV.started += ctx => resetPos(povTarget);

        controls.Camera.POV.canceled += ctx => inpov = false;
        controls.Camera.POV.canceled += ctx => freeMovement = true;

        controls.Camera.LockOn.started += ctx => inLockOn = true;
        controls.Camera.LockOn.started += ctx => inpov = false;
        controls.Camera.LockOn.started += ctx => freeMovement = false;

        controls.Camera.LockOn.canceled += ctx => inLockOn = false;
        controls.Camera.LockOn.canceled += ctx => freeMovement = true;
    }

    private void OnEnable()
    {
        controls.Camera.Enable();
    }

    private void OnDisable()
    {
        controls.Camera.Disable();
    }

    void LateUpdate()
    {
        if (inLockOn)
        {
            lockOnTarget = playerScriptRef.getLockOnTarget();
            if (lockOnTarget == null)
            {
                inLockOn = false;
                freeMovement = true;
            }
            else
            {
                Debug.Log(lockOnTarget);
            }
        }
        if (freeMovement)
        {
            freeOrbitMode(mainTarget);
        }
        if (inpov)
        {
            povMode(povTarget);
        }
        
    }

    void freeOrbitMode(Transform target)
    {
        yaw += move.x * sensitivity;
        if (isInverted)
        {
            pitch += move.y * sensitivity;
        }
        else
        {
            pitch -= move.y * sensitivity;
        }
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref smoothVelocity, smoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * targetDistance;

        currentPosition = transform.position;
    }

    void resetPos(Transform target)
    {
        yaw = target.transform.eulerAngles.y;
        pitch = 0;
    }

    void resetPosLeft(Transform target)
    {
        yaw = target.transform.eulerAngles.y + 90;
        pitch = 0;
    }

    void resetPosRight(Transform target)
    {
        yaw = target.transform.eulerAngles.y - 90;
        pitch = 0;
    }

    void povMode(Transform target)
    {

        Vector2 yawLimits = new Vector2(target.transform.eulerAngles.y - 40, target.transform.eulerAngles.y + 40);

        yaw += move.x * povSensitivity;
        if (isInverted)
        {
            pitch += move.y * povSensitivity;
        }
        else
        {
            pitch -= move.y * povSensitivity;
        }

        yaw = Mathf.Clamp(yaw, yawLimits.x, yawLimits.y);
        pitch = Mathf.Clamp(pitch, povPitchLimits.x, povPitchLimits.y);

        transform.eulerAngles = new Vector3(pitch, yaw);

        //currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref smoothVelocity, smoothTime);*/

        //transform.eulerAngles = currentRotation;

        currentPosition = Vector3.SmoothDamp(currentPosition, target.transform.position, ref smoothVelocity, smoothTime);
        transform.position = currentPosition;
    }
}
