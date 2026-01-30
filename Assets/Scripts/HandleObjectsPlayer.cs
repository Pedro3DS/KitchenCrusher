using Unity.VisualScripting;
using UnityEngine;

public class HandleObjectsPlayer : MonoBehaviour
{

    public Transform holdPoint;
    public Ingredient heldIngredient;
    public IngredientInstance heldIngredientInstance;

    public bool IsHoldingIngredient() => heldIngredient != null;

    // Novo: Verifica se o que o player tem na mão já está cozido
    public bool IsHoldingCooked() => heldIngredientInstance != null && heldIngredientInstance.isCooked;

    void Update()
    {
        // Verifica se apertou o botão de Drop (East no controle ou Q no teclado)
        if (Input.GetKeyDown(KeyCode.Q) ||
           (JoystickController.Instance != null && JoystickController.Instance.IsEastButtonPressed()))
        {
            DropItem();
        }
    }

    public void DropItem()
    {
        if (holdPoint.childCount == 0) return;

        // Pega o objeto que está na mão
        Transform itemTransform = holdPoint.GetChild(0);
        itemTransform.SetParent(null); // Solta do player

        // 1. Reativa a física para cair no chão
        Rigidbody rb = itemTransform.GetComponent<Rigidbody>();
        if (rb == null) rb = itemTransform.gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        // 2. Reativa o colisor para poder ser pego de novo
        Collider col = itemTransform.GetComponent<Collider>();
        if (col != null) col.enabled = true;
        if(col.isTrigger == true) col.isTrigger = false;

        // 3. Se for um prato, avisa que não está mais sendo segurado
        Plate plate = itemTransform.GetComponent<Plate>();
        if (plate != null) {
            plate.isHeld = false;
            rb.constraints = RigidbodyConstraints.None;
            }

        // Limpa as referências do player
        heldIngredient = null;
        heldIngredientInstance = null;
    }

    // Função auxiliar que você já usa para pegar coisas
    public void HoldIngredient(Ingredient ingredient, bool cooked)
    {
        ClearHeldItem();
        heldIngredient = ingredient;

        GameObject prefab = cooked ? ingredient.cookedPrefab : ingredient.rawPrefab;
        GameObject instance = Instantiate(prefab, holdPoint.position, holdPoint.rotation, holdPoint);

        
        // Ao segurar, removemos a física para não bugar o movimento do player
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        heldIngredientInstance = instance.GetComponent<IngredientInstance>();
        if (heldIngredientInstance == null) heldIngredientInstance = instance.AddComponent<IngredientInstance>();
        heldIngredientInstance.Setup(ingredient, cooked);
    }

    public void ClearHeldItem()
    {
        heldIngredient = null;
        heldIngredientInstance = null;
        foreach (Transform child in holdPoint) Destroy(child.gameObject);
    }
}
