using UnityEngine;

/// <summary>
/// Glitch que troca a câmera ativa do jogo.
/// Pode alternar entre diferentes câmeras ou ativar uma câmera específica de glitch.
/// </summary>
public class CameraSwitchGlitch : GlitchBase
{
    [Header("Configuração de Câmera")]
    [SerializeField] private Camera normalCamera;
    [SerializeField] private Camera glitchCamera;

    [Header("Opções")]
    [SerializeField] private bool disablePlayerControl = false;
    [SerializeField] private MonoBehaviour playerMovementScript;

    private void Awake()
    {
        glitchType = GlitchType.CameraSwitch;
    }

    private void Start()
    {
        // Registra automaticamente no GlitchManager
        if (GlitchManager.Instance != null)
        {
            GlitchManager.Instance.RegisterGlitch(this);
        }
    }

    protected override void OnActivate()
    {
        // Desativa câmera normal e ativa câmera de glitch
        if (normalCamera != null)
            normalCamera.gameObject.SetActive(false);

        if (glitchCamera != null)
            glitchCamera.gameObject.SetActive(true);

        // Opcionalmente desativa controle do jogador
        if (disablePlayerControl && playerMovementScript != null)
            playerMovementScript.enabled = false;
    }

    protected override void OnDeactivate()
    {
        // Restaura câmeras ao normal
        if (normalCamera != null)
            normalCamera.gameObject.SetActive(true);

        if (glitchCamera != null)
            glitchCamera.gameObject.SetActive(false);

        // Restaura controle do jogador
        if (disablePlayerControl && playerMovementScript != null)
            playerMovementScript.enabled = true;
    }

    /// <summary>
    /// Define as câmeras em runtime se necessário.
    /// </summary>
    public void SetCameras(Camera normal, Camera glitch)
    {
        normalCamera = normal;
        glitchCamera = glitch;
    }
}
