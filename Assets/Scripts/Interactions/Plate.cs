using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [System.Serializable]
    public struct PlateContent
    {
        public Ingredient data;
        public bool isCooked;
    }
    [Header("Visual Slots")]
    [SerializeField] private Transform[] ingredientSlots; // tamanho 3
    [SerializeField] private int maxIngredients = 3;

    public List<PlateContent> ingredients = new List<PlateContent>();
    public bool isHeld = false; // Para o prato saber se está na mão de alguém

    private bool interactionLocked; // Trava para o cooldown

    public bool AddIngredient(Ingredient ingredient, bool cooked)
    {
        if (ingredients.Count >= maxIngredients)
        {
            Debug.Log("Prato cheio!");
            return false;
        }

        // Dados
        ingredients.Add(new PlateContent
        {
            data = ingredient,
            isCooked = cooked
        });

        // Visual
        Transform slot = ingredientSlots[ingredients.Count - 1];

        GameObject prefab = cooked
            ? ingredient.cookedPrefab
            : ingredient.rawPrefab;

        Instantiate(prefab, slot.position, slot.rotation, slot);

        GameEvents.OnIngredientAddedToPlate?.Invoke(ingredient);

        Debug.Log($"Adicionado ao prato: {ingredient.name} no slot {ingredients.Count - 1}");
        return true;
    }

    // Verifica se todos os ingredientes no prato estão cozidos
    public bool IsPlateReady()
    {
        if (ingredients.Count == 0) return false;
        foreach (var item in ingredients)
        {
            if (!item.isCooked) return false; // Se achar um cru, o prato não está pronto
        }
        return true;
    }

    public int CountCookedIngredient(Ingredient target)
    {
        int count = 0;
        foreach (var item in ingredients)
        {
            if (item.data == target && item.isCooked) count++;
        }
        return count;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Handler") || isHeld || interactionLocked || IsFull()) return;

        HandleObjectsPlayer handler = other.GetComponent<HandleObjectsPlayer>();
        if (handler == null) return;
        Debug.Log("Player near plate.");

        if (InteractPressed())
        {
            // CASO 1: Player tem algo na mão -> Coloca no prato
            if (handler.IsHoldingIngredient())
            {
                bool added = AddIngredient(handler.heldIngredient, handler.heldIngredientInstance.isCooked);

                if (added)
                {
                    handler.ClearHeldItem();
                }
            }
            // CASO 2: Player com mão vazia -> Pega o prato
            else
            {
                PickUpPlate(handler);
            }
        }
    }

    void PickUpPlate(HandleObjectsPlayer handler)
    {
        isHeld = true;
        transform.SetParent(handler.holdPoint);
        transform.localPosition = Vector3.zero;

        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        // transform.localRotation = Quaternion.identity;
        // Desativa o collider para não atrapalhar outras interações enquanto carrega
        GetComponent<Collider>().enabled = false;
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
    public bool IsFull()
    {
        return ingredients.Count > maxIngredients;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            Destroy(this.gameObject);
            GameEvents.OnPlateBraked?.Invoke();
            // GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        if (collision.gameObject.CompareTag("Ground") && !isHeld)
        {
            Debug.Log("Prato Quebrou!");
            Destroy(this.gameObject);
            GameEvents.OnPlateBraked?.Invoke();
        }
    }
}
