using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour
{
    public IngredientsSO[] ingredients;
    public static IngredientsSO[] ingredientsStatic;
    //매 플레이마다 초기화
    public static List<int> addedIngredientCount = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        if (ingredientsStatic == null)
        {
            ingredientsStatic = new IngredientsSO[ingredients.Length];
        }

        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredientsStatic[i] = ingredients[i];
        }

        PlayerDataSaver.LoadAll();
    }

}
