using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientData : MonoBehaviour
{
    // 각 레벨 마다 필요한 경험치 1레벨일 때 50경험치 모으면 2레벨 됨
    static public List<IngredientInfo> data = new List<IngredientInfo>()
    {
        new IngredientInfo(0, "Cooking Oil"),
        new IngredientInfo(1, "Egg"),
        new IngredientInfo(2, "Flour"),
        new IngredientInfo(3, "Gelatin"),
        new IngredientInfo(4, "Ice"),
        new IngredientInfo(5, "Pepper"),
        new IngredientInfo(6, "Rice"),
        new IngredientInfo(7, "Sesame Oil"),
        new IngredientInfo(8, "Starch"),
        new IngredientInfo(9, "Chili Flakes"),
        new IngredientInfo(10, "Herb"),
        new IngredientInfo(11, "salt"),
        new IngredientInfo(12, "Soy Sauce"),
        new IngredientInfo(13, "Sugar"),
        new IngredientInfo(14, "Vinegar"),
        new IngredientInfo(15, "Hind Leg"),
        new IngredientInfo(16, "Triangle Tail"),
        new IngredientInfo(17, "Larva"),
        new IngredientInfo(18, "Cocoon"),
        new IngredientInfo(19, "Bloodshot Eyeball"),
        new IngredientInfo(20, "Devil Wing"),
        new IngredientInfo(21, "Cow Bone"),
        new IngredientInfo(22, "Cow Rib "),
        new IngredientInfo(23, "Twinkle Wing"),
        new IngredientInfo(24, "Honey"),
        new IngredientInfo(25, "Honey Ant"),
        new IngredientInfo(26, "Ant Egg"),
        new IngredientInfo(27, "Chicken Leg"),
        new IngredientInfo(28, "Chicken Wing"),
        new IngredientInfo(29, "Pork Neck"),
        new IngredientInfo(30, "Pork Cutlet"),
        new IngredientInfo(31, "Clear Essence"),
        new IngredientInfo(32, "Cotton Candy"),
        new IngredientInfo(33, "Frozen Blood"),
        new IngredientInfo(34, "Blue Crystal"),
        new IngredientInfo(35, "Ink Powder"),
        new IngredientInfo(36, "Shining Feather"),
        new IngredientInfo(37, "Chaos Feather"),
        new IngredientInfo(38, "Milk"),
        new IngredientInfo(39, "Dark Powder"),
        new IngredientInfo(40, "Jokbal"),
        new IngredientInfo(41, "ForeLeg"),
        new IngredientInfo(42, "Cacao"),
        new IngredientInfo(43, "Magma Cream"),
    };
    static public void SetIngredientSprite()
    {
        for(int i = 0; i < data.Count; i++)
        {
            data[i]._ingredientSprite = Resources.Load<Sprite>($"Sprites/Ingredients/{i}");
        }
    }
}
public class IngredientInfo
{
    public int _ingredientID;
    public string _ingredientName;
    public Sprite _ingredientSprite;
    public IngredientInfo(int ingredientID, string ingredientName)
    {
        this._ingredientID = ingredientID;
        this._ingredientName = ingredientName;
    }   
}