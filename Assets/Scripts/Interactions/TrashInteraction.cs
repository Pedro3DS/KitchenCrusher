using UnityEngine;

public class TrashInteraction : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Handler"))
        {
            Debug.Log("Player stayed in trash interaction area.");
            if (Input.GetKeyDown(KeyCode.E) || JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed())
            {
                HandleObjectsPlayer handleObjectsPlayer = other.GetComponent<HandleObjectsPlayer>();
                Debug.Log("Player interacted with the trash.");
                
                if (handleObjectsPlayer.heldIngredient != null) handleObjectsPlayer.ClearHeldItem();

            }
        }
    }
}
