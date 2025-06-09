using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("Components")]
    public ParticleSystem extinguisherParticles;
    public AudioSource extinguisherSound;
    
    [Header("Input Settings")]
    [SerializeField] private float activationThreshold = 0.5f;
    
    [Header("Haptics Settings")]
    [Range(0, 1)] public float hapticIntensity = 0.1f;    // уменьшили интенсивность
    [Range(0, 1)] public float hapticDuration = 0.05f;    // короткий, мягкий импульс
    
    [Header("Joystick Input")]
    public InputActionProperty joystickDirection;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private ActionBasedController actionController;
    private bool isHeld = false;
    private Coroutine hapticCoroutine = null;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Update()
    {
        if (!isHeld) return;

        HandleInput();
        HandleJoystickRotation();
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        actionController = args.interactorObject.transform.GetComponent<ActionBasedController>();
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        actionController = null;
        StopExtinguisher();
    }

    private void HandleInput()
    {
        if (actionController == null) return;
        
        float activateValue = actionController.activateAction.action.ReadValue<float>();
        if (activateValue > activationThreshold)
        {
            StartExtinguisher();
        }
        else
        {
            StopExtinguisher();
        }
    }

    private void HandleJoystickRotation()
    {
        if (joystickDirection == null || joystickDirection.action == null) return;

        Vector2 input = joystickDirection.action.ReadValue<Vector2>();
        if (input.magnitude >= 0.1f)
        {
            float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, angle, 0);
        }
    }

    private void StartExtinguisher()
    {
        if (!extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Play();
            extinguisherSound.Play();
        }
        // Запускаем корутину мягкой пульсации, если ещё не запущена
        if (hapticCoroutine == null)
            hapticCoroutine = StartCoroutine(HapticPulseRoutine());
    }

    private void StopExtinguisher()
    {
        if (extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Stop();
            extinguisherSound.Stop();
        }
        // Останавливаем корутину вибрации
        if (hapticCoroutine != null)
        {
            StopCoroutine(hapticCoroutine);
            hapticCoroutine = null;
        }
    }

    private IEnumerator HapticPulseRoutine()
    {
        while (true)
        {
            TriggerHaptic();
            // добавляем небольшую паузу для ритма: длительность импульса + 0.1 секунды
            yield return new WaitForSeconds(hapticDuration + 0.1f);
        }
    }

    private void TriggerHaptic()
    {
        if (actionController != null)
        {
            actionController.SendHapticImpulse(hapticIntensity, hapticDuration);
        }
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
