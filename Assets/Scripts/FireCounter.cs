using UnityEngine;
using System.Collections;

public class FireCounter : MonoBehaviour
{
    public static FireCounter Instance;

    [Header("Настройки цели")]
    public int initialTarget = 0;    // Стартовая цель
    public int increaseStep = 1;     // На сколько растёт цель при новом очаге
    public int maxActiveFires = 45;  // Максимальное количество активных пожаров
    public int evacFireThreshold = 50; // Порог пожаров для эвакуации
    public float evacTimerSec = 30f; // Время до эвакуации
    public float instructionDelay = 4f; // Задержка между инструкциями

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
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        currentTarget = initialTarget;
        StartCoroutine(ShowInitialInstructions());
        UpdateHUD();
        Debug.Log($"FireCounter инициализирован. Активных пожаров: {activeFires}");
    }

    public void OnHotspotSpawned()
    {
        if (activeFires >= maxActiveFires)
        {
            Debug.LogWarning("Достигнут лимит активных пожаров!");
            return;
        }
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

        if (activeFires > evacFireThreshold)
        {
            evacStarted = true;
            StartCoroutine(EvacuateCountdown());
            return;
        }

        if (activeFires == 0 && extinguishedCount > 0)
        {
            hud.SetObjective("Весь огонь потушен", Color.green, false);
        }
        else
        {
            string msg = $"Потушено: {extinguishedCount}/{currentTarget}";
            Color c = Color.Lerp(
                new Color(1f, 0.6f, 0f),
                Color.green,
                (float)extinguishedCount / currentTarget
            );
            hud.SetObjective(msg, c, false);
        }
    }

    private IEnumerator ShowInitialInstructions()
    {
        string[] instructions = new string[]
        {
            "Обследуйте квартиру",
            "Начался пожар",
            "Найдите огнетушитель",
            "Нажмите на кнопку эвакуации"
        };

        foreach (string instruction in instructions)
        {
            hud.SetObjective(instruction, Color.white, true);
            yield return new WaitForSeconds(instructionDelay);
        }
        UpdateHUD(); // Показываем стандартное сообщение после инструкций
    }

    private IEnumerator EvacuateCountdown()
    {
        float timer = evacTimerSec;
        while (timer > 0f)
        {
            hud.SetObjective($"Эвакуация через {Mathf.CeilToInt(timer)} сек!", Color.red, true);
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
        hud.SetObjective("Эвакуация началась! Перейдите в зону эвакуации", Color.red, false);
        // Здесь можно добавить логику перехода в зону эвакуации (например, загрузку сцены)
    }
}