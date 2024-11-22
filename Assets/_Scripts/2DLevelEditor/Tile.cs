using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Change color immediately when clicked
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            StartCoroutine(SmoothColorChange(PixelLevelEditor.instance.currentColor, 0.25f));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change color on hover while left mouse button is held
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(SmoothColorChange(PixelLevelEditor.instance.currentColor, 0.25f));
        }
    }

    private IEnumerator SmoothColorChange(Color targetColor, float duration)
    {
        Color startColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;
    }
}
