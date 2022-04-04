using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using PathCreation;

public class controlCamera : MonoBehaviour
{
    CameraControls controls;
    Vector2 move;

    float yaw;
    float pitch;

    public float sensitivity = .75f;
    public float povSensitivity = .25f;

    public bool isInverted = false;

    [SerializeField] Transform mainTarget;
    [SerializeField] Transform povTarget;
    [SerializeField] Text ringCount;

    public float targetDistance = 5;
    Vector2 pitchLimits = new Vector2(-20, 55);

    Vector2 povPitchLimits = new Vector2(-40, 40);

    float zoom;

    Vector2 zoomLimits = new Vector2(25, 100);

    public float smoothTime = .12f;
    Vector3 smoothVelocity;

    Vector3 currentRotation;
    Vector3 currentPosition;

    bool freeMovement = true;
    bool inpov = false;
    bool inLockOn = false;
    bool canLockOn = false;

    bool povButtonPressed = false;
    bool lockOnButtonPressed = false;

    GameObject lockOnTarget;
    public float lockOnTargetDistance = 7;

    Camera camera;

    bool zoomIn;
    bool zoomOut;

    public float zoomSpeed = 1;

    //public PathCreator pathCreator;

    public bool onSpline = false;
    public float splinePathPoint;

    int splineFlipDir = 1;



    void Awake()
    {
        controls = new CameraControls();

        controls.Camera.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Camera.Move.canceled += ctx => move = Vector2.zero;

        controls.Camera.Centre.started += ctx => resetPos(mainTarget);

        controls.Camera.SnapLeft.started += ctx => resetPosLeft(mainTarget);

        controls.Camera.SnapRight.started += ctx => resetPosRight(mainTarget);

        controls.Camera.SnapUp.started += ctx => resetPosUp(mainTarget);

        controls.Camera.POV.started += ctx => inpov = true;

        controls.Camera.POV.canceled += ctx => endPovMode();

        controls.Camera.LockOn.performed += ctx => inLockOn = true;

        controls.Camera.LockOn.canceled += ctx => endLockOnMode();

        controls.Camera.ZoomIn.performed += ctx => zoomIn = true;
        controls.Camera.ZoomIn.canceled += ctx => zoomIn = false;

        controls.Camera.ZoomOut.performed += ctx => zoomOut = true;
        controls.Camera.ZoomOut.canceled += ctx => zoomOut = false;

        camera = GetComponent<Camera>();
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
            if (!canLockOn)
            {
                inLockOn = false;
                freeMovement = true;
            }
            else
            {
                if (lockOnTarget != null)
                {
                    lockOnMode(lockOnTarget.transform);
                }
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

        if (onSpline)
        {
            resetPos(mainTarget);
            splineMode(mainTarget);
        }

        /*if (povButtonPressed)
        {
            if (canPov)
            {
                povMode(povTarget);
            }
        }*/
        
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

    public void resetPos(Transform target)
    {
        yaw = target.transform.eulerAngles.y;
/*
        float newYaw;
        newYaw = Mathf.Clamp(yaw, (yaw - (yaw % 360)), (yaw + (yaw % 360)));

        

        newYaw += target.transform.eulerAngles.y;
        Debug.Log(yaw);

        yaw = newYaw;*/

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

    void resetPosUp(Transform target)
    {
        yaw = target.transform.eulerAngles.y;
        pitch = pitchLimits.y;
        if (inpov)
        {
            pitch = povPitchLimits.y;
        }
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

        if (zoomIn)
        {
            zoom = zoom + zoomSpeed;
            //Debug.Log("zoom in");
        }
        if (zoomOut)
        {
            zoom = zoom - zoomSpeed;
            //Debug.Log("zoom out");
        }
        zoom = Mathf.Clamp(zoom, zoomLimits.x, zoomLimits.y);
        camera.fieldOfView = zoom;

        freeMovement = false;
        ringCount.gameObject.SetActive(false);

    }

    void endPovMode()
    {
        if (inpov)
        {
            ringCount.gameObject.SetActive(true);
            freeMovement = true;
            inpov = false;
            resetPos(mainTarget);

            zoom = 60;
            camera.fieldOfView = zoom;
        }
        
    }

    public void assignLockOnTarget(GameObject newLockOnTarget)
    {
        canLockOn = true;
        lockOnTarget = newLockOnTarget;
    }

    public void disableLockOn()
    {
        canLockOn = false;
    }

    void lockOnMode(Transform target)
    {

        inpov = false;
        freeMovement = false;

        float playerDistFromTarget = Vector3.Distance(mainTarget.transform.position, target.transform.position);
        playerDistFromTarget = playerDistFromTarget * 0.25f;

        Vector3 newPosition = target.position - (mainTarget.transform.forward * lockOnTargetDistance * playerDistFromTarget) + (mainTarget.transform.up * 5);
        currentPosition = Vector3.SmoothDamp(currentPosition, newPosition, ref smoothVelocity, smoothTime);

        transform.position = currentPosition;
        transform.LookAt(target);
    }

    void endLockOnMode()
    {
        if (inLockOn)
        {
            inLockOn = false;
            freeMovement = true;
            resetPos(mainTarget);
        }
    }

    public void setSplinePoint(float newPoint)
    {
        splinePathPoint = newPoint;
    }

    public void setOnSpline(bool newOnSpline)
    {
        onSpline = newOnSpline;

        if (!onSpline)
        {
            freeMovement = true;
        }
    }

    public void setSplineFlipDir(int newFlipDir)
    {
        splineFlipDir = newFlipDir;
    }

    void splineMode(Transform target)
    {
        freeMovement = false;
        inpov = false;
        canLockOn = false;
        
        Vector3 newPos = target.position + target.transform.right * (targetDistance * 3) * splineFlipDir;
        newPos.y = target.position.y;
        StartCoroutine (moveToPoint(newPos));

        transform.LookAt(target);

    }

    IEnumerator moveToPoint(Vector3 endPos)
    {
        float elapsedTime = 0;
        float waitTime = .2f;

        currentPosition = transform.position;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPosition, endPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = endPos;
        yield return null;
    }
}
