using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gerenciador central de todos os glitches do jogo.
/// Singleton que permite ativar/desativar glitches de qualquer lugar.
/// </summary>
public class GlitchManager : MonoBehaviour
{
    public static GlitchManager Instance { get; private set; }

    [Header("Glitches Registrados")]
    [SerializeField] private List<GlitchBase> registeredGlitches = new List<GlitchBase>();

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    // Cache para acesso rápido por tipo
    private Dictionary<GlitchType, List<GlitchBase>> glitchCache = new Dictionary<GlitchType, List<GlitchBase>>();

    #region Events
    /// <summary>
    /// Disparado quando qualquer glitch é ativado.
    /// </summary>
    public static event Action<GlitchType> OnGlitchActivated;

    /// <summary>
    /// Disparado quando qualquer glitch é desativado.
    /// </summary>
    public static event Action<GlitchType> OnGlitchDeactivated;

    /// <summary>
    /// Disparado quando todos os glitches são resetados.
    /// </summary>
    public static event Action OnAllGlitchesReset;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        BuildCache();
    }

    void Start()
    {
        // Auto-detecta glitches na cena se a lista estiver vazia
        if (registeredGlitches.Count == 0)
        {
            AutoRegisterGlitches();
        }
    }

    /// <summary>
    /// Reconstrói o cache de glitches por tipo.
    /// </summary>
    private void BuildCache()
    {
        glitchCache.Clear();
        foreach (var glitch in registeredGlitches)
        {
            if (glitch == null) continue;

            if (!glitchCache.ContainsKey(glitch.Type))
            {
                glitchCache[glitch.Type] = new List<GlitchBase>();
            }
            glitchCache[glitch.Type].Add(glitch);
        }
    }

    /// <summary>
    /// Procura e registra automaticamente todos os GlitchBase na cena.
    /// </summary>
    public void AutoRegisterGlitches()
    {
        registeredGlitches.Clear();
        GlitchBase[] foundGlitches = FindObjectsByType<GlitchBase>(FindObjectsSortMode.None);
        registeredGlitches.AddRange(foundGlitches);
        BuildCache();

        if (debugMode)
            Debug.Log($"[GlitchManager] Auto-registrados {registeredGlitches.Count} glitches");
    }

    /// <summary>
    /// Registra um glitch manualmente.
    /// </summary>
    public void RegisterGlitch(GlitchBase glitch)
    {
        if (glitch == null || registeredGlitches.Contains(glitch)) return;

        registeredGlitches.Add(glitch);

        if (!glitchCache.ContainsKey(glitch.Type))
        {
            glitchCache[glitch.Type] = new List<GlitchBase>();
        }
        glitchCache[glitch.Type].Add(glitch);

        if (debugMode)
            Debug.Log($"[GlitchManager] Glitch registrado: {glitch.Type}");
    }

    /// <summary>
    /// Remove um glitch do registro.
    /// </summary>
    public void UnregisterGlitch(GlitchBase glitch)
    {
        if (glitch == null) return;

        registeredGlitches.Remove(glitch);

        if (glitchCache.ContainsKey(glitch.Type))
        {
            glitchCache[glitch.Type].Remove(glitch);
        }
    }

    #region Ativação/Desativação

    /// <summary>
    /// Ativa todos os glitches de um tipo específico.
    /// </summary>
    public void ActivateGlitch(GlitchType type)
    {
        if (!glitchCache.ContainsKey(type) || glitchCache[type].Count == 0)
        {
            if (debugMode)
                Debug.LogWarning($"[GlitchManager] Nenhum glitch do tipo {type} encontrado");
            return;
        }

        foreach (var glitch in glitchCache[type])
        {
            glitch.Activate();
        }

        OnGlitchActivated?.Invoke(type);
    }

    /// <summary>
    /// Desativa todos os glitches de um tipo específico.
    /// </summary>
    public void DeactivateGlitch(GlitchType type)
    {
        if (!glitchCache.ContainsKey(type)) return;

        foreach (var glitch in glitchCache[type])
        {
            glitch.Deactivate();
        }

        OnGlitchDeactivated?.Invoke(type);
    }

    /// <summary>
    /// Ativa um glitch por um tempo determinado, depois desativa.
    /// </summary>
    public void ActivateGlitchForDuration(GlitchType type, float duration)
    {
        StartCoroutine(GlitchDurationRoutine(type, duration));
    }

    private IEnumerator GlitchDurationRoutine(GlitchType type, float duration)
    {
        ActivateGlitch(type);
        yield return new WaitForSeconds(duration);
        DeactivateGlitch(type);
    }

    /// <summary>
    /// Toggle de um tipo de glitch.
    /// </summary>
    public void ToggleGlitch(GlitchType type)
    {
        if (!glitchCache.ContainsKey(type)) return;

        foreach (var glitch in glitchCache[type])
        {
            glitch.Toggle();
        }
    }

    /// <summary>
    /// Ativa múltiplos glitches ao mesmo tempo.
    /// </summary>
    public void ActivateMultipleGlitches(params GlitchType[] types)
    {
        foreach (var type in types)
        {
            ActivateGlitch(type);
        }
    }

    /// <summary>
    /// Desativa múltiplos glitches ao mesmo tempo.
    /// </summary>
    public void DeactivateMultipleGlitches(params GlitchType[] types)
    {
        foreach (var type in types)
        {
            DeactivateGlitch(type);
        }
    }

    /// <summary>
    /// Desativa TODOS os glitches ativos.
    /// </summary>
    public void DeactivateAllGlitches()
    {
        foreach (var glitch in registeredGlitches)
        {
            if (glitch != null && glitch.IsActive)
            {
                glitch.Deactivate();
            }
        }
    }

    /// <summary>
    /// Reseta todos os glitches ao estado inicial.
    /// </summary>
    public void ResetAllGlitches()
    {
        foreach (var glitch in registeredGlitches)
        {
            if (glitch != null)
            {
                glitch.ResetGlitch();
            }
        }

        OnAllGlitchesReset?.Invoke();
    }

    #endregion

    #region Queries

    /// <summary>
    /// Verifica se algum glitch do tipo está ativo.
    /// </summary>
    public bool IsGlitchActive(GlitchType type)
    {
        if (!glitchCache.ContainsKey(type)) return false;

        foreach (var glitch in glitchCache[type])
        {
            if (glitch.IsActive) return true;
        }
        return false;
    }

    /// <summary>
    /// Retorna todos os glitches de um tipo.
    /// </summary>
    public List<GlitchBase> GetGlitchesOfType(GlitchType type)
    {
        if (glitchCache.ContainsKey(type))
            return new List<GlitchBase>(glitchCache[type]);

        return new List<GlitchBase>();
    }

    /// <summary>
    /// Retorna todos os glitches ativos no momento.
    /// </summary>
    public List<GlitchBase> GetActiveGlitches()
    {
        List<GlitchBase> activeGlitches = new List<GlitchBase>();
        foreach (var glitch in registeredGlitches)
        {
            if (glitch != null && glitch.IsActive)
            {
                activeGlitches.Add(glitch);
            }
        }
        return activeGlitches;
    }

    #endregion
}
