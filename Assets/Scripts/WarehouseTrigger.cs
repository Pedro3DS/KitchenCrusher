using UnityEngine;

public class WarehouseTrigger : MonoBehaviour
{
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            // Verifica se o jogador está sob estresse (opcional, mas recomendado)
            // Aqui chamamos o evento que muda a visão para a Mãe
            TriggerGlitch();
            triggered = true;
        }
    }

    void TriggerGlitch()
    {
        Debug.Log("Glitch Ativado: Visão da Mãe");
        
        // Usando o seu script ChangePersonView para trocar a câmera
        if (ChangePersonView.Instance != null)
        {
            // Aqui assumimos que TopDown ou uma nova câmera representa a visão real da mãe
            GameManager.OnChangeToMomView?.Invoke(); 
            
            // Aqui você ativaria seu Shader de PS1 e desativaria os objetos do "Filho"
            ApplyPS1HorrorEffects();
        }
    }

    void ApplyPS1HorrorEffects()
    {
        // 1. Desativar o modelo visual do jogador (ele é um fantasma que sumiu)
        // 2. Ativar o volume de Post-Processing do Shader PS1
        // 3. Tocar um som perturbador (estática ou silêncio ensurdecedor)
    }
}