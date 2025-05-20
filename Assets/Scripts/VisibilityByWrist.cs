using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class VisibilityByWrist : MonoBehaviour
{
    [Header("References")]
    public Transform wristTransform;     // обычно твой WristCanvas.transform
    public Transform cameraTransform;    // Main Camera.transform

    [Header("Settings")]
    [Tooltip("Максимальный угол между нормалью ладони и направлением на камеру")]
    [Range(0, 90)]
    public float visibleAngleThreshold = 45f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (wristTransform == null) wristTransform = transform;
    }

    void Update()
    {
        Vector3 toCam = (cameraTransform.position - wristTransform.position).normalized;
        Vector3 palmNormal = wristTransform.forward; // направлен внутрь ладони

        // Полный угол
        float angle = Vector3.Angle(palmNormal, toCam);

        // Показываем, если угол в пределах threshold
        bool show = angle <= visibleAngleThreshold;
        canvasGroup.alpha = show ? 1f : 0f;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}
