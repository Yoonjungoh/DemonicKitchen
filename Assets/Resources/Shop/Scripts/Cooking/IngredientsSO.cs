using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RecipeIngredient", menuName = "Scriptable Objects/New Ingredient", order = 2)]
public class IngredientsSO : ScriptableObject
{
    public int ID;
    public Sprite image;
    public string title;
    public string description;
    public string rank;
    public int salesPrice;
    public bool isMainIngredient;
    public bool isSeasoning;
    public bool isBasicIngredient;
    public bool isSpecialIngredient;
}
