using UnityEngine;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2f;
    public Color fadeColor = Color.white; // Устанавливаем начальный белый цвет
    public AnimationCurve fadeCurve;
    public string colorPropertyName = "_BaseColor"; // Используем _BaseColor для Unlit/TransparentTexture
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("Renderer не найден на объекте!", this);
            return;
        }
        rend.enabled = false;
        Debug.Log("Рендерер изначально отключён");

        if (fadeOnStart)
        {
            Debug.Log("Запуск FadeIn");
            FadeIn();
        }
    }

    public void FadeIn()
    {
        Fade(1f, 0f);
    }

    public void FadeOut()
    {
        Fade(0f, 1f);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        Debug.Log($"Начало фейда: от {alphaIn} к {alphaOut}, длительность {fadeDuration} сек");
        rend.enabled = true;
        Debug.Log("Рендерер включён");

        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, fadeCurve.Evaluate(timer / fadeDuration));
            rend.material.SetColor(colorPropertyName, newColor);
            Debug.Log($"Установлена альфа: {newColor.a}");

            timer += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        rend.material.SetColor(colorPropertyName, finalColor);
        Debug.Log($"Фейд завершён, конечная альфа: {finalColor.a}");

        if (alphaOut == 0)
        {
            rend.enabled = false;
            Debug.Log("Рендерер отключён");
        }
    }
}