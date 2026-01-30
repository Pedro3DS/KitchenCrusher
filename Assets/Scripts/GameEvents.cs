using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    // Cozinha
    public static Action<Ingredient> OnIngredientCooked;
    public static Action<Ingredient> OnIngredientBurned;

    // Interações
    public static Action<Ingredient> OnIngredientPicked;
    public static Action<Ingredient> OnIngredientDiscarded;

    // Prato
    public static Action<Ingredient> OnIngredientAddedToPlate;

    // Entrega / Balcão
    public static Action<Plate> OnPlateDelivered;

    // Tasks
    public static Action<KitchenTask> OnTaskCompleted;
}
