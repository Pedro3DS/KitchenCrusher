using UnityEngine;

public class HandleObjectsPlayer : MonoBehaviour
{

    public Transform holdPoint;
    public Ingredient heldIngredient;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Debug.Log("Player entered box interaction area.");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (heldIngredient != null) return;
        if (other.CompareTag("Box"))
        {
            Debug.Log("Player stayed in box interaction area.");
            if (Input.GetKeyDown(KeyCode.E) || JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed())
            {
                Debug.Log("Player interacted with the box.");
                BoxInteraction boxInteraction = other.GetComponent<BoxInteraction>();
                if (boxInteraction != null)
                {
                    heldIngredient = boxInteraction.containedIngredient;
                    Debug.Log("Player picked up: " + heldIngredient.ingredientName);
                    Instantiate(heldIngredient.ingredientPrefab, holdPoint.position, Quaternion.identity, holdPoint);
                }
            }
        }

    }
}
