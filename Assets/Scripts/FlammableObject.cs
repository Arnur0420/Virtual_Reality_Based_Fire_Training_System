using UnityEngine;

public class FlammableObject : MonoBehaviour
{
    [Header("Settings")]
    public GameObject firePrefab; // Префаб огня
    public Vector3 fireOffset = new Vector3(0, 1f, 0); // Смещение огня

    private GameObject fireInstance;
    private bool isBurning = false;

    public void Ignite()
    {
        if (isBurning) return;

        isBurning = true;
        Debug.Log($"{gameObject.name} загорелся!");

        if (firePrefab != null)
        {
            fireInstance = Instantiate(firePrefab, transform.position + fireOffset, Quaternion.identity, transform);
            BoxCollider boxCol = fireInstance.GetComponent<BoxCollider>();
            if (boxCol == null)
            {
                boxCol = fireInstance.AddComponent<BoxCollider>();
                boxCol.isTrigger = false; // Для OnParticleCollision
            }
            // Уведомляем FireCounter о новом пожаре
            if (FireCounter.Instance != null)
            {
                FireCounter.Instance.OnHotspotSpawned();
            }
            else
            {
                Debug.LogError("FireCounter.Instance не найден!");
            }
        }
        else
        {
            Debug.LogError($"Префаб огня не назначен для {gameObject.name}!");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + fireOffset, 0.2f);
    }
}