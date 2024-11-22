using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    [SerializeField] private Vector2 itemSize;
    [SerializeField] private Vector2 detectionBoxSize;

    [SerializeField] private List<InventoryTile> collidingTiles = new List<InventoryTile>();

    private bool isDragging = false;

    [Header("References")]
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector3 defaultPosition;
    private Vector3 defaultRotation;

    private Vector2 actualDetectionBoxSize; // Store the original detection box size

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("The Item script requires the object to be under a Canvas.");
        }

        actualDetectionBoxSize = detectionBoxSize; // Store the original size of the detection box
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.05f);

        StartCoroutine(DropObject());
    }

    private void Update()
    {
        UpdateCollidingTiles();

        // Rotate the object while dragging when pressing R
        if (isDragging && Input.GetKeyDown(KeyCode.R))
        {
            RotateObject();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        isDragging = true;
        defaultPosition = transform.position;
        defaultRotation = transform.rotation.eulerAngles;

        foreach (InventoryTile tile in collidingTiles)
        {
            tile.IsOccupied = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        isDragging = false;
        StartCoroutine(DropObject());
    }

    private IEnumerator DropObject()
    {
        Vector3? targetPosition = CalculateMidPoint();

        if (targetPosition.HasValue)
        {
            transform.position = targetPosition.Value;
        }
        else
        {
            // Reset rotation to default if it's not already the default rotation
            if (transform.rotation.eulerAngles != defaultRotation)
            {
                transform.rotation = Quaternion.Euler(defaultRotation);
                UpdateDetectionBox(); // Update detection box to match the default rotation
            }

            transform.position = defaultPosition; // Reset position to defaultPosition
        }

        yield return new WaitForSeconds(0.2f);

        foreach (InventoryTile tile in collidingTiles)
        {
            tile.IsOccupied = true;
        }
    }

    private void UpdateCollidingTiles()
    {
        collidingTiles.Clear(); // Clear previous detections

        // Define the detection box center and size
        Vector2 boxCenter = (Vector2)transform.position;
        Vector2 boxSize = detectionBoxSize;

        // Use Physics2D.OverlapBoxAll to detect colliding objects (assuming 2D)
        Collider2D[] detectedColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0);

        // Loop through detected colliders and add InventoryTile components to the list
        foreach (var collider in detectedColliders)
        {
            InventoryTile tile = collider.GetComponent<InventoryTile>();
            if (tile != null && !collidingTiles.Contains(tile))
            {
                collidingTiles.Add(tile);
            }
        }
    }

    private Vector3? CalculateMidPoint()
    {
        // Return null if there are no colliding tiles
        if (collidingTiles.Count == 0)
            return null;

        // Return null if the number of colliding tiles doesn't match the item size
        if (itemSize.x * itemSize.y != collidingTiles.Count)
            return null;

        // Check if any tile is occupied; if so, return null
        if (collidingTiles.Any(tile => tile.IsOccupied))
            return null;

        // Calculate the average position of all colliding tiles
        Vector2 totalPosition = Vector2.zero;
        foreach (var tile in collidingTiles)
        {
            totalPosition += (Vector2)tile.transform.position;
        }

        Vector2 averagePosition = totalPosition / collidingTiles.Count;

        // Return the calculated midpoint as a Vector3
        return averagePosition;
    }

    private void RotateObject()
    {
        // Rotate the object by 90 degrees
        transform.Rotate(0, 0, 90);

        // Update the detection box size
        UpdateDetectionBox();
    }

    private void UpdateDetectionBox()
    {
        // Swap width and height of the detection box based on rotation
        if (Mathf.Abs(transform.rotation.eulerAngles.z % 180) > 0.1f) // Rotated at 90° or 270°
        {
            detectionBoxSize = new Vector2(actualDetectionBoxSize.y, actualDetectionBoxSize.x);
        }
        else // Rotated at 0° or 180°
        {
            detectionBoxSize = actualDetectionBoxSize; // Reset to original detection box size
        }
    }

    // Visualize the detection box in the Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan; // Color of the gizmo box
        Vector3 boxPosition = transform.position; // Center of the detection box
        Vector3 boxSize = new Vector3(detectionBoxSize.x, detectionBoxSize.y, 0); // Size of the box (z is 0 for 2D)

        Gizmos.DrawWireCube(boxPosition, boxSize);
    }
}
