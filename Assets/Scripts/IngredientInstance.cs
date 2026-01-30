using UnityEngine;

public class IngredientInstance : MonoBehaviour
{
    public Ingredient data;
    public bool isCooked;
    public bool isBurned;

    public void Setup(Ingredient ingredient, bool cooked)
    {
        data = ingredient;
        isCooked = cooked;
        isBurned = false;
    }
    void OnTriggerStay(Collider other)
    {
        // Se o player estiver perto, não estiver segurando nada e apertar o botão de pegar
        if (other.CompareTag("Handler"))
        {
            HandleObjectsPlayer handler = other.GetComponent<HandleObjectsPlayer>();

            if (handler != null && !handler.IsHoldingIngredient() && InteractPressed())
            {
                // Em vez de instanciar um novo, "anexamos" este mesmo objeto à mão
                PickUp(handler);
            }
        }
    }

    void PickUp(HandleObjectsPlayer handler)
    {
        transform.SetParent(handler.holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Desativa física enquanto segura
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Configura as referências no player
        handler.heldIngredient = this.data;
        handler.heldIngredientInstance = this;

        // Desativa o colisor trigger temporariamente para não interferir
        GetComponent<Collider>().enabled = false;
    }

    bool InteractPressed() => Input.GetKeyDown(KeyCode.E) ||
                             (JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed());
}
