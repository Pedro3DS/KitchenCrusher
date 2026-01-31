using UnityEngine;


public class DorInteraction : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioSource audioSource;

    [Header("Sons Normais")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    [Header("Sons de Terror/Glitch")]
    [SerializeField] private AudioClip horrorSound;
    [SerializeField] private bool useHorrorSound = false;

    private void OnEnable()
    {
        // Se o evento de glitch for disparado, a porta entra em modo terror
        GameEvents.OnHandleTrigger += ActivateGlitchMode;
    }

    private void OnDisable()
    {
        GameEvents.OnHandleTrigger -= ActivateGlitchMode;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        doorAnimator.SetBool("isOpen", true);

        // Se estiver em modo glitch, toca o som de terror, senão o normal
        if (useHorrorSound && horrorSound != null)
        {
            audioSource.PlayOneShot(horrorSound);
        }
        else if (openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    private void CloseDoor()
    {
        doorAnimator.SetBool("isOpen", false);

        if (closeSound != null && !useHorrorSound)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }

    // Chamado pelo evento do sistema de estresse ou glitch
    public void ActivateGlitchMode()
    {
        useHorrorSound = true;
        Debug.Log("Porta agora emitirá sons de glitch.");
    }
}
