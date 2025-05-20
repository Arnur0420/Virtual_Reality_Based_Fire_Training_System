using UnityEngine;
using System.Collections.Generic;

public class ObjectivesManager : MonoBehaviour
{
    [Header("HUD")]
    public HUDObjectives hud;

    private List<string> objectives = new List<string>();

    // Добавить новую задачу в конец списка
    public void AddObjective(string newObj)
    {
        objectives.Add(newObj);
        UpdateHUD();
    }

    // Удалить задачу по индексу (обычно 0) и показать следующую
    public void CompleteObjective(int index = 0)
    {
        if (index >= 0 && index < objectives.Count)
            objectives.RemoveAt(index);
        UpdateHUD();
    }

    // Обновить HUD — показываем первую задачу, если есть, иначе финал
    private void UpdateHUD()
    {
        if (objectives.Count > 0)
        {
            hud.SetObjective(objectives[0], Color.yellow, false);
        }
        else
        {
            hud.SetObjective("✅ Все цели выполнены!", Color.green, false);
            // при необходимости можно тут же скрыть HUD через hud.SetObjective("", ..., false)
        }
    }
}
