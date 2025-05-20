using UnityEngine;

public class WristBillboard : MonoBehaviour
{
    public Transform wristTransform;      // Рука или объект, к которому прикреплён HUD
    public Transform cameraTransform;     // Камера (например, Main Camera)
    public RectTransform canvasRect;      // Твой Canvas (UI объект)

    void LateUpdate()
    {
        if (wristTransform == null || cameraTransform == null || canvasRect == null)
            return;

        // 1. Повернуть HUD лицом к камере (как обычный billboard)
        Vector3 toCamera = cameraTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-toCamera);

        // 2. Определить ориентацию ладони
        Vector3 wristUp = wristTransform.up;
        float angle = Vector3.Angle(wristUp, Vector3.up);

        // 3. Если рука смотрит вверх — портрет, если вбок — альбомный
        if (angle > 60f)  // например, если рука почти горизонтальна
        {
            canvasRect.localEulerAngles = Vector3.zero;           // портретный режим
        }
        else
        {
            canvasRect.localEulerAngles = new Vector3(0, 0, 90);  // альбомный режим
        }
    }
}
