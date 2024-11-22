using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float movementSmoothness = 0.1f;
    public float sprintMultiplier = 1.5f;
    public bool canSprint = true;

    [Header("Key Bindings")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.C;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public bool canJump = true;

    [Header("Head Bob Settings")]
    public bool enableHeadBob = true;
    public float headBobSpeed = 10f;
    public float headBobAmount = 0.1f;

    [Header("Crouch Settings")]
    public bool canCrouch = true;
    public float crouchSpeedMultiplier = 0.5f;
    public float crouchHeight = 0.5f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Sprint FOV Settings")]
    public float normalFOV = 60f;
    public float sprintFOV = 75f;
    public float fovTransitionSpeed = 10f;

    private CharacterController characterController;
    private Vector3 originalCameraPosition;
    private bool isCrouching = false;
    private bool isSprinting = false;
    private Vector3 moveDirection;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    private float verticalVelocity;

    private Transform cameraTransform;
    private Camera mainCamera;
    private float xRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        mainCamera = Camera.main;
        originalCameraPosition = cameraTransform.localPosition;

        // Lock the cursor to the game window
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        // Set the initial FOV
        mainCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        if (canJump) HandleJump();
        if (enableHeadBob) HandleHeadBob();
        if (canCrouch) HandleCrouch();
        UpdateFOV();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Normalize movement direction and ensure zero vector when there's no input
        moveDirection = move.magnitude > 0 ? move.normalized : Vector3.zero;

        // Smooth the movement direction
        moveDirection = Vector3.Lerp(moveDirection, move, movementSmoothness);

        // Sprinting logic
        float currentSpeed = moveSpeed;
        if (canSprint && Input.GetKey(sprintKey) && moveDirection != Vector3.zero)
        {
            currentSpeed *= sprintMultiplier;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (isCrouching)
        {
            currentSpeed *= crouchSpeedMultiplier;
        }

        // Apply movement
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Apply gravity
        if (characterController.isGrounded)
        {
            verticalVelocity = -2f; // Small downward force to keep grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }


    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotation
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player body left and right
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && characterController.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    void HandleHeadBob()
    {
        if (moveDirection.magnitude > 0 && characterController.isGrounded)
        {
            float bobOffset = Mathf.Sin(Time.time * headBobSpeed) * headBobAmount;
            cameraTransform.localPosition = originalCameraPosition + new Vector3(0, bobOffset, 0);
        }
        else
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originalCameraPosition, Time.deltaTime * headBobSpeed);
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching;
            characterController.height = isCrouching ? crouchHeight : 2f; // Assuming default height is 2
        }
    }

    void UpdateFOV()
    {
        // Transition FOV based on whether sprinting
        float targetFOV = isSprinting ? sprintFOV : normalFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
    }
}
