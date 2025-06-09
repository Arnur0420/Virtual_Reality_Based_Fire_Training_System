using UnityEngine;

public class PulsingEffectOverall : MonoBehaviour
{
    public MeshRenderer safeZoneRenderer; // Ссылка на MeshRenderer для SafeZonePlane
    public LineRenderer exitPathLineRenderer; // Ссылка на LineRenderer для ExitPathLine
    public float pulseSpeed = 1f; // Скорость пульсации (чем меньше, тем медленнее)

    private float alpha = 1f; // Текущая прозрачность
    private bool fadingIn = false; // true — появление, false — исчезновение

    void Start()
    {
        // Проверяем, присвоены ли компоненты
        if (safeZoneRenderer == null)
        {
            safeZoneRenderer = GetComponent<MeshRenderer>();
            if (safeZoneRenderer == null)
            {
                Debug.LogError("MeshRenderer не найден на объекте!");
            }
        }

        if (exitPathLineRenderer == null)
        {
            exitPathLineRenderer = GetComponent<LineRenderer>();
            if (exitPathLineRenderer == null)
            {
                Debug.LogError("LineRenderer не найден на объекте!");
            }
        }
    }

    void Update()
    {
        // Плавно меняем прозрачность
        if (fadingIn)
        {
            alpha += Time.deltaTime * pulseSpeed;
            if (alpha >= 1f)
            {
                alpha = 1f;
                fadingIn = false;
            }
        }
        else
        {
            alpha -= Time.deltaTime * pulseSpeed;
            if (alpha <= 0f)
            {
                alpha = 0f;
                fadingIn = true;
            }
        }

        // Применяем прозрачность к SafeZonePlane
        if (safeZoneRenderer != null)
        {
            Color color = safeZoneRenderer.material.color;
            color.a = alpha;
            safeZoneRenderer.material.color = color;
        }

        // Применяем прозрачность к ExitPathLine
        if (exitPathLineRenderer != null)
        {
            Color startColor = exitPathLineRenderer.startColor;
            startColor.a = alpha;
            exitPathLineRenderer.startColor = startColor;

            Color endColor = exitPathLineRenderer.endColor;
            endColor.a = alpha;
            exitPathLineRenderer.endColor = endColor;
        }
    }
}