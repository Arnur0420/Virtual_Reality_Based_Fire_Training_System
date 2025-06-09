using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SafeZoneTrigger : MonoBehaviour
{
    public GameOverOverlayController gameOverOverlay;
    public string safeZoneMessage = "Вы обучены и теперь знаете, как действовать во время пожара";
    public float countdownTime = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameOverOverlay == null)
            {
                Debug.LogError("Ошибка: gameOverOverlay не назначен!");
                return;
            }
            StartCoroutine(ShowSafeZoneMessageAndCountdown());
        }
    }

    private IEnumerator ShowSafeZoneMessageAndCountdown()
    {
        gameOverOverlay.ShowGameOver(safeZoneMessage);
        float timer = countdownTime;
        while (timer > 0)
        {
            gameOverOverlay.gameOverText.text = $"{safeZoneMessage}\nПерезапуск через {Mathf.CeilToInt(timer)} сек.";
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
        gameOverOverlay.gameOverText.text = "Перезапуск...";
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("1 Start Scene");
    }
}