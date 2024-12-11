using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float countSpeed = 0.1f;  // Speed of the count animation

    public int number = 0;

    private bool isShaking = false;

    private void Start()
    {
        counterText.text = number.ToString();
    }

    public void IncreaseFirstButton()
    {
        number += 5;
        counterText.text = number.ToString();
    }

    public void IncreaseSecondButton()
    {
        number += 5;
        StartCoroutine(SmoothCountAnimation(number));

        if (!isShaking)
        {
            StartCoroutine(ShakeText());
        }
    }

    private IEnumerator ShakeText()
    {
        isShaking = true;

        Vector3 originalScale = counterText.transform.localScale;
        float duration = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            counterText.transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.2f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            counterText.transform.localScale = Vector3.Lerp(originalScale * 1.2f, originalScale * 0.8f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            counterText.transform.localScale = Vector3.Lerp(originalScale * 0.8f, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        counterText.transform.localScale = originalScale;

        isShaking = false;
    }

    private IEnumerator SmoothCountAnimation(int targetNumber)
    {
        int startNumber = number - 1;  // Start from the previous number
        float elapsedTime = 0f;

        while (elapsedTime < countSpeed)
        {
            number = Mathf.RoundToInt(Mathf.Lerp(startNumber, targetNumber, elapsedTime / countSpeed));
            counterText.text = number.ToString();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        number = targetNumber;
        counterText.text = number.ToString();
    }
}
