using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float moveInput;
    private float turnInput;
    private bool isGrounded;

    [Header("Car Settings")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float reverseSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float nitrousMultiplier = 2f;
    [SerializeField] private float airDrag;
    [SerializeField] private float groundDrag;

    [SerializeField] private LayerMask groundLayer;

    [Header("Referances")]
    [SerializeField] private Rigidbody sphereRB;
    [SerializeField] private Rigidbody carRB;

    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    private float steeringAngle;
    private float targetSteeringAngle;

    private float currentSpeed;

    
    private bool isNitrousActive;

    [Header("Camera Settings")]
    [SerializeField] private float minFOV = 60f;
    [SerializeField] private float maxFOV = 75f;
    [SerializeField] private float fovSmoothSpeed = 5f;
    private Camera mainCamera;

    private void Start()
    {
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //Getting Inputs
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        //Nitrous
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isNitrousActive = true;
        }
        else
        {
            isNitrousActive = false;
        }

        float speedFactor = isNitrousActive ? nitrousMultiplier : 1f;

        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;
        moveInput *= speedFactor;

        transform.position = sphereRB.transform.position;

        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        transform.Rotate(0, newRotation, 0, Space.World);

        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        if (isGrounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            sphereRB.drag = groundDrag;
        }
        else
        {
            sphereRB.drag = airDrag;
        }

        targetSteeringAngle = turnInput * 30f;
        steeringAngle = Mathf.Lerp(steeringAngle, targetSteeringAngle, Time.deltaTime * 10f);

        float targetFOV = isNitrousActive ? maxFOV : minFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovSmoothSpeed);

        RotateWheels();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCarPosition();
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);

            if (currentSpeed > 0)
            {
                sphereRB.velocity = sphereRB.velocity * 0.98f;
            }
        }
        else
        {
            sphereRB.AddForce(transform.up * -30f);
        }

        carRB.MoveRotation(transform.rotation);
    }

    private void RotateWheels()
    {
        float wheelRollingSpeed = moveInput * Time.deltaTime * 360f;

        frontLeftWheel.Rotate(Vector3.right, wheelRollingSpeed, Space.Self);
        frontRightWheel.Rotate(Vector3.right, wheelRollingSpeed, Space.Self);
        rearLeftWheel.Rotate(Vector3.right, wheelRollingSpeed, Space.Self);
        rearRightWheel.Rotate(Vector3.right, wheelRollingSpeed, Space.Self);

        steeringAngle = Mathf.Clamp(steeringAngle, -30f, 30f);

        Vector3 frontLeftEuler = frontLeftWheel.localEulerAngles;
        frontLeftEuler.y = steeringAngle;
        frontLeftEuler.z = 0f;
        frontLeftWheel.localEulerAngles = frontLeftEuler;

        Vector3 frontRightEuler = frontRightWheel.localEulerAngles;
        frontRightEuler.y = steeringAngle;
        frontRightEuler.z = 0f;
        frontRightWheel.localEulerAngles = frontRightEuler;

        Vector3 rearLeftEuler = rearLeftWheel.localEulerAngles;
        rearLeftEuler.y = 0f;
        rearLeftEuler.z = 0f;
        rearLeftWheel.localEulerAngles = rearLeftEuler;

        Vector3 rearRightEuler = rearRightWheel.localEulerAngles;
        rearRightEuler.y = 0f;
        rearRightEuler.z = 0f;
        rearRightWheel.localEulerAngles = rearRightEuler;
    }

    private void ResetCarPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += 20f;
        transform.position = newPosition;

        sphereRB.velocity = Vector3.zero;
        sphereRB.angularVelocity = Vector3.zero;

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
