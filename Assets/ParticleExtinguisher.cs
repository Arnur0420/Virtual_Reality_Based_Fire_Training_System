using UnityEngine;

public class ParticleExtinguisher : MonoBehaviour
{
    void Start()
    {
        // Настройки частиц (обязательно!)
        var ps = GetComponent<ParticleSystem>();
        var collision = ps.collision;
        collision.enabled = true;
        collision.sendCollisionMessages = true; // Включаем отправку сообщений
        collision.collidesWith = LayerMask.GetMask("Fire"); // Частицы сталкиваются только с слоем "Fire"
    }
}