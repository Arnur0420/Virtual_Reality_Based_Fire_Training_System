using UnityEngine;
using TMPro;
using System.Collections;

public class GameOverOverlayController : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup canvasGroup;           
    public TextMeshProUGUI gameOverText;      

    [Header("Timings")]
    public float delayBeforeShow = 3f;        
    public float fadeDuration = 1f;           
    public float holdDuration = 10f;          
    public float fadeOutDuration = 1f;

    // Сохраняем сообщение для корутины
    private string currentMessage;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Запустить Game Over sequence
    /// </summary>
    public void ShowGameOver(string message)
    {
        currentMessage = message;               // сохраняем сюда
        StopAllCoroutines();
        gameOverText.text = currentMessage;     // сразу показываем текст
        StartCoroutine(DoShow());
    }

    private IEnumerator DoShow()
    {
        // 1) задержка перед показом
        yield return new WaitForSeconds(delayBeforeShow);
        Debug.Log("[GameOverOverlay] Showing overlay");

        // 2) fade-in
        float t = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 3) обратный отсчёт и обновление текста
        float timer = holdDuration;
        while (timer > 0f)
        {
            gameOverText.text = $"{currentMessage}\nПерезапуск через {Mathf.CeilToInt(timer)}...";
            timer -= Time.deltaTime;
            yield return null;
        }

        // 4) fade-out
        t = 0f;
        while (t < fadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Debug.Log("[GameOverOverlay] Overlay hidden");
    }
}
