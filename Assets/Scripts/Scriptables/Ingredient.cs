using Game.Resources;
using UnityEngine;
using UnityEngine.UI;
// using Re

[CreateAssetMenu(fileName = "New Ingredient Data", menuName = "Game/Ingredients/Ingredient Data")]
public class Ingredient : ScriptableObject
{
    [Header("Ingredient Info")]
    public string ingredientName;
    public Sprite ingredientIcon;
    public GameObject ingredientPrefab;
    public ResourceType ingredientType;
    public float cookingTime;
    public bool needCooking;
}
