using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum CookingState { Idle, Cooking, Cooked, Burned }
public class CookingStation : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image ingredientIcon;
    [SerializeField] private Slider cookingSlider;
    [SerializeField] private Image sliderFill;

    [Header("Runtime")]
    [SerializeField] private Ingredient currentIngredient;
    private float timer;
    private CookingState state = CookingState.Idle;

    void Update()
    {
        if (state == CookingState.Idle || currentIngredient == null) return;

        timer += Time.deltaTime;
        cookingSlider.value = timer;

        if (state == CookingState.Cooking && timer >= currentIngredient.cookingTime)
        {
            state = CookingState.Cooked;
            // Opcional: GameEvents.OnIngredientCooked?.Invoke(currentIngredient);
        }

        if (timer >= currentIngredient.cookingTime + currentIngredient.burnTime && state != CookingState.Burned)
        {
            state = CookingState.Burned;
            ClearStation();
        }

        UpdateSliderColor();
    }

    void UpdateSliderColor()
    {
        if (state == CookingState.Cooking) sliderFill.color = Color.green;
        else if (state == CookingState.Cooked) sliderFill.color = Color.yellow;
        else if (state == CookingState.Burned) sliderFill.color = Color.red;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Handler")) return; // Certifique-se que a tag do Player está correta

        HandleObjectsPlayer handler = other.GetComponent<HandleObjectsPlayer>();
        if (handler == null) return;
        Debug.Log("Player in cooking station area.");

        if (InteractPressed())
        {
            Debug.Log("Player in cooking station area.");
            // CASO 1: Forno vazio e Player tem item CRU -> Colocar no forno
            if (state == CookingState.Idle && handler.IsHoldingIngredient() && !handler.IsHoldingCooked())
            {
                PlaceIngredient(handler.heldIngredient);
                handler.ClearHeldItem();
            }
            // CASO 2: Forno tem item COZIDO e Player está com a mão vazia -> Pegar do forno
            else if ((state == CookingState.Cooked || state == CookingState.Burned) && !handler.IsHoldingIngredient())
            {
                bool isBurned = (state == CookingState.Burned);
                handler.HoldIngredient(currentIngredient, !isBurned); // Se queimou, você decide se quer dar um item "cozido" ou deletar
                ClearStation();
            }
        }
    }

    public void PlaceIngredient(Ingredient ingredient)
    {
        currentIngredient = ingredient;
        timer = 0f;
        state = CookingState.Cooking;
        ingredientIcon.sprite = ingredient.ingredientIcon;
        ingredientIcon.enabled = true;
        cookingSlider.maxValue = ingredient.cookingTime + ingredient.burnTime;
        cookingSlider.gameObject.SetActive(true);
    }

    void ClearStation()
    {
        currentIngredient = null;
        timer = 0f;
        state = CookingState.Idle;
        ingredientIcon.enabled = false;
        // cookingSlider.gameObject.SetActive(false);
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
