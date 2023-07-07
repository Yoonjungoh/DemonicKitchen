using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static IngredientsSO[] ingredientSOsStatic;
    public IngredientsSO[] ingredientsSOs;
    public GameObject ingredientInfoTab;
    public GameObject[] ingredientIconGO;
    public IngredientThumbnail[] ingredientThumbnails;
    public InventoryToggleTemplate inventoryToggleTemplate;
    private int currentIngredientID;
    public static Dictionary<IngredientsSO, int> ingredientInventory = null;
    public FinanceManager financeManager;
    public Button sellButton;

    //Use playerpref class to save away the first time acquisition, gold, etc
    //Use lists to store away the ids of the acquired ingredients and unacquired ones

    // Start is called before the first frame update
    void Start()
    {
        if (ingredientInventory == null)
        {
            ingredientInventory = new Dictionary<IngredientsSO, int>();

            for (int i = 0; i < ingredientIconGO.Length; i++)
            {
                ingredientInventory.Add(ingredientsSOs[i], 0);
            }
        }

        for (int i = 0; i < ingredientIconGO.Length; i++)
        {
            ingredientIconGO[i].SetActive(true);
            ingredientThumbnails[i].Thumbnail.sprite = ingredientsSOs[i].image;
            ingredientThumbnails[i].ThumbnailCount.text = "0";
        }

        for (int i = 0; i < ingredientIconGO.Length; i++)
        {
            UpdateUICount(i);
        }

        if ( ingredientSOsStatic == null )
        {
            ingredientSOsStatic = new IngredientsSO[ingredientsSOs.Length];
        }
 
        for (int i = 0; i < ingredientsSOs.Length; i++)
        {
            ingredientSOsStatic[i] = ingredientsSOs[i];
        }

        PlayerDataSaver.LoadInventory();
        if (InformationManager.addedIngredientCount != null)
        {
            for (int i = 0; i < InformationManager.addedIngredientCount.Count; i++)
            {
                IngredientsSO temp = ingredientsSOs[InformationManager.addedIngredientCount[i]];
                int count;
                ingredientInventory.TryGetValue(temp, out count);
                ingredientInventory[temp] = count + 1;
                Debug.Log(ingredientInventory[temp]);
            }

            InformationManager.addedIngredientCount.Clear();
        }

        //Inventory UI Update
        for (int i = 0; i < ingredientIconGO.Length; i++)
        {
            UpdateUICount(i);
        }

        PlayerDataSaver.LoadMoney();
        //Inventory Update
        financeManager.syncAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUICount(int thumbnailNo)
    {
        if (ingredientInventory[ingredientsSOs[thumbnailNo]] > 0)
        {
            ingredientThumbnails[thumbnailNo].shade.enabled = false;
        }
        else
        {
            ingredientThumbnails[thumbnailNo].shade.enabled = true;
        }
        ingredientThumbnails[thumbnailNo].ThumbnailCount.text = ingredientInventory[ingredientsSOs[thumbnailNo]].ToString();

    }

    public void toggleInformation() 
    {
        ingredientInfoTab.SetActive(true);
    }

    public void untoggleInformation()
    {
        ingredientInfoTab.SetActive(false);
    }

    public void LoadIngredients(int iconNo)
    {
        currentIngredientID = ingredientsSOs[iconNo].ID;
        inventoryToggleTemplate.ingredientID.text = "No." + ingredientsSOs[iconNo].ID.ToString();
        inventoryToggleTemplate.ingredientThumbNail.sprite = ingredientsSOs[iconNo].image;
        inventoryToggleTemplate.ingredientTitle.text = ingredientsSOs[iconNo].title.ToString();
        inventoryToggleTemplate.ingredientDescription.text = ingredientsSOs[iconNo].description.ToString();
        inventoryToggleTemplate.ingredientRank.text = ingredientsSOs[iconNo].rank.ToString();
        inventoryToggleTemplate.ingredientPrice.text = ingredientsSOs[iconNo].salesPrice.ToString() + " G";
        inventoryToggleTemplate.ingredientAmount.text = "Amount: " + ingredientInventory[ingredientsSOs[iconNo]].ToString();
        if (ingredientsSOs[iconNo].isMainIngredient)
        {
            inventoryToggleTemplate.ingredientType.text = "메인 재료";
        } 
        else if (ingredientsSOs[iconNo].isSeasoning) 
        {
            inventoryToggleTemplate.ingredientType.text = "조미료";
        }
        else if (ingredientsSOs[iconNo].isBasicIngredient)
        {
            inventoryToggleTemplate.ingredientType.text = "기본 재료";
        }
        else if (ingredientsSOs[iconNo].isSpecialIngredient)
        {
            inventoryToggleTemplate.ingredientType.text = "특수 재료";
        }
        sellControl();
        toggleInformation();
    }

    public void sellItem()
    {
        if (ingredientInventory[ingredientsSOs[currentIngredientID]] > 0)
        {
            ingredientInventory[ingredientsSOs[currentIngredientID]]--;
            inventoryToggleTemplate.ingredientAmount.text = "Amount: " + ingredientInventory[ingredientsSOs[currentIngredientID]].ToString();
            UpdateUICount(currentIngredientID);
            //Add Money
            FinanceManager.coins += ingredientsSOs[currentIngredientID].salesPrice;
            PlayerDataSaver.SaveMoney();
            financeManager.syncCoinUI();
        }
        else
        {
            sellButton.interactable = false;
        }
    }

    public void sellAllItems()
    {
        if (ingredientInventory[ingredientsSOs[currentIngredientID]] > 0)
        {
            int currentMaxCount = ingredientInventory[ingredientsSOs[currentIngredientID]];
            ingredientInventory[ingredientsSOs[currentIngredientID]] -= ingredientInventory[ingredientsSOs[currentIngredientID]];
            inventoryToggleTemplate.ingredientAmount.text = "Amount: " + ingredientInventory[ingredientsSOs[currentIngredientID]].ToString();
            UpdateUICount(currentIngredientID);
            FinanceManager.coins += ingredientsSOs[currentIngredientID].salesPrice * currentMaxCount;
            PlayerDataSaver.SaveMoney();
            financeManager.syncCoinUI();
            sellButton.interactable = false;
        }
    }

    public void sellControl()
    {
        if (ingredientInventory[ingredientsSOs[currentIngredientID]] > 0)
        {
            sellButton.interactable = true;
        }
        else 
        {
            sellButton.interactable = false;
        }
    }
}
