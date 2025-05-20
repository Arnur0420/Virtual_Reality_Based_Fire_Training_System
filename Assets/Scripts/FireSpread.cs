using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public float spreadRadius = 5.0f;
    public float spreadDelay = 3.0f;
    public int maxSpreadCount = 5;

    private int currentSpreadCount = 0;

    void Start()
    {
        Invoke(nameof(SpreadFire), spreadDelay);
    }

    void SpreadFire()
    {
        if (currentSpreadCount >= maxSpreadCount)
            return;

        Collider[] cols = Physics.OverlapSphere(transform.position, spreadRadius);
        foreach (var col in cols)
        {
            if (col.transform == transform) continue;
            var fl = col.GetComponent<FlammableObject>();
            if (fl != null) fl.Ignite();
        }

        currentSpreadCount++;
        Invoke(nameof(SpreadFire), spreadDelay);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spreadRadius);
    }
}
