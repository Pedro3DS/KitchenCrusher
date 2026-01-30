using System.Collections;
using UnityEngine;

public class DeliveryCounter : MonoBehaviour
{
    private bool interactionLocked; // Trava para o cooldown
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Handler") || interactionLocked) return;

        HandleObjectsPlayer handler = other.GetComponent<HandleObjectsPlayer>();
        if (handler == null) return;

        // Tenta encontrar um prato no HoldPoint do Player
        Plate plate = handler.holdPoint.GetComponentInChildren<Plate>();

        if (plate != null && InteractPressed())
        {
            if (plate.IsPlateReady())
            {
                Debug.Log("Pedido Entregue com Sucesso!");
                GameEvents.OnPlateDelivered?.Invoke(plate);
                Destroy(plate.gameObject); // Remove o prato após entregar
            }
            else
            {
                Debug.Log("ERRO: O prato contém ingredientes crus!");
                // Aqui você pode tocar um som de erro
            }
        }
    }

    bool InteractPressed()
    {
        if (interactionLocked) return false;

        bool buttonPressed = Input.GetKeyDown(KeyCode.E) ||
                             (JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed());

        if (buttonPressed)
        {
            StartCoroutine(InteractionCooldown());
            return true;
        }
        return false;
    }

    private IEnumerator InteractionCooldown()
    {
        interactionLocked = true;
        yield return new WaitForSeconds(0.2f); // Tempo de espera para a próxima ação
        interactionLocked = false;
    }
}
