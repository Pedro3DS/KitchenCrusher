using Game.Resources;
using UnityEngine;
using UnityEngine.UI;
// using Re

[CreateAssetMenu(fileName = "New Recipe Data", menuName = "Game/Recipes/Recipe Data")]
public class Recipe : ScriptableObject
{
    [Header("Recipe Info")]
    public string recipeName;
    public Sprite recipeIcon;
    public ResourceType[] ingredients;
    public float cookingTime;
}
