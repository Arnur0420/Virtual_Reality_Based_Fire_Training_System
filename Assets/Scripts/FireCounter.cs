using UnityEngine;
using System.Collections;

public class FireCounter : MonoBehaviour
{
    public static FireCounter Instance;

    [Header("Настройки цели")]
    public int initialTarget = 5;    // Стартовая цель
    public int increaseStep = 5;     // На сколько растёт цель при новом очаге
    public int maxBeforeEvac = 200;  // Активных очагов до эвакуации
    public float evacTimerSec = 30f;

    [Header("Ссылки")]
    public HUDObjectives hud;

    private int currentTarget;
    private int activeFires = 0;
    public int ActiveFires => activeFires;
    private int extinguishedCount = 0;
    private bool evacStarted = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем между сценами
        }
    }

    void Start()
    {
        currentTarget = initialTarget;
        UpdateHUD();
        Debug.Log($"FireCounter инициализирован. Активных пожаров: {activeFires}");
    }

    public void OnHotspotSpawned()
    {
        activeFires++;
        currentTarget += increaseStep;
        Debug.Log($"Огонь добавлен. Активных пожаров: {activeFires}, Цель: {currentTarget}");
        UpdateHUD();
    }

    public void OnHotspotExtinguished()
    {
        extinguishedCount++;
        activeFires = Mathf.Max(0, activeFires - 1);
        Debug.Log($"Огонь потушен. Активных пожаров: {activeFires}, Потушено: {extinguishedCount}");
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (evacStarted) return;

        if (activeFires > maxBeforeEvac)
        {
            evacStarted = true;
            StartCoroutine(EvacuateCountdown());
            return;
        }

        string msg = $"Потушено: {extinguishedCount}/{currentTarget}";
        Color c = Color.Lerp(
            new Color(1f, 0.6f, 0f),
            Color.green,
            (float)extinguishedCount / currentTarget
        );
        hud.SetObjective(msg, c, false);
    }

    private IEnumerator EvacuateCountdown()
    {
        float timer = evacTimerSec;
        while (timer > 0f)
        {
            hud.SetObjective(
                $"Эвакуация через {Mathf.CeilToInt(timer)} сек!",
                Color.red,
                true
            );
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
        hud.SetObjective("Эвакуация началась!", Color.red, false);
    }
}