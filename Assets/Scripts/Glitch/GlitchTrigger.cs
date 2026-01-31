using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Trigger modular para ativar/desativar glitches.
/// Pode ser configurado para responder a diferentes eventos.
/// </summary>
public class GlitchTrigger : MonoBehaviour
{
    [Header("Glitch a Disparar")]
    [SerializeField] private GlitchType glitchType = GlitchType.CameraSwitch;
    [SerializeField] private GlitchAction action = GlitchAction.Activate;

    [Header("Configuração de Duração")]
    [SerializeField] private bool useDuration = false;
    [SerializeField] private float duration = 5f;

    [Header("Trigger por Colisão")]
    [SerializeField] private bool useCollisionTrigger = true;
    [SerializeField] private string targetTag = "Player";

    [Header("Configurações Extras")]
    [SerializeField] private bool oneShot = false;
    [SerializeField] private float cooldown = 0f;

    [Header("Eventos")]
    public UnityEvent OnGlitchTriggered;

    private bool hasTriggered = false;
    private float lastTriggerTime = -999f;

    public enum GlitchAction
    {
        Activate,
        Deactivate,
        Toggle
    }

    /// <summary>
    /// Dispara o glitch manualmente (pode ser chamado de outros scripts).
    /// </summary>
    public void TriggerGlitch()
    {
        if (GlitchManager.Instance == null)
        {
            Debug.LogWarning("[GlitchTrigger] GlitchManager não encontrado!");
            return;
        }

        // Verifica one-shot
        if (oneShot && hasTriggered) return;

        // Verifica cooldown
        if (Time.time - lastTriggerTime < cooldown) return;

        ExecuteGlitchAction();

        hasTriggered = true;
        lastTriggerTime = Time.time;
        OnGlitchTriggered?.Invoke();
    }

    private void ExecuteGlitchAction()
    {
        switch (action)
        {
            case GlitchAction.Activate:
                if (useDuration)
                    GlitchManager.Instance.ActivateGlitchForDuration(glitchType, duration);
                else
                    GlitchManager.Instance.ActivateGlitch(glitchType);
                break;

            case GlitchAction.Deactivate:
                GlitchManager.Instance.DeactivateGlitch(glitchType);
                break;

            case GlitchAction.Toggle:
                GlitchManager.Instance.ToggleGlitch(glitchType);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useCollisionTrigger) return;

        if (other.CompareTag(targetTag))
        {
            TriggerGlitch();
        }
    }

    /// <summary>
    /// Reseta o trigger para poder disparar novamente (útil para one-shot).
    /// </summary>
    public void ResetTrigger()
    {
        hasTriggered = false;
    }

    /// <summary>
    /// Define o tipo de glitch em runtime.
    /// </summary>
    public void SetGlitchType(GlitchType type)
    {
        glitchType = type;
    }
}
