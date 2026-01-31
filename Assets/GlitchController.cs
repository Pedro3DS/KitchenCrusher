using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class GlitchController : MonoBehaviour
{

    [Header("Câmeras e Máscaras")]
    [SerializeField] private Camera playerCamera; // A câmera normal do player
    [SerializeField] private Camera[] glitchCameras;
    [SerializeField] private GameObject[] glitchCamerasMask;

    [Header("Post Processing")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private VolumeProfile normalProfile;

    [Header("Configurações")]
    [SerializeField] private float glitchDuration = 5f;

    [SerializeField] private GameObject topBox;
    
    public int currentCameraIndex = 0;

    public UnityEvent events;

    void OnEnable()
    {
        GameManager.OnChangeToMomView += ActivateGlitchMode;
    }

    void OnDisable()
    {
        GameManager.OnChangeToMomView -= ActivateGlitchMode;
    }

   public void ActivateGlitchMode()
    {
        // Se ainda houver câmeras na lista, inicia o efeito
        if (currentCameraIndex < glitchCameras.Length)
        {
            StartCoroutine(TemporaryGlitchEffect());
        }
        else
        {
            EndGame();
        }
    }

    IEnumerator TemporaryGlitchEffect()
    {
        // 1. Desativa a câmera do player
        playerCamera.enabled = false;
        playerCamera.tag = "Untagged";
        topBox.SetActive(true);

        // 2. Prepara a câmera de glitch atual
        Camera currentGlitchCam = glitchCameras[currentCameraIndex];
        currentGlitchCam.gameObject.SetActive(true);
        currentGlitchCam.enabled = true;
        currentGlitchCam.tag = "MainCamera"; // Torna ela a principal

        // 3. Ativa a máscara correspondente
        if(glitchCamerasMask.Length > currentCameraIndex)
            glitchCamerasMask[currentCameraIndex].SetActive(true);

        // 4. Ativa o efeito de Volume
        if (globalVolume != null && volumeProfile != null)
        {
            globalVolume.profile = volumeProfile;
        }

        // Espera pelo tempo determinado
        yield return new WaitForSeconds(glitchDuration);

        // --- RETORNO OU PRÓXIMO ---

        // 5. Desativa a câmera de glitch e sua máscara
        currentGlitchCam.enabled = false;
        currentGlitchCam.tag = "Untagged";
        currentGlitchCam.gameObject.SetActive(false);
        
        if(glitchCamerasMask.Length > currentCameraIndex)
            glitchCamerasMask[currentCameraIndex].SetActive(false);

        // 6. Retorna para a câmera do player
        topBox.SetActive(false);
        playerCamera.enabled = true;
        playerCamera.tag = "MainCamera";

        // 7. Retorna o perfil normal
        if (globalVolume != null && normalProfile != null)
        {
            globalVolume.profile = normalProfile;
        }

        // 8. Incrementa o index para a próxima vez que o evento for chamado
        currentCameraIndex++;
        events.Invoke();
        // Se após incrementar chegamos ao fim, podemos encerrar ou esperar o próximo evento
        if (currentCameraIndex >= glitchCameras.Length)
        {
            
        }
    }

    void EndGame()
    {
        Debug.Log("Encerrando o jogo...");
        events.Invoke();
        // Aqui você pode carregar uma cena de créditos ou fechar o app
       // Application.Quit(); 
        
        // Se estiver no editor do Unity, isso ajuda a testar:
        //#if UNITY_EDITOR
       // UnityEditor.EditorApplication.isPlaying = false;
        //#endif
    }
}
