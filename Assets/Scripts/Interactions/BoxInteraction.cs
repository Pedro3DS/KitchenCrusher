using System.Collections;
using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    public Ingredient containedIngredient;

    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Handler"))
        {
            HandleObjectsPlayer handleObjectsPlayer = other.GetComponent<HandleObjectsPlayer>();
            GameEvents.OnHandleTrigger?.Invoke();
            if (handleObjectsPlayer != null && handleObjectsPlayer.heldIngredient == null)
            {
                if (InteractPressed())
                {
                    handleObjectsPlayer.HoldIngredient(containedIngredient, false);
                    Debug.Log("Picked up ingredient from box: " + containedIngredient.ingredientName);
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Handler"))
        {
            GameEvents.OnHandleTriggerRelease?.Invoke();
        }
    }
    // Lógica do Botão (E ou Joystick)
    private bool interactionLocked;
    bool InteractPressed()
    {
        if (interactionLocked) return false;
        if (Input.GetKeyDown(KeyCode.E)|| JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed()) // Adicione seu Joystick aqui
        {
            StartCoroutine(InteractionCooldown());
            return true;
        }
        return false;
    }
    IEnumerator InteractionCooldown()
    {
        interactionLocked = true;
        yield return new WaitForSeconds(0.2f);
        interactionLocked = false;
    }

}
