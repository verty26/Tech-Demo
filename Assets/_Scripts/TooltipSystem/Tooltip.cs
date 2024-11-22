using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;

    public LayoutElement layoutElement;
    public int characterWrapLimit;

    public RectTransform rectTransform;

    public Vector2 offset; // Add an offset field

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerText.gameObject.SetActive(false);
        }
        else
        {
            headerText.gameObject.SetActive(true);
            headerText.text = header;
        }

        contentText.text = content;

        int headerLength = headerText.text.Length;
        int contentLength = contentText.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerText.text.Length;
            int contentLength = contentText.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
        }

        Vector2 mousePos = Input.mousePosition;

        // Adjust the pivot dynamically but constrain it for better usability
        float pivotX = mousePos.x < Screen.width / 2 ? 0f : 1f; // Left or right of the mouse
        float pivotY = mousePos.y < Screen.height / 2 ? 0f : 1f; // Above or below the mouse

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        // Set the tooltip position, considering the pivot
        rectTransform.position = mousePos + offset;
    }


}
