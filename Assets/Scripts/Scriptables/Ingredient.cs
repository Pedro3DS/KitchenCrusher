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

    [Header("Prefabs")]
    public GameObject rawPrefab;
    public GameObject cookedPrefab;

    [Header("Cooking")]
    public ResourceType ingredientType;
    public float cookingTime;      // tempo até ficar pronto
    public float burnTime = 3f;    // tempo extra até queimar
    public bool needCooking;
}
