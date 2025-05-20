using UnityEngine;
using TMPro; // Для TextMeshPro, если используешь его

public class GameOverOverlayController : MonoBehaviour
{
    public GameObject gameOverPanel; // Канвас или панель Game Over
    public TextMeshProUGUI gameOverText; // Текстовое поле для сообщения (или Text, если не используешь TMP)

    public void ShowGameOver(string message)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
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

    public void HideGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("Game Over скрыт");
        }
        else
        {
            Debug.LogError("gameOverPanel не назначен при скрытии!");
        }
    }
}