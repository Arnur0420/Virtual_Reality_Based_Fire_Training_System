using UnityEngine;

public class FireStarter : MonoBehaviour
{
    [Header("Fire Prefabs")]
    public GameObject bigFirePrefab;
    public GameObject mediumFirePrefab;
    public GameObject smallFirePrefab;

    [Header("Settings")]
    public FireSize fireSize = FireSize.Medium;
    public float startRadius = 3.0f;
    [Tooltip("Какие слои считать воспламеняемыми")]
    public LayerMask flammableLayer;

    public enum FireSize { Big, Medium, Small }

    void Start()
    {
        StartFire();
    }

    void StartFire()
    {
        GameObject firePrefab = ChooseFirePrefab(fireSize);
        if (firePrefab == null)
        {
            Debug.LogError("❌ [FireStarter] No fire prefab assigned!");
            return;
        }

        // Указываем mask flammableLayer
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position, 
            startRadius, 
            flammableLayer, 
            QueryTriggerInteraction.Collide
        );

        foreach (var hit in hitColliders)
        {
            FlammableObject flammable = hit.GetComponent<FlammableObject>();
            if (flammable != null)
                flammable.Ignite();
        }
    }

    GameObject ChooseFirePrefab(FireSize size)
    {
        return size switch
        {
            FireSize.Big    => bigFirePrefab,
            FireSize.Medium => mediumFirePrefab,
            FireSize.Small  => smallFirePrefab,
            _               => mediumFirePrefab,
        };
    }
}
