using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void RewardBeginnerPackage()
    {
        GiveGold(10000);
        GiveRuby(300);
        GiveSeasoningIngredient(3);
        GiveBasicIngredient(3);
        GiveEScroll(1);
    }

    public static void RewardDevildomPackage()
    {
        GiveGold(20000);
        GiveRuby(400);
        GiveSeasoningIngredient(1);
        GiveBasicIngredient(1);
    }

    public static void RewardWorldPackage()
    {
        GiveGold(35000);
        GiveRuby(550);
        GiveSeasoningIngredient(2);
        GiveBasicIngredient(2);
        GiveCrystal(3);
    }
    public static void RewardHeaven5Package()
    {
        GiveGold(50000);
        GiveRuby(700);
        GiveEScroll(1);
        GiveCrystal(3);
    }

    public static void RewardHeaven10Package()
    {
        GiveGold(70000);
        GiveRuby(900);
        GiveEScroll(1);
        GiveIScroll(1);
        GiveCrystal(7);
    }
    public static void RewardHeaven15Package()
    {
        GiveGold(100000);
        GiveRuby(1000);
        GiveEScroll(2);
        GiveIScroll(2);
        GiveCrystal(10);
    }

    public static void GiveGold(int gold)
    {
        FinanceManager.coins += gold;
        PlayerDataSaver.SaveAll();
    }

    public static void GiveRuby(int ruby)
    {
        FinanceManager.ruby += ruby;
        PlayerDataSaver.SaveAll();
    }

    public static void GiveCrystal(int crystal)
    {
        FinanceManager.crystal += crystal;
        PlayerDataSaver.SaveAll();
    }

    public static void GiveEScroll(int scroll)
    {
        FinanceManager.enhanceScroll += scroll;
        PlayerDataSaver.SaveAll();
    }

    public static void GiveIScroll(int scroll)
    {
        FinanceManager.ingredientScroll += scroll;
        PlayerDataSaver.SaveAll();
    }

    public static void GiveBasicIngredient(int multiplier)
    {
        for (int i = 0; i < multiplier; i++)
        {
            int id = Random.Range(0,8);
            if (InventoryManager.ingredientInventory.ContainsKey(InventoryManager.ingredientSOsStatic[id]))
            {
                InventoryManager.ingredientInventory.Add(InventoryManager.ingredientSOsStatic[id],1);
            }
            else
            {
                int count;
                InventoryManager.ingredientInventory.TryGetValue(InventoryManager.ingredientSOsStatic[id],out count);
                InventoryManager.ingredientInventory[InventoryManager.ingredientSOsStatic[id]] = count + 1;
            }
        }
        PlayerDataSaver.SaveAll();
    }

    public static void GiveSeasoningIngredient(int multiplier)
    {
        for (int i = 0; i < multiplier; i++)
        {
            int id = Random.Range(9, 14);
            if (InventoryManager.ingredientInventory.ContainsKey(InventoryManager.ingredientSOsStatic[id]))
            {
                InventoryManager.ingredientInventory.Add(InventoryManager.ingredientSOsStatic[id], 1);
            }
            else
            {
                int count;
                InventoryManager.ingredientInventory.TryGetValue(InventoryManager.ingredientSOsStatic[id], out count);
                InventoryManager.ingredientInventory[InventoryManager.ingredientSOsStatic[id]] = count + 1;
            }
        }
        PlayerDataSaver.SaveAll();
    }
}
