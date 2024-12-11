using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; // Speed of rotation based on mouse movement
    private float currentRotationY = 0f; // To keep track of the current rotation
    private float lastMouseX; // To store the last mouse X position

    // Start is called before the first frame update
    void Start()
    {
        // Store the initial position of the mouse when the game starts
        lastMouseX = Input.mousePosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the mouse is being dragged (pressed)
        if (Input.GetMouseButton(0)) // 0 is for left mouse button
        {
            // Get the difference in mouse movement (dragging) on the X-axis
            float mouseDeltaX = Input.mousePosition.x - lastMouseX;

            // Reverse the rotation direction by negating the mouseDeltaX
            currentRotationY -= mouseDeltaX * rotationSpeed * Time.deltaTime;

            // Apply the new rotation to the gun (around the Y-axis)
            transform.rotation = Quaternion.Euler(0f, currentRotationY, 0f);
        }

        // Update the last mouse position for the next frame
        lastMouseX = Input.mousePosition.x;
    }
}
