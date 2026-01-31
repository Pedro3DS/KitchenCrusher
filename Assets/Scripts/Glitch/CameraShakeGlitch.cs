using System.Collections;
using UnityEngine;

/// <summary>
/// Glitch que faz a câmera tremer.
/// Útil para momentos de tensão ou horror.
/// </summary>
public class CameraShakeGlitch : GlitchBase
{
    [Header("Configuração do Tremor")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float shakeIntensity = 0.1f;
    [SerializeField] private float shakeSpeed = 10f;

    [Header("Intensidade Progressiva")]
    [SerializeField] private bool useProgressiveIntensity = false;
    [SerializeField] private float maxIntensity = 0.5f;
    [SerializeField] private float intensityGrowthRate = 0.1f;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;
    private float currentIntensity;

    private void Awake()
    {
        glitchType = GlitchType.CameraShake;
    }

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main?.transform;

        if (cameraTransform != null)
            originalPosition = cameraTransform.localPosition;

        if (GlitchManager.Instance != null)
            GlitchManager.Instance.RegisterGlitch(this);
    }

    protected override void OnActivate()
    {
        if (cameraTransform == null) return;

        originalPosition = cameraTransform.localPosition;
        currentIntensity = shakeIntensity;
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    protected override void OnDeactivate()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        if (cameraTransform != null)
            cameraTransform.localPosition = originalPosition;
    }

    private IEnumerator ShakeRoutine()
    {
        while (isActive)
        {
            float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * 2f - 1f;

            cameraTransform.localPosition = originalPosition + new Vector3(
                offsetX * currentIntensity,
                offsetY * currentIntensity,
                0f
            );

            if (useProgressiveIntensity)
            {
                currentIntensity = Mathf.Min(currentIntensity + intensityGrowthRate * Time.deltaTime, maxIntensity);
            }

            yield return null;
        }
    }

    /// <summary>
    /// Define a intensidade do tremor em runtime.
    /// </summary>
    public void SetIntensity(float intensity)
    {
        shakeIntensity = intensity;
        if (isActive)
            currentIntensity = intensity;
    }
}
