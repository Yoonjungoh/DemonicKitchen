using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotIcon : MonoBehaviour
{
    public int slotIngredientID;
    public Image image;
    private InventoryManager inventoryManager;

    public void setSlotIngredientID(int id)
    { 
        this.slotIngredientID = id;
    }

    public void setSlotThumbnail(Sprite sprite)
    {
        this.image.sprite = sprite;
    }

    public void setInventoryManager(InventoryManager Object)
    {
        this.inventoryManager = Object;
    }

    public void viewIngredientInfoTab()
    {
        inventoryManager.LoadIngredients(slotIngredientID);
    }
}
