using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WristObjectivesUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Ссылка на TextMeshProUGUI внутри панели.")]
    public TextMeshProUGUI objectiveText;

    [Header("Settings")]
    [Tooltip("Интервал между сменой сообщений (в секундах).")]
    public float switchInterval = 3f;

    // Список сообщений
    private readonly List<string> messages = new List<string>()
    {
        "🔍 Исследуйте комнату",
        "🔥 Найдите огнетушитель",
        "🚨 Включите сигнализацию",
        "🔥 Пожар сильный! Ищите выход!"
    };

    private int currentIndex = 0;
    private float timer = 0f;
    private bool isLastBlinking = false;

    void Start()
    {
        if (objectiveText == null)
        {
            Debug.LogError("WristObjectivesUI: не назначен Objective Text!");
            enabled = false;
            return;
        }
        // Запустить первый показ
        UpdateObjective();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Если мы на последнем сообщении и оно мигает — не менять его цвет плавно
        if (isLastBlinking)
            return;

        // Цветной переход: от желтого к зелёному в течение switchInterval
        float t = Mathf.Clamp01(timer / switchInterval);
        Color startColor = Color.yellow;
        Color endColor = Color.green;
        objectiveText.color = Color.Lerp(startColor, endColor, t);

        // Когда время вышло — переходим к следующему сообщению
        if (timer >= switchInterval)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % messages.Count;
            UpdateObjective();
        }
    }

    private void UpdateObjective()
    {
        string msg = messages[currentIndex];
        objectiveText.text = msg;

        // Если это последнее сообщение — включаем красное мигание
        if (currentIndex == messages.Count - 1)
        {
            isLastBlinking = true;
            StartCoroutine(BlinkRed());
        }
        else
        {
            // Обычный сброс состояний
            isLastBlinking = false;
            StopAllCoroutines();
            objectiveText.enabled = true;
            objectiveText.color = Color.yellow;
        }
    }

    private IEnumerator BlinkRed()
    {
        while (true)
        {
            objectiveText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            objectiveText.enabled = !objectiveText.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
