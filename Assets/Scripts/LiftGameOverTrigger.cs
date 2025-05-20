using UnityEngine;

public class LiftGameOverTrigger : MonoBehaviour
{
    // drag & drop сюда из Inspector:
    public GameOverOverlayController gameOverOverlay;
    public FireCounter fireCounter;

    // эту функцию мы будем вызывать из VRButton.onPressed
    public void OnLiftButtonPressed()
    {
        if (fireCounter != null && fireCounter.ActiveFires > 0) // Используем публичное свойство
        {
            gameOverOverlay.ShowGameOver("⚠️ Game Over\nНельзя пользоваться лифтом во время пожара.");
        }
    }
}