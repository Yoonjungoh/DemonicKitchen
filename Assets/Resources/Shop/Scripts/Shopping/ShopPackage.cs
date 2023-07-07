using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PackageMenu", menuName = "Scriptable Objects/New package", order = 4)]
public class ShopPackage : ScriptableObject
{
    public string packageTitle;
    public int packagePrice;
    public int packageEfficiency;

    public int packageGold;
    public int packageRuby;
    public int basicIngredientQty;
    public int seasoningQty;
    public int enhanceScrollQty;
    public int ingredientScrollQty;
    public int packageCrystal;

    public void rewardPackage()
    {
        FinanceManager.coins += this.packageGold;
        FinanceManager.ruby += this.packageRuby;
        TreasureChest.randomBasicIngredientDraw(this.basicIngredientQty);
        TreasureChest.randomSeasoningDraw(this.seasoningQty);
        FinanceManager.enhanceScroll += this.enhanceScrollQty;
        FinanceManager.ingredientScroll += this.ingredientScrollQty;
        FinanceManager.crystal += this.packageCrystal;
    }
}
