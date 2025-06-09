using UnityEngine;
using TMPro;

public class PulsingTextEffect : MonoBehaviour
{
    public TextMeshProUGUI text; // Ссылка на текстовый компонент
    public float pulseSpeed = 1f; // Скорость пульсации (чем меньше, тем медленнее)

    private float alpha = 1f; // Текущая прозрачность
    private bool fadingIn = false; // true — появление, false — исчезновение

    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();

        }
    }

    void Update()
    {
        if (text != null)
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
            // Применяем прозрачность к тексту
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }
}