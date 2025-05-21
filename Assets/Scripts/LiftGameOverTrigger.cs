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
            if (gameOverOverlay == null)
            {
                Debug.LogError("Ошибка: gameOverOverlay не назначен!");
                return;
            }
            Debug.Log("Вызываю ShowGameOver");
            gameOverOverlay.ShowGameOver("Game Over\nНельзя пользоваться лифтом во время пожара.");
            StartCoroutine(FadeOutGameOverAfterDelay(3f));
        }
        else
        {
            Debug.Log("Нет активных пожаров, Game Over не показывается");
        }
    }

    private IEnumerator FadeOutGameOverAfterDelay(float delay)
    {
        Debug.Log("Запущен таймер для fade-out Game Over");
        yield return new WaitForSeconds(delay);
        if (gameOverOverlay != null)
        {
            gameOverOverlay.FadeOutGameOver();
            Debug.Log("Запущен fade-out Game Over");
        }
        else
        {
            Debug.LogError("Ошибка: gameOverOverlay не назначен при fade-out!");
        }
    }
}