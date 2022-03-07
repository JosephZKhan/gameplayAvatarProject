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


    void Awake()
    {
        controls = new CameraControls();

        controls.Camera.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Camera.Move.canceled += ctx => move = Vector2.zero;

        controls.Camera.Centre.performed += ctx => centreCamera();
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
    }

    void centreCamera()
    {
        transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        transform.position = target.position - transform.forward * targetDistance;
        Debug.Log("bababooey");
    }

}
