using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelLevelEditor : MonoBehaviour
{
    public static PixelLevelEditor instance;

    public Color currentColor;

    public Transform cursor; // Reference to the cursor (UI Image)
    public Canvas canvas; // Reference to the parent Canvas

    private RectTransform cursorRectTransform;

    [SerializeField] private List<Tile> tiles = new List<Tile>();

    private void Awake()
    {
        instance = this;
        cursorRectTransform = cursor.GetComponent<RectTransform>();

        // Hide the system cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // Optional: keep the cursor within the game window
    }

    private void Update()
    {
        FollowMouse();
    }

    public void SetCurrentColor(Color color)
    {
        currentColor = color;
        cursor.GetComponent<Image>().color = color;
    }

    private void FollowMouse()
    {
        // Convert mouse position to screen space relative to the canvas
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            Input.mousePosition,
            canvas.worldCamera,
            out mousePosition
        );

        // Update the cursor position
        cursorRectTransform.localPosition = mousePosition;
    }

    private void OnDestroy()
    {
        // Restore the system cursor when the script is destroyed or the scene is exited
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClearCanvas()
    {
        foreach(Tile tile in tiles)
        {
            tile.GetComponent<Image>().color = Color.white;
        }
    }
}
