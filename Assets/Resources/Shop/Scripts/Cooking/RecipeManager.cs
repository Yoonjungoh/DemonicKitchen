using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField]
    private MenuSO[] menuSOs;
    public static MenuSO[] MenuSOStatic;

    private List<MenuSO> CRecipeBook = new List<MenuSO>();
    private List<MenuSO> BRecipeBook = new List<MenuSO>();
    private List<MenuSO> ARecipeBook = new List<MenuSO>();
    private List<MenuSO> SRecipeBook = new List<MenuSO>();
    private List<MenuSO> SSRecipeBook = new List<MenuSO>();
    private List<MenuSO> SSSRecipeBook = new List<MenuSO>();

    private List<GameObject> loadedRecipe = new List<GameObject>();
    public GameObject recipeTemplate;
    public GameObject menuIconPrefab;
    public GameObject menuPanel;
    public RecipeCreator recipeCreator;

    public static Dictionary<MenuSO, int> foodInventory = null;
    public static List<MenuSO> completeFoodList = null;
    public static Dictionary<int, MenuSO> foodDictionary = null;

    // Start is called before the first frame update
    void Start()
    {
        if (completeFoodList == null)
        { 
            completeFoodList = new List<MenuSO>();
        }

        if (foodInventory == null)
        {
            foodInventory = new Dictionary<MenuSO, int>();

            for (int i = 0; i < menuSOs.Length; i++)
            {
                foodInventory.Add(menuSOs[i], 0);
            }
        }

        if (foodDictionary == null)
        {
            foodDictionary = new Dictionary<int, MenuSO>();
            for (int i = 0; i < menuSOs.Length; i++)
            {
                foodDictionary.Add(menuSOs[i].ID, menuSOs[i]);
            }
        }

        for (int i = 0; i < menuSOs.Length; i++) 
        {
            if (menuSOs[i].rank == "C")
            {
                CRecipeBook.Add(menuSOs[i]);
                GameManager.SpecialAbilityList.Add(Define.SpecialAbility.HPPotion);
                // Save Data
                PlayerDataSaver.SaveSpecialAbillity("HPPotion");
            } 
            else if (menuSOs[i].rank == "B")
            {
                BRecipeBook.Add(menuSOs[i]);
                GameManager.SpecialAbilityList.Add(Define.SpecialAbility.EarthQuake);
                // Save Data
                PlayerDataSaver.SaveSpecialAbillity("EarthQuake");
            }
            else if (menuSOs[i].rank == "A")
            {
                ARecipeBook.Add(menuSOs[i]);
                GameManager.SpecialAbilityList.Add(Define.SpecialAbility.Reflection);
                // Save Data
                PlayerDataSaver.SaveSpecialAbillity("Reflection");
            }
            else if (menuSOs[i].rank == "S")
            {
                SRecipeBook.Add(menuSOs[i]);
                GameManager.SpecialAbilityList.Add(Define.SpecialAbility.Resurrection);
                // Save Data
                PlayerDataSaver.SaveSpecialAbillity("Resurrection");
            }
            else if (menuSOs[i].rank == "SS")
            {
                SSRecipeBook.Add(menuSOs[i]);
            }
            else if (menuSOs[i].rank == "SSS")
            {
                SSSRecipeBook.Add(menuSOs[i]);
            }
        }

        if (RecipeCreator.haveCreatedOnce.Count == 0)
        {
            for (int i = 0; i < menuSOs.Length; i++)
            {
                RecipeCreator.haveCreatedOnce.Add(i, false);
            }
        }

        if (MenuSOStatic == null)
        {
            MenuSOStatic = new MenuSO[menuSOs.Length];
        }

        for (int i = 0; i < menuSOs.Length; i++)
        {
            MenuSOStatic[i] = menuSOs[i];
        }

        recipeCreator.restoreRecipeProgress();
        PlayerDataSaver.LoadFoodInventory();
        PlayerDataSaver.LoadRecipeCreation();
        PlayerDataSaver.LoadFoodList();
        //RecipeBook UI Update
        recipeCreator.restoreRecipeProgress();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<MenuSO> GetRecipeBook (string rank)
    {
        if (rank == "C")
        {
            return CRecipeBook;
        }
        else if (rank == "B")
        {
            return BRecipeBook;
        }
        else if (rank == "A")
        {
            return ARecipeBook;
        }
        else if (rank == "S")
        {
            return SRecipeBook;
        }
        else if (rank == "SS")
        {
            return SSRecipeBook;
        }
        else if (rank == "SSS")
        {
            return SSSRecipeBook;
        }

        return null;
    }

    public MenuSO GetMenu(int menuID)
    {
        return menuSOs[menuID];
    }

    public int GetMenuCount()
    {
        return menuSOs.Length;
    }

    public void writeRecipe(List<MenuSO> RecipeBook)
    {
        int newHeight = RecipeBook.Count * 75;
        RectTransform rt = menuPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.rect.width, newHeight);
        for (int i = 0; i < RecipeBook.Count; i++)
        {
            GameObject tempMenuIcon = Instantiate(menuIconPrefab, menuPanel.transform);
            MenuIcon menuIcon = tempMenuIcon.GetComponent<MenuIcon>();
            if (menuIcon != null)
            {
                menuIcon.SetRecipeCreator(recipeCreator);
                menuIcon.setThumbnail(RecipeBook[i].image);
                menuIcon.setTitle(RecipeBook[i].title);
                menuIcon.setID(RecipeBook[i].ID);
                menuIcon.setCheckMark(RecipeCreator.haveCreatedOnce[RecipeBook[i].ID]);
                loadedRecipe.Add(tempMenuIcon);
            }
        }
    }

    public void loadRecipe(string rank)
    {
        if (rank == "C")
        {
            writeRecipe(CRecipeBook);
        }
        else if (rank == "B")
        {
            writeRecipe(BRecipeBook);
        }
        else if (rank == "A")
        {
            writeRecipe(ARecipeBook);
        }
        else if (rank == "S")
        {
            writeRecipe(SRecipeBook);
        }
        else if (rank == "SS")
        {
            writeRecipe(SSRecipeBook);
        }
        else if (rank == "SSS")
        {
            writeRecipe(SSSRecipeBook);
        }
        recipeTemplate.SetActive(true);
    }

    public void shutRecipe()
    {
        for (int i = 0; i < loadedRecipe.Count; i++)
        {
            Destroy(loadedRecipe[i].gameObject);
        }
        recipeTemplate.SetActive(false);
    }
}
