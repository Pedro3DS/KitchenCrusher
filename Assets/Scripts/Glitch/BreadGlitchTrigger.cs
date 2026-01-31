using UnityEngine;
using Game.Resources;

/// <summary>
/// Componente que detecta quando o jogador pega o pão durante o evento de estresse
/// e ativa o glitch de troca de câmera.
/// </summary>
public class BreadGlitchTrigger : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private ResourceType targetResource = ResourceType.Bread;

    [Header("Glitches ao Pegar")]
    [SerializeField] private GlitchType[] glitchesToActivate = { GlitchType.CameraSwitch };

    [Header("Duração do Glitch")]
    [SerializeField] private float glitchDuration = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private bool isStressed = false;

    private void OnEnable()
    {
        // Escuta quando um ingrediente é pego
        GameEvents.OnIngredientPicked += HandleIngredientPicked;

        // Escuta eventos de estresse
        GameEvents.OnStressTriggered += HandleStressTriggered;
        GameEvents.OnStressResolved += HandleStressResolved;
    }

    private void OnDisable()
    {
        GameEvents.OnIngredientPicked -= HandleIngredientPicked;
        GameEvents.OnStressTriggered -= HandleStressTriggered;
        GameEvents.OnStressResolved -= HandleStressResolved;
    }

    private void HandleStressTriggered()
    {
        isStressed = true;
        Debug.Log("[BreadGlitchTrigger] ESTRESSE ATIVADO - aguardando pegar o pão");
    }

    private void HandleStressResolved()
    {
        isStressed = false;
        Debug.Log("[BreadGlitchTrigger] Estresse resolvido");
    }

    private void HandleIngredientPicked(Ingredient ingredient)
    {
        Debug.Log($"[BreadGlitchTrigger] Ingrediente pego: {ingredient.ingredientName} | Tipo: {ingredient.ingredientType} | Estresse: {isStressed}");

        // Só ativa se estiver em modo de estresse
        if (!isStressed)
        {
            Debug.Log("[BreadGlitchTrigger] Ignorado - não está em modo de estresse");
            return;
        }

        // Verifica se é o ingrediente alvo (pão)
        if (ingredient.ingredientType != targetResource)
        {
            Debug.Log($"[BreadGlitchTrigger] Ignorado - tipo {ingredient.ingredientType} != {targetResource}");
            return;
        }

        Debug.Log("[BreadGlitchTrigger] PÃO DETECTADO! Ativando glitch de câmera...");
        ActivateGlitches();
    }

    private void ActivateGlitches()
    {
        if (GlitchManager.Instance == null)
        {
            Debug.LogWarning("[BreadGlitchTrigger] GlitchManager não encontrado!");
            return;
        }

        foreach (var glitchType in glitchesToActivate)
        {
            // Ativa o glitch por tempo determinado e depois volta ao normal
            GlitchManager.Instance.ActivateGlitchForDuration(glitchType, glitchDuration);
        }
    }
}
