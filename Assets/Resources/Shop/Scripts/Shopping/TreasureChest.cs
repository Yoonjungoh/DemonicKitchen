using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public static void randomBasicIngredientDraw(int qty)
    {
        if (qty > 0)
        {
            for (int i = 0; i < qty; i++)
            {
                int rnd = Random.Range(0, 8);
                //Add ingredient using the rnd as the index
            }
        }
    }

    public static void randomSeasoningDraw(int qty)
    {
        if (qty > 0)
        {
            for (int i = 0; i < qty; i++)
            {
                int rnd = Random.Range(9, 14);
                //Add ingredient using the rnd as the index
            }
        }
    }
}
