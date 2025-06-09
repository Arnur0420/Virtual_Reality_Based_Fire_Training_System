using UnityEngine;
using System.Collections;

public class FireAlarmTrigger : MonoBehaviour
{
    public GameOverOverlayController gameOverOverlay;
    public FireCounter fireCounter;

    public void OnFireAlarmPressed()
    {
        Debug.Log("Нажата кнопка пожарной сигнализации");
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
            Debug.Log("Вызываю ShowGameOver для пожарной сигнализации");
            gameOverOverlay.ShowGameOver("Молодец!\nПожарные прибудут через 5 минут.");
            StartCoroutine(FadeOutGameOverAfterDelay(3f));
        }
        else
        {
            Debug.Log("Нет активных пожаров, сообщение не показывается");
        }
    }

    private IEnumerator FadeOutGameOverAfterDelay(float delay)
    {
        Debug.Log("Запущен таймер для fade-out сообщения");
        yield return new WaitForSeconds(delay);
        if (gameOverOverlay != null)
        {
            gameOverOverlay.FadeOutGameOver();
            Debug.Log("Запущен fade-out сообщения");
        }
        else
        {
            Debug.LogError("Ошибка: gameOverOverlay не назначен при fade-out!");
        }
    }
}