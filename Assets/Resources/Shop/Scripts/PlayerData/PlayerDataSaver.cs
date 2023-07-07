using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSaver : MonoBehaviour
{
    //플레이어 골드
    //플레이어 재료 인벤토리 갯수
    //플레이어 레시피 북 해금 진행도
    private static bool s_bLoadedInven = false;
    private static bool s_bLoadedRecipe = false;
    private static bool s_bLoadedFoodInven = false;
    private static bool s_bLoadedFoodList = false;
    public static float highestScore;
    public static bool s_hasSeenKitchen = false;
    public static bool s_hasSeenSkills = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    //게임이 꺼질때 파괴되면서 자동으로 저장
    private void OnDestroy()
    {
        SaveAll();
    }

    public static void SaveAll()
    { 
        SaveMoney();
        SaveRuby();
        SaveCrystal();
        SaveScroll();
        if (InventoryManager.ingredientInventory != null)
        {
            SaveInventory();
        }

        if (RecipeCreator.haveCreatedOnce != null)
        {
            SaveRecipeCreation();
        }

        if (RecipeManager.foodInventory != null)
        {
            SaveFoodInventory();
        }

        if (RecipeManager.completeFoodList != null)
        {
            SaveFoodList();
        }

        if (highestScore > 0)
        {
            SaveScore();
        }

        SaveSeenKitchen();
        SaveSeenSkills();
    }

    public static void LoadAll()
    {
        LoadCrystal();
        LoadFoodInventory();
        LoadFoodList();
        LoadInventory();
        LoadMoney();
        LoadRecipeCreation();
        LoadRuby();
        LoadScroll();
        LoadSeenKitchen();
        LoadSeenSkills();
    }

    public static void SaveSeenKitchen()
    {
        if (s_hasSeenKitchen)
        {
            PlayerPrefs.SetInt("KitchenTutorial: ", 1);
            PlayerPrefs.Save();
        }
        else 
        {
            PlayerPrefs.SetInt("KitchenTutorial: ", 0);
            PlayerPrefs.Save();
        }
    }

    public static bool LoadSeenKitchen()
    {
        if (PlayerPrefs.GetInt("KitchenTutorial: ") > 0)
        {
            s_hasSeenKitchen=true;
            return true;
        }
        else 
        {
            s_hasSeenKitchen = false;
            return false;
        }
    }

    public static void SaveSeenSkills()
    {
        if (s_hasSeenSkills)
        {
            PlayerPrefs.SetInt("SkillsTutorial: ", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("SkillsTutorial: ", 0);
            PlayerPrefs.Save();
        }
    }

    public static bool LoadSeenSkills()
    {
        if (PlayerPrefs.GetInt("SkillsTutorial: ") > 0)
        {
            s_hasSeenSkills = true;
            return true;
        }
        else
        {
            s_hasSeenSkills = false;
            return false;
        }
    }

    public static void LoadScenario()
    {
        GameManager.Scenario = PlayerPrefs.GetInt("Scenario");
    }

    public static void SaveScore()
    {
        PlayerPrefs.SetFloat("HighestScore", highestScore);
        PlayerPrefs.Save();
    }

    public static float LoadScore()
    {
        return PlayerPrefs.GetFloat("HighestScore");
    }

    public static void SaveMoney()
    {
        PlayerPrefs.SetInt("PlayerGold", FinanceManager.coins);
        PlayerPrefs.Save();
    }

    //Called in InventoryManager
    public static void LoadMoney()
    {
        if (!PlayerPrefs.HasKey("PlayerGold"))
        {
            FinanceManager.coins = 0;
        }
        else 
        {
            FinanceManager.coins = PlayerPrefs.GetInt("PlayerGold");
        }
    }

    public static void SaveRuby()
    {
        PlayerPrefs.SetInt("PlayerRuby", FinanceManager.ruby);
        PlayerPrefs.Save();
    }

    public static void LoadRuby()
    {
        if (!PlayerPrefs.HasKey("PlayerRuby"))
        {
            FinanceManager.ruby = 0;
        }
        else
        {
            FinanceManager.ruby = PlayerPrefs.GetInt("PlayerRuby");
        }
    }

    public static void SaveCrystal()
    {
        PlayerPrefs.SetInt("PlayerCrystal", FinanceManager.crystal);
        PlayerPrefs.Save();
    }

    public static void LoadCrystal()
    {
        if (!PlayerPrefs.HasKey("PlayerCrystal"))
        {
            FinanceManager.crystal = 0;
        }
        else
        {
            FinanceManager.crystal = PlayerPrefs.GetInt("PlayerCrystal");
        }
    }

    public static void SaveScroll()
    {
        PlayerPrefs.SetInt("PlayerEScroll", FinanceManager.enhanceScroll);
        PlayerPrefs.SetInt("PlayerIScroll", FinanceManager.ingredientScroll);
        PlayerPrefs.Save();
    }

    public static void LoadScroll()
    {
        if (!PlayerPrefs.HasKey("PlayerEScroll"))
        {
            FinanceManager.enhanceScroll = 0;
        }
        else
        {
            FinanceManager.enhanceScroll = PlayerPrefs.GetInt("PlayerEScroll");
        }

        if (!PlayerPrefs.HasKey("PlayerIScroll"))
        {
            FinanceManager.ingredientScroll = 0;
        }
        else
        {
            FinanceManager.ingredientScroll = PlayerPrefs.GetInt("PlayerIScroll");
        }
    }

    public static void SaveInventory()
    {
        for (int i = 0; i < InventoryManager.ingredientInventory.Count; i++)
        {
            PlayerPrefs.SetInt("InventoryNumber:" + i.ToString(), InventoryManager.ingredientInventory[InventoryManager.ingredientSOsStatic[i]]);
            //Debug.Log(PlayerPrefs.GetInt("InventoryNumber:" + i.ToString()));
        }
        PlayerPrefs.Save();
    }

    //Called in InventoryManager
    public static void LoadInventory()
    {
        if (s_bLoadedInven == true)
            return;

        s_bLoadedInven = true;

        Dictionary<IngredientsSO, int> result = new Dictionary<IngredientsSO, int>();
        for (int i = 0; i < InventoryManager.ingredientInventory.Count; i++)
        {
            result.Add(InventoryManager.ingredientSOsStatic[i],PlayerPrefs.GetInt("InventoryNumber:" + i.ToString()));
        }
        InventoryManager.ingredientInventory = result;
    }

    public static void SaveFoodList()
    {
        for (int i = 0; i < RecipeManager.completeFoodList.Count; i++)
        {
            PlayerPrefs.SetInt("FoodCompletion: " + i.ToString(), RecipeManager.completeFoodList[i].ID);
        }
        PlayerPrefs.Save();
    }

    public static void LoadFoodList()
    {
        if (s_bLoadedFoodList == true)
            return;

        s_bLoadedFoodList = true;

        List<MenuSO> result = new List<MenuSO>();
        for (int i = 0; i < RecipeManager.completeFoodList.Count; i++)
        {
            result.Add(RecipeManager.foodDictionary[PlayerPrefs.GetInt("FoodCompletion: " + i.ToString())]);
        }
        RecipeManager.completeFoodList = result;
    }

    public static void SaveFoodInventory()
    {
        for (int i = 0; i < RecipeManager.foodInventory.Count; i++)
        {
            PlayerPrefs.SetInt("FoodNumber:" + i.ToString(), RecipeManager.foodInventory[RecipeManager.MenuSOStatic[i]]);
        }
        PlayerPrefs.Save();
    }

    public static void LoadFoodInventory()
    {
        if (s_bLoadedFoodInven == true)
            return;

        s_bLoadedFoodInven = true;

        Dictionary<MenuSO, int> result = new Dictionary<MenuSO, int>();
        for (int i = 0; i < RecipeManager.foodInventory.Count; i++)
        {
            result.Add(RecipeManager.MenuSOStatic[i], PlayerPrefs.GetInt("FoodNumber:" + i.ToString()));
        }
        RecipeManager.foodInventory = result;
    }

    public static void SaveRecipeCreation()
    {
        for (int i = 0; i < RecipeCreator.haveCreatedOnce.Count; i++)
        {
            if (RecipeCreator.haveCreatedOnce[i])
            {
                PlayerPrefs.SetInt("MenuNumber:" + i.ToString(), 1);
                //Debug.Log(PlayerPrefs.GetInt("MenuNumber:" + i.ToString()));
            } 
            else 
            {
                PlayerPrefs.SetInt("MenuNumber:" + i.ToString(), 0);
                //Debug.Log(PlayerPrefs.GetInt("MenuNumber:" + i.ToString()));
            }
        }
        PlayerPrefs.Save();
    }

    //Called in RecipeManager
    public static void LoadRecipeCreation()
    {
        if (s_bLoadedRecipe == true)
            return;

        s_bLoadedRecipe = true;

        Dictionary<int, bool> result = new Dictionary<int, bool>();
        for (int i = 0; i < RecipeCreator.haveCreatedOnce.Count; i++)
        {
            if (PlayerPrefs.GetInt("MenuNumber:" + i.ToString()) == 0)
            {
                result.Add(i,false);
            } else if (PlayerPrefs.GetInt("MenuNumber:" + i.ToString()) == 1)
            {
                result.Add(i, true);
            }
        }
        RecipeCreator.haveCreatedOnce = result;
    }

    public static void SaveHPPrice(int hpPrice)
    {
        PlayerPrefs.SetInt("HPPrice", hpPrice);
        PlayerPrefs.Save();
    }

    public static void SaveDEFPrice(int defPrice)
    {
        PlayerPrefs.SetInt("DEFPrice", defPrice);
        PlayerPrefs.Save();
    }

    public static void SaveATKPrice(int atkPrice)
    {
        PlayerPrefs.SetInt("ATKPrice", atkPrice);
        PlayerPrefs.Save();
    }

    public static int LoadHPPrice()
    {
        return PlayerPrefs.GetInt("HPPrice");
    }

    public static int LoadDEFPrice()
    {
        return PlayerPrefs.GetInt("DEFPrice");
    }

    public static int LoadATKPrice()
    {
        return PlayerPrefs.GetInt("ATKPrice");
    }

    public static int LoadPricebyIdx(int index)
    {
        if (index == 0)
        {
            return LoadHPPrice();
        }
        else if (index == 1)
        {
            return LoadDEFPrice();
        }
        else if (index == 2)
        {
            return LoadATKPrice();
        }

        return 0;
    }
    public static void SaveSpecialAbillity(string abillity)
    {
        PlayerPrefs.SetInt(abillity, 1);
        PlayerPrefs.Save();
    }
    public static int LoadSpecialAbillity(string abillity)
    {
        return PlayerPrefs.GetInt(abillity);
    }
}
