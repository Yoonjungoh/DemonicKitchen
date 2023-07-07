using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuIcon : MonoBehaviour
{
    public int menuID;
    public Image image;
    public Image checkMark;
    public TMP_Text titleTxt;
    private RecipeCreator menuIconRecipeCreator;

    public void setThumbnail(Sprite sprite)
    {
        this.image.sprite = sprite;
    }

    public void setCheckMark(bool hasCreatedOnce)
    {
        if (hasCreatedOnce)
        {
            this.checkMark.gameObject.SetActive(true);
        }
    }

    public void setTitle(string title)
    {
        this.titleTxt.text = title;
    }

    public void setID(int id)
    {
        this.menuID = id;
    }

    public void SetRecipeCreator(RecipeCreator Object)
    {
        this.menuIconRecipeCreator = Object;  
    }

    public void viewRecipeCreator()
    {
        menuIconRecipeCreator.loadRecipeCreationTab(menuID);
        menuIconRecipeCreator.gameObject.SetActive(true);
    }

}
