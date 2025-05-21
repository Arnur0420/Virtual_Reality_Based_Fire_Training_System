using UnityEngine;

public class FlammableObject : MonoBehaviour
{
    [Header("Settings")]
    public GameObject firePrefab;
    public Vector3 fireOffset = new Vector3(0, 1f, 0);

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
                boxCol.isTrigger = false;
            }
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