using UnityEngine;
using System.Collections;

public class FireCounter : MonoBehaviour
{
    public static FireCounter Instance;

    [Header("Настройки цели")]
    public int initialTarget = 5;    // стартовая цель
    public int increaseStep = 5;     // на сколько растёт цель при новом очаге
    public int maxBeforeEvac = 200;  // активных очагов до эвакуации
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
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        currentTarget = initialTarget;
        UpdateHUD();
    }

    /// <summary>Вызываем при спавне нового огня</summary>
    public void OnHotspotSpawned()
    {
        activeFires++;
        currentTarget += increaseStep;  // динамическое увеличение цели
        UpdateHUD();
    }

    /// <summary>Вызываем при полном гашении огня</summary>
    public void OnHotspotExtinguished()
    {
        extinguishedCount++;
        activeFires = Mathf.Max(0, activeFires - 1);
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (evacStarted) return;

        // Условие эвакуации по числу активных очагов
        if (activeFires > maxBeforeEvac)
        {
            evacStarted = true;
            StartCoroutine(EvacuateCountdown());
            return;
        }

        // Обычный режим: показываем прогресс тушений  и цель
        string msg = $"🔥 Потушено: {extinguishedCount}/{currentTarget}";
        // Цвет от оранжевого к зелёному (можешь поменять на любой)
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
                $"🚨 Эвакуация через {Mathf.CeilToInt(timer)} сек!",
                Color.red,
                true
            );
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
        hud.SetObjective("🏃‍♂️ Эвакуация началась!", Color.red, false);
        // Здесь можно вызывать переход на финальную сцену
    }
}
