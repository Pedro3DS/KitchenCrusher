using UnityEngine;

/// <summary>
/// Controlador que conecta o sistema de estresse do jogo ao sistema de glitches.
/// Coloque este componente em um GameObject na cena para ativar glitches automaticamente
/// quando eventos de estresse ocorrerem.
/// </summary>
public class StressGlitchController : MonoBehaviour
{
    [Header("Glitches para Ativar no Estresse")]
    [Tooltip("Lista de glitches que serão ativados quando o jogador entrar em estado de estresse (NÃO inclua CameraSwitch aqui - ele é ativado pelo BreadGlitchTrigger)")]
    [SerializeField] private GlitchType[] glitchesOnStress = { };

    [Header("Glitches Adicionais por Área")]
    [Tooltip("Glitches que serão ativados quando o jogador entrar em uma área específica (ex: armazém)")]
    [SerializeField] private GlitchType[] areaGlitches = { GlitchType.CameraShake };

    [Header("Configuração")]
    [SerializeField] private bool autoRegister = true;

    private void OnEnable()
    {
        if (autoRegister)
        {
            GameEvents.OnStressTriggered += HandleStressTriggered;
            GameEvents.OnStressResolved += HandleStressResolved;
        }
    }

    private void OnDisable()
    {
        GameEvents.OnStressTriggered -= HandleStressTriggered;
        GameEvents.OnStressResolved -= HandleStressResolved;
    }

    private void HandleStressTriggered()
    {
        if (GlitchManager.Instance == null) return;

        Debug.Log("[StressGlitchController] Ativando glitches de estresse");

        foreach (var glitchType in glitchesOnStress)
        {
            GlitchManager.Instance.ActivateGlitch(glitchType);
        }
    }

    private void HandleStressResolved()
    {
        if (GlitchManager.Instance == null) return;

        Debug.Log("[StressGlitchController] Desativando glitches de estresse");

        // Desativa glitches de estresse
        foreach (var glitchType in glitchesOnStress)
        {
            GlitchManager.Instance.DeactivateGlitch(glitchType);
        }

        // Também desativa glitches de área
        foreach (var glitchType in areaGlitches)
        {
            GlitchManager.Instance.DeactivateGlitch(glitchType);
        }
    }

    /// <summary>
    /// Ativa os glitches de área (chamar quando jogador entrar em uma zona específica).
    /// </summary>
    public void ActivateAreaGlitches()
    {
        if (GlitchManager.Instance == null) return;

        foreach (var glitchType in areaGlitches)
        {
            GlitchManager.Instance.ActivateGlitch(glitchType);
        }
    }

    /// <summary>
    /// Desativa os glitches de área.
    /// </summary>
    public void DeactivateAreaGlitches()
    {
        if (GlitchManager.Instance == null) return;

        foreach (var glitchType in areaGlitches)
        {
            GlitchManager.Instance.DeactivateGlitch(glitchType);
        }
    }

    /// <summary>
    /// Ativa um glitch específico manualmente (útil para chamar de UnityEvents).
    /// </summary>
    public void ActivateGlitch(int glitchTypeIndex)
    {
        if (GlitchManager.Instance != null)
        {
            GlitchManager.Instance.ActivateGlitch((GlitchType)glitchTypeIndex);
        }
    }

    /// <summary>
    /// Desativa um glitch específico manualmente (útil para chamar de UnityEvents).
    /// </summary>
    public void DeactivateGlitch(int glitchTypeIndex)
    {
        if (GlitchManager.Instance != null)
        {
            GlitchManager.Instance.DeactivateGlitch((GlitchType)glitchTypeIndex);
        }
    }
}
