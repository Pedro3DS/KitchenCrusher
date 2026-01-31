using UnityEngine;

/// <summary>
/// Componente que escuta eventos do jogo e dispara glitches automaticamente.
/// Use este componente para conectar eventos existentes (como TaskFailed) aos glitches.
/// </summary>
public class GlitchEventListener : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private bool listenToStressEvent = true;
    [SerializeField] private bool listenToTaskEvents = false;

    [Header("Glitches para Evento de Estresse")]
    [SerializeField] private GlitchType[] stressGlitches = { GlitchType.CameraSwitch };

    [Header("Referências")]
    [SerializeField] private KitchenTaskController taskController;

    private void OnEnable()
    {
        // Inscreve nos eventos de glitch do manager
        GlitchManager.OnGlitchActivated += HandleGlitchActivated;
        GlitchManager.OnGlitchDeactivated += HandleGlitchDeactivated;

        // Inscreve nos eventos de task
        if (listenToTaskEvents)
        {
            GameEvents.OnTaskCompleted += HandleTaskCompleted;
        }
    }

    private void OnDisable()
    {
        GlitchManager.OnGlitchActivated -= HandleGlitchActivated;
        GlitchManager.OnGlitchDeactivated -= HandleGlitchDeactivated;

        if (listenToTaskEvents)
        {
            GameEvents.OnTaskCompleted -= HandleTaskCompleted;
        }
    }

    private void HandleGlitchActivated(GlitchType type)
    {
        Debug.Log($"[GlitchEventListener] Glitch {type} foi ativado");
    }

    private void HandleGlitchDeactivated(GlitchType type)
    {
        Debug.Log($"[GlitchEventListener] Glitch {type} foi desativado");
    }

    private void HandleTaskCompleted(KitchenTask task)
    {
        // Quando uma task de glitch é completada, desativa os glitches
        if (task.hasGlitchTask)
        {
            DeactivateStressGlitches();
        }
    }

    /// <summary>
    /// Chamado externamente quando o sistema de estresse é ativado.
    /// Conecte este método ao KitchenTaskController quando ele dispara o evento de estresse.
    /// </summary>
    public void OnStressEventTriggered()
    {
        if (!listenToStressEvent) return;

        Debug.Log("[GlitchEventListener] Evento de estresse detectado - ativando glitches");
        ActivateStressGlitches();
    }

    /// <summary>
    /// Ativa todos os glitches configurados para o evento de estresse.
    /// </summary>
    public void ActivateStressGlitches()
    {
        if (GlitchManager.Instance == null) return;

        foreach (var glitchType in stressGlitches)
        {
            GlitchManager.Instance.ActivateGlitch(glitchType);
        }
    }

    /// <summary>
    /// Desativa todos os glitches de estresse.
    /// </summary>
    public void DeactivateStressGlitches()
    {
        if (GlitchManager.Instance == null) return;

        foreach (var glitchType in stressGlitches)
        {
            GlitchManager.Instance.DeactivateGlitch(glitchType);
        }
    }
}
