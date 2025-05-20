using UnityEngine;
using System.Collections;

public class LiftGameOverTrigger : MonoBehaviour
{
    public GameOverOverlayController gameOverOverlay;
    public FireCounter fireCounter;

    public void OnLiftButtonPressed()
    {
        Debug.Log("Нажата кнопка лифта");
        if (fireCounter == null)
        {
            Debug.LogError("Ошибка: fireCounter не назначен!");
            return;
        }
        Debug.Log($"Количество активных пожаров: {fireCounter.ActiveFires}");
        if (fireCounter.ActiveFires > 0)
        {
            Debug.Log("Вызываю ShowGameOver");
            if (gameOverOverlay == null)
            {
                Debug.LogError("Ошибка: gameOverOverlay не назначен!");
                return;
            }
            gameOverOverlay.ShowGameOver("⚠️ Game Over\nНельзя пользоваться лифтом во время пожара.");
            StartCoroutine(HideGameOverAfterDelay(10f));
        }
        else
        {
            Debug.Log("Нет активных пожаров, Game Over не показывается");
        }
    }

    private IEnumerator HideGameOverAfterDelay(float delay)
    {
        Debug.Log("Запущен таймер для скрытия Game Over");
        yield return new WaitForSeconds(delay);
        if (gameOverOverlay != null)
        {
            gameOverOverlay.HideGameOver();
            Debug.Log("Game Over скрыт");
        }
        else
        {
            Debug.LogError("Ошибка: gameOverOverlay не назначен при скрытии!");
        }
    }
}