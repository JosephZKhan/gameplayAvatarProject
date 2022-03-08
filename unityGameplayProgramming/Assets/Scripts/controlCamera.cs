using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class controlCamera : MonoBehaviour
{
    CameraControls controls;
    Vector2 move;

    float yaw;
    float pitch;

    public float sensitivity = 10;
    public bool isInverted = false;

    [SerializeField] Transform target;
    public float targetDistance = 5;
    Vector2 pitchLimits = new Vector2(-20, 55);

    public float smoothTime = .12f;
    Vector3 smoothVelocity;

    Vector3 currentRotation;
    Vector3 currentPosition;

    public float centreTime = .06f;
    Vector3 centreVelocity;

    bool centreCameraButtonPressed = false;


    void Awake()
    {
        controls = new CameraControls();

        controls.Camera.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Camera.Move.canceled += ctx => move = Vector2.zero;
        controls.Camera.Move.performed += ctx => centreCameraButtonPressed = false;

        controls.Camera.Centre.started += ctx => centreCameraButtonPressed = true;
        controls.Camera.Centre.performed += ctx => centreCameraButtonPressed = true;
        //controls.Camera.Centre.canceled += ctx => centreCameraButtonPressed = false;
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
        //transform.position = target.position - target.transform.forward * targetDistance; you could use this for a pov mode

        if (centreCameraButtonPressed)
        {
            centreCamera(target);
        }
        else
        {
            currentPosition = transform.position;
        }
        
        void centreCamera(Transform centreTarget)
        {
            Vector3 newTargetPos = centreTarget.position - centreTarget.transform.forward * targetDistance;
            //transform.position = newTargetPos;

            currentPosition = Vector3.SmoothDamp(currentPosition, newTargetPos, ref centreVelocity, centreTime);
            transform.position = currentPosition;

            transform.LookAt(centreTarget);

            pitch = 0;
            yaw = 0;
            currentRotation = new Vector3(0, 0, 0);
            smoothVelocity = new Vector3(0, 0, 0);
        }
    }
}
