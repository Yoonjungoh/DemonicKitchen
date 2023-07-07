using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public static bool increaseEnhance;
    public static bool spendingNone;

    public static void increaseEnhanceProb()
    {
        increaseEnhance = true;
    }

    public static void spendNoIngredient()
    { 
        spendingNone = true;
    }
}
