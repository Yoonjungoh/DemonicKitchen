using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeCreator : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public RecipeManager recipeManager;
    public FinanceManager financeManager;
    public CheatController cheatController;
    public GameObject slotContainer;
    public GameObject slotIconPrefab;
    public Button createButton;

    public Button BRecipeBook;
    public Button ARecipeBook;
    public Button SRecipeBook;
    public Button SSRecipeBook;
    public Button SSSRecipeBook;

    public GameObject successPanel;
    public GameObject failurePanel;

    private int currentMenuID;

    public TMP_Text menuName;
    public TMP_Text menuRank;
    public TMP_Text menuDescription;
    public TMP_Text menuCost;

    private List<GameObject> loadedSlots = new List<GameObject>();
    public static Dictionary<int, bool> haveCreatedOnce = new Dictionary<int, bool>();

    // Start is called before the first frame update
    void Start()
    {
        PlayerDataSaver.LoadScroll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadRecipeCreationTab(int recipeID)
    {
       currentMenuID = recipeID;
       controlCreation();
       checkOpenNewRecipe(recipeManager.GetMenu(currentMenuID).rank);
       MenuSO menuRef = recipeManager.GetMenu(recipeID);
       menuName.text = menuRef.title;
       menuRank.text = menuRef.rank;
       menuDescription.text = menuRef.description;
       menuCost.text = "필요 골드: " + menuRef.cost.ToString() + "G";
       int newWidth = menuRef.ingridientNeeded.Count * 300;
       RectTransform rt = slotContainer.GetComponent<RectTransform>();
       rt.sizeDelta = new Vector2(newWidth, rt.rect.height);
       for (int i = 0; i < menuRef.ingridientNeeded.Count; i++)
       {
            GameObject tempSlotIcon = Instantiate(slotIconPrefab, slotContainer.transform);
            SlotIcon slotIcon = tempSlotIcon.GetComponent<SlotIcon>();
            if (slotIcon != null)
            {
                slotIcon.setInventoryManager(inventoryManager);
                slotIcon.setSlotIngredientID(menuRef.ingridientNeeded[i].ID);
                slotIcon.setSlotThumbnail(menuRef.ingridientNeeded[i].image);
                loadedSlots.Add(tempSlotIcon);
            }
        }
    }

    public void createMenu()
    {
        MenuSO cMenuRef = recipeManager.GetMenu(currentMenuID);
        float prob = 0;
        if (cheatController.isUsingRecipeCheat)
        {
            prob = 100;
        }
        else 
        {
            prob = cMenuRef.successRate;
        }
        
        if (Dods_ChanceMaker.GetThisChanceResult_Percentage(prob))
        {
            FinanceManager.coins -= cMenuRef.cost;
            financeManager.syncCoinUI();

            if (Scroll.spendingNone)
            {
                haveCreatedOnce[currentMenuID] = true;
                controlCreation();
                checkOpenNewRecipe(recipeManager.GetMenu(currentMenuID).rank);
                Scroll.spendingNone = false;
            }
            else
            {
                for (int i = 0; i < cMenuRef.ingridientNeeded.Count; i++)
                {
                    if (InventoryManager.ingredientInventory[cMenuRef.ingridientNeeded[i]] > 0)
                    {
                        InventoryManager.ingredientInventory[cMenuRef.ingridientNeeded[i]]--;
                        inventoryManager.UpdateUICount(cMenuRef.ingridientNeeded[i].ID);
                        haveCreatedOnce[currentMenuID] = true;
                        controlCreation();
                        checkOpenNewRecipe(recipeManager.GetMenu(currentMenuID).rank);
                    }
                }
            }

            int count;
            RecipeManager.foodInventory.TryGetValue(cMenuRef,out count);
            RecipeManager.foodInventory[cMenuRef] = count + 1;
            RecipeManager.completeFoodList.Add(cMenuRef);

            successPanel.SetActive(true);
            PlayerDataSaver.SaveAll();
        }
        else
        {
            FinanceManager.coins -= cMenuRef.cost;
            financeManager.syncCoinUI();
            if (Scroll.spendingNone)
            {
                controlCreation();
                checkOpenNewRecipe(recipeManager.GetMenu(currentMenuID).rank);
                Scroll.spendingNone = false;
            }
            else
            {
                for (int i = 0; i < cMenuRef.ingridientNeeded.Count; i++)
                {
                    if (InventoryManager.ingredientInventory[cMenuRef.ingridientNeeded[i]] > 0)
                    {
                        InventoryManager.ingredientInventory[cMenuRef.ingridientNeeded[i]]--;
                        inventoryManager.UpdateUICount(cMenuRef.ingridientNeeded[i].ID);
                        controlCreation();
                        checkOpenNewRecipe(recipeManager.GetMenu(currentMenuID).rank);
                    }
                }
            }
            failurePanel.SetActive(true);
            PlayerDataSaver.SaveAll();
        }

    }

    public void controlCreation()
    {
        bool hasEverythingInStock = true;
        bool hasEnoughGold = true;
        MenuSO dMenuRef = recipeManager.GetMenu(currentMenuID);
        for (int i = 0; i < dMenuRef.ingridientNeeded.Count; i++)
        {
            if (InventoryManager.ingredientInventory[dMenuRef.ingridientNeeded[i]] <= 0)
            {
                hasEverythingInStock = false;
            }
        }

        if (dMenuRef.cost > FinanceManager.coins)
        {
            hasEnoughGold = false;
        }
        Debug.Log(hasEverythingInStock);
        // && !haveCreatedOnce[currentMenuID]
        if (hasEverythingInStock && hasEnoughGold)
        {
            createButton.interactable = true;
        }
        else
        {
            createButton.interactable = false;
        }
    }

    public void shutRecipeCreationTab()
    {
        for (int i = 0; i < loadedSlots.Count; i++)
        {
            Destroy(loadedSlots[i].gameObject);
        }
    }

    //0~4 = C
    //5~8 = B
    //9~12 = A
    //13~17 = S
    //18~22 = SS
    //23 = SSS

    public Button getRecipeButton(string rank)
    {
        if (rank == "B")
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

    public void checkOpenNewRecipe(string rank)
    {
        bool isReady = true;
        for (int i = 0; i < recipeManager.GetRecipeBook(rank).Count; i++)
        {
            if (!haveCreatedOnce[recipeManager.GetRecipeBook(rank)[i].ID])
            {
                isReady = false;
                break;
            }
        }

        string newRank = "";
        if (rank == "C")
        {
            newRank = "B";
        } 
        else if (rank == "B")
        {
            newRank = "A";
        }
        else if (rank == "A")
        {
            newRank = "S";
        }
        else if (rank == "S")
        {
            newRank = "SS";
        }
        else if (rank == "SS")
        {
            newRank = "SSS";
        }
        else if (rank == "SSS")
        {
            newRank = "SSS";
        }

        if (isReady)
        {
            getRecipeButton(newRank).interactable = true;
        }
    }

    public void restoreRecipeProgress()
    {
        checkOpenNewRecipe("C");
        checkOpenNewRecipe("B");
        checkOpenNewRecipe("A");
        checkOpenNewRecipe("S");
        checkOpenNewRecipe("SS");
        checkOpenNewRecipe("SSS");
    }
}
