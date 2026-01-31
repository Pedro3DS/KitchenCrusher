using UnityEngine;

/// <summary>
/// Classe base abstrata para todos os glitches.
/// Herde desta classe para criar novos tipos de glitch.
/// </summary>
public abstract class GlitchBase : MonoBehaviour
{
    [Header("Configuração Base")]
    [SerializeField] protected GlitchType glitchType;
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected bool isActive = false;

    /// <summary>
    /// Tipo do glitch para identificação.
    /// </summary>
    public GlitchType Type => glitchType;

    /// <summary>
    /// Se o glitch está ativo no momento.
    /// </summary>
    public bool IsActive => isActive;

    /// <summary>
    /// Duração padrão do glitch.
    /// </summary>
    public float Duration => duration;

    /// <summary>
    /// Ativa o glitch. Override para implementar comportamento específico.
    /// </summary>
    public virtual void Activate()
    {
        if (isActive) return;

        isActive = true;
        OnActivate();
        Debug.Log($"[Glitch] {glitchType} ATIVADO");
    }

    /// <summary>
    /// Desativa o glitch. Override para implementar comportamento específico.
    /// </summary>
    public virtual void Deactivate()
    {
        if (!isActive) return;

        isActive = false;
        OnDeactivate();
        Debug.Log($"[Glitch] {glitchType} DESATIVADO");
    }

    /// <summary>
    /// Toggle do estado do glitch.
    /// </summary>
    public void Toggle()
    {
        if (isActive)
            Deactivate();
        else
            Activate();
    }

    /// <summary>
    /// Implementar a lógica de ativação específica do glitch.
    /// </summary>
    protected abstract void OnActivate();

    /// <summary>
    /// Implementar a lógica de desativação específica do glitch.
    /// </summary>
    protected abstract void OnDeactivate();

    /// <summary>
    /// Chamado quando o glitch precisa ser resetado ao estado inicial.
    /// </summary>
    public virtual void ResetGlitch()
    {
        if (isActive)
            Deactivate();
    }
}
