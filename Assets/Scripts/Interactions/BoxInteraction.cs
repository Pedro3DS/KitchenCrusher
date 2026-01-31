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
                if (Input.GetKeyDown(KeyCode.E) || JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed())
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

}
