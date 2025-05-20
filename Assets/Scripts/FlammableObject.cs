using UnityEngine;

public class FlammableObject : MonoBehaviour
{
    [Header("Fire Settings")]
    public GameObject firePrefab; // Префаб огня для создания
    public Vector3 fireOffset = new Vector3(0, 1f, 0); // Смещение позиции огня относительно объекта

    private bool isBurning = false; // Флаг, указывающий, горит ли объект

    public void Ignite()
    {
        if (isBurning) return;

        isBurning = true;
        Debug.Log($"🔥 {gameObject.name} теперь горит!");

        if (firePrefab != null)
        {
            GameObject fireInstance = Instantiate(firePrefab, transform.position + fireOffset, Quaternion.identity, transform);
            // Убеждаемся, что у огня есть коллайдер для столкновения с частицами
            if (fireInstance.GetComponent<Collider>() == null)
            {
                BoxCollider collider = fireInstance.AddComponent<BoxCollider>();
                collider.isTrigger = false; // Должен быть не триггером для OnParticleCollision
            }
        }
        else
        {
            Debug.LogError("Префаб огня не назначен!");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + fireOffset, 0.1f); // Визуализация точки появления огня
    }
}