using UnityEngine;
using TMPro;
using System.Collections;

public class GameOverOverlayController : MonoBehaviour
{
    public GameObject gameOverPanel; // Канвас или панель Game Over
    public TextMeshProUGUI gameOverText; // Текстовое поле
    public Camera mainCamera; // Главная или VR-камера
    private CanvasGroup canvasGroup; // Для fade-out эффекта

    void Awake()
    {
        // Добавляем CanvasGroup, если его нет
        canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();
        }
        // Изначально делаем канвас прозрачным и выключенным
        canvasGroup.alpha = 0f;
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(string message)
    {
        if (gameOverPanel != null)
        {
            // Позиционируем канвас перед камерой
            if (mainCamera != null)
            {
                Vector3 cameraPos = mainCamera.transform.position;
                Vector3 cameraForward = mainCamera.transform.forward;
                gameOverPanel.transform.position = cameraPos + cameraForward * 1f;
                gameOverPanel.transform.rotation = Quaternion.LookRotation(gameOverPanel.transform.position - cameraPos);
            }
            else
            {
                Debug.LogError("mainCamera не назначена в GameOverOverlayController!");
            }

            gameOverPanel.SetActive(true);
            canvasGroup.alpha = 1f; // Делаем полностью видимым
            if (gameOverText != null)
            {
                gameOverText.text = message;
                Debug.Log($"Показан Game Over с текстом: {message}");
            }
            else
            {
                Debug.LogError("gameOverText не назначен!");
            }
        }
        else
        {
            Debug.LogError("gameOverPanel не назначен!");
        }
    }

    public void FadeOutGameOver()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (gameOverPanel != null && canvasGroup != null)
        {
            float duration = 1f; // Длительность fade-out в секундах
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
            gameOverPanel.SetActive(false);
            Debug.Log("Game Over скрыт с fade-out");
        }
        else
        {
            Debug.LogError("gameOverPanel или canvasGroup не назначены!");
        }
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            canvasGroup.alpha = 0f;
            Debug.Log("Game Over скрыт мгновенно");
        }
        else
        {
            Debug.LogError("gameOverPanel не назначен при скрытии!");
        }
    }
}