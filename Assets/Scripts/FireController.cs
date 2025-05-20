using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("Fire Settings")]
    public float extinguishAmount = 0.01f; // Количество уменьшения размера за столкновение
    public float minSizeThreshold = 0.5f; // Порог для тушения

    private float currentSize = 1f;
    private bool isExtinguished = false;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
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
        currentSize -= extinguishAmount;
        currentSize = Mathf.Clamp(currentSize, 0f, 1f);
        UpdateFireSize();

        if (currentSize <= minSizeThreshold)
        {
            ExtinguishFire();
        }
    }

    void UpdateFireSize()
    {
        transform.localScale = initialScale * currentSize;
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
        gameObject.SetActive(false);
        // Уведомляем FireCounter о тушении
        if (FireCounter.Instance != null)
        {
            FireCounter.Instance.OnHotspotExtinguished();
            Debug.Log("Огонь потушен, уведомлен FireCounter");
        }
        else
        {
            Debug.LogError("FireCounter.Instance не найден!");
        }
    }
}