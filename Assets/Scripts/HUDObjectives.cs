using UnityEngine;
using TMPro;
using System.Collections;

public class HUDObjectives : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI objectiveText;
    public CanvasGroup canvasGroup;

    private Coroutine blinkRoutine;

    /// <summary>
    /// Показывает сообщение message цветом color и, если blinking = true, плавно мигает текст.
    /// </summary>
    public void SetObjective(string message, Color color, bool blinking = false)
    {
        // Сброс предыдущего мигания
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
            SetTextAlpha(1f);
        }

        // Обновляем текст и фон
        objectiveText.text = message;
        objectiveText.color = color;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (blinking)
            blinkRoutine = StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        float duration = 1f; // 1 секунда на fade-out/ fade-in
        while (true)
        {
            // fade-out
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                SetTextAlpha(Mathf.Lerp(1f, 0f, t / duration));
                yield return null;
            }
            // fade-in
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                SetTextAlpha(Mathf.Lerp(0f, 1f, t / duration));
                yield return null;
            }
        }
    }

    private void SetTextAlpha(float alpha)
    {
        var c = objectiveText.color;
        c.a = alpha;
        objectiveText.color = c;
    }
}
