using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public RecipeManager recipeManager;
    public FinanceManager financeManager;
    public Stat statManager;
    public bool isUsingRecipeCheat;
    // Start is called before the first frame update
    void Start()
    {
        // TODO DELETE
        statManager = GameObject.Find("@StatManager").GetComponent<Stat>();
        isUsingRecipeCheat = false;
    }

    void Update()
    {
        
    }

    public void increaseCoins()
    {
        FinanceManager.coins += 10000;
        PlayerDataSaver.SaveMoney();
        financeManager.syncCoinUI();
    }

    public void incrementInventory()
    {
        for (int i = 0; i < InventoryManager.ingredientInventory.Count; i++)
        {
            InventoryManager.ingredientInventory[inventoryManager.ingredientsSOs[i]]++;
            inventoryManager.UpdateUICount(i);
        }
    }

    public void togglePerfectChance()
    {
        if (isUsingRecipeCheat)
        {
            isUsingRecipeCheat = false;
        }
        else 
        {
            isUsingRecipeCheat = true;
        }
    }
    // TODO DELETE
    public void DeleteShopStat()
    {
        // statManagerÀÇ ½ºÅÈ
        PlayerPrefs.SetInt("PlayerMaxHP", 0);
        PlayerPrefs.SetFloat("PlayerHP", 0);
        PlayerPrefs.SetInt("PlayerAttack", 0);
        PlayerPrefs.SetInt("PlayerDefense", 0);
        PlayerPrefs.SetFloat("PlayerMaxSpeed", 0);

        statManager.GetComponent<Stat>()._maxHp = PlayerPrefs.GetInt("PlayerMaxHP");
        statManager.GetComponent<Stat>()._hp = PlayerPrefs.GetFloat("PlayerHP");
        statManager.GetComponent<Stat>()._attack = PlayerPrefs.GetInt("PlayerAttack");
        statManager.GetComponent<Stat>()._defense = PlayerPrefs.GetInt("PlayerDefense");
        statManager.GetComponent<Stat>()._maxSpeed = PlayerPrefs.GetFloat("PlayerMaxSpeed");

        PlayerPrefs.Save();
    }

}
