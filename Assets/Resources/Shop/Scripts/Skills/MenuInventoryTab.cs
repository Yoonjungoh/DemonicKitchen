using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuInventoryTab : MonoBehaviour
{
    public GameObject menuIconGO;
    public GameObject menuIconContainer;

    private List<GameObject> LoadedSlots = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(RecipeManager.foodInventory.Count);
        LoadInventoryGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadInventoryGrid()
    {
        int width = (RecipeManager.foodInventory.Count / 3) * 250;
        int height = 750;
        RectTransform rt = menuIconContainer.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);
        for (int i = 0; i < RecipeManager.foodInventory.Count; i++)
        {
            GameObject tempSlot = Instantiate(menuIconGO, menuIconContainer.transform);
            SkillMenuIcon menuSlot = tempSlot.GetComponent<SkillMenuIcon>();
            if (menuSlot != null)
            {
                menuSlot.setThumbnail(RecipeManager.completeFoodList[i].image);
                menuSlot.setRank(RecipeManager.completeFoodList[i].rank);
                LoadedSlots.Add(tempSlot);
            }
        }
    }

    public void shutFoodInventory()
    {
        for (int i = 0; i < LoadedSlots.Count; i++)
        {
            Destroy(LoadedSlots[i].gameObject);
        }
    }
}
