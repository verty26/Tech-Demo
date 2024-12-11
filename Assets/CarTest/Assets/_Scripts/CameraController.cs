using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // The target to follow (e.g., car)
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // Default offset
    [SerializeField] private float followSpeed = 10f; // How quickly the camera follows the target
    [SerializeField] private float rotationSmoothness = 5f; // How smoothly the camera rotates

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position based on the target's rotation and offset
        Vector3 rotatedOffset = target.TransformDirection(offset);
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smoothly interpolate the camera's position to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Smoothly rotate the camera to always face the back of the car
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothness * Time.deltaTime);
    }
}
