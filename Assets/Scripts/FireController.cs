using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("Fire Settings")]
    public float extinguishAmount = 0.01f; // Количество уменьшения размера огня за каждое столкновение с частицей
    public float minSizeThreshold = 0.5f; // Порог размера, при котором огонь считается потушенным

    private float currentSize = 1f; // Текущий размер огня (от 0 до 1)
    private bool isExtinguished = false; // Флаг, указывающий, потушен ли огонь
    private Vector3 initialScale; // Начальный масштаб объекта огня

    void Start()
    {
        initialScale = transform.localScale; // Сохраняем начальный масштаб
    }

    void OnParticleCollision(GameObject other)
    {
        if (!isExtinguished)
        {
            ReduceFire();
        }
    }

    void ReduceFire()
    {
        currentSize -= extinguishAmount; // Уменьшаем размер на фиксированное значение
        currentSize = Mathf.Clamp(currentSize, 0f, 1f); // Ограничиваем размер между 0 и 1
        UpdateFireSize();

        if (currentSize <= minSizeThreshold)
        {
            ExtinguishFire();
        }
    }

    void UpdateFireSize()
    {
        // Обновляем масштаб объекта огня
        transform.localScale = initialScale * currentSize;

        // Если есть система частиц, корректируем её параметры
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startSize = currentSize;
        }
    }

    void ExtinguishFire()
    {
        isExtinguished = true;
        gameObject.SetActive(false); // Деактивируем объект огня
        Debug.Log("Огонь потушен!");
    }
}