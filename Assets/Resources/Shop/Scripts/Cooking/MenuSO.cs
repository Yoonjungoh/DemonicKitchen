using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeMenu", menuName = "Scriptable Objects/New Menu", order = 3)]
public class MenuSO : ScriptableObject
{
    public int ID;
    public Sprite image;
    public string title;
    public string description;
    public string rank;
    public float successRate;
    public int cost;
    public List<IngredientsSO> ingridientNeeded = new List<IngredientsSO>();
}
