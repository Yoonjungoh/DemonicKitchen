using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public FinanceManager financeManager;
    public SpecialAbilities[] abilities;
    public SpecialAbilities currentAbility;

    public GameObject[] abilityButtonsGO;

    private MenuSO currentMenuSO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setCurrentMenuSO()
    { 
        //Set current menu we want to spend as a cost
    }

    public void setCurrentAbility()
    { 
        //Set current ability we are on
    }

    public void tryUnlock()
    {
        if (Dods_ChanceMaker.GetThisChanceResult_Percentage(currentAbility.prob))
        {
            int count;
            RecipeManager.foodInventory.TryGetValue(currentMenuSO, out count);
            RecipeManager.foodInventory[currentMenuSO] = count - 1;
            FinanceManager.coins -= currentAbility.unlockCost;
            PlayerDataSaver.SaveMoney();
            financeManager.syncCoinUI();
            currentAbility.isUnlocked = true;
            //Save the boolean value of the unlocking for loading in future
            //refresh whatever UI
        }
        else
        {
            int count;
            RecipeManager.foodInventory.TryGetValue(currentMenuSO, out count);
            RecipeManager.foodInventory[currentMenuSO] = count - 1;
            FinanceManager.coins -= currentAbility.unlockCost;
            PlayerDataSaver.SaveMoney();
            financeManager.syncCoinUI();
        }
    }
}
