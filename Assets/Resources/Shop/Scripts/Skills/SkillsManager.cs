using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public SpecialAbilities[] SpecialAbilities;
    public GameObject[] AbilitiesPanel;
    public SkillTemplate[] SkillTemplates;
    public GameObject[] SkillsButtons;

    public SkillUnlockPanel skillUnlockPanel;
    public GameObject skillUnlockPanelGO;

    public SkillEnhancePanel skillEnhancePanel;
    public GameObject skillEnhancePanelGO;

    public SkillEnhanceTab skillEnhanceTab;
    public GameObject skillEnhanceTabGO;

    private int currentAbilityNo;
    private MenuSO currentMenuSO;
    private SpecialAbilities currentAbility;

    private bool crystalLoaded;
    public GameObject crystalImage;
    public Image unlockFoodIcon;
    public Image enhanceFoodIcon;
    private bool foodLoaded;

    FinanceManager financeManager;

    // Start is called before the first frame update
    void Start()
    {
        crystalLoaded = false;
        foodLoaded = false;
        PlayerDataSaver.LoadCrystal();
        LoadSkills();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSkills()
    {
        for (int i = 0; i < SpecialAbilities.Length; i++)
        {
            SkillTemplates[i].heading.text = SpecialAbilities[i].title;
            SkillTemplates[i].thumbnail.sprite = SpecialAbilities[i].image;
        }
    }

    public void LoadTabs(int iconNo)
    {
        currentAbilityNo = iconNo;
        currentAbility = SpecialAbilities[currentAbilityNo];
        if (!SpecialAbilities[iconNo].isUnlocked)
        {
            skillUnlockPanel.unlockHeader.text = SpecialAbilities[iconNo].title + " 해금";
            skillUnlockPanel.skillThumbnail.sprite = SpecialAbilities[iconNo].image;
            skillUnlockPanel.skillHeader.text = SpecialAbilities[iconNo].title;
            skillUnlockPanel.percentage.text = SpecialAbilities[iconNo].prob.ToString() + "%";
            skillUnlockPanel.cost.text = SpecialAbilities[iconNo].unlockCost.ToString();
            skillUnlockPanelGO.SetActive(true);
        } else
        {
            skillEnhancePanel.heading.text = SpecialAbilities[iconNo].title;
            for (int i = 0; i < 4; i++) 
            {
                skillEnhancePanel.thumbnails[i].sprite = SpecialAbilities[iconNo].image;
            }
            for (int i = 0; i < SpecialAbilities[iconNo].skillLevel; i++)
            {
                skillEnhancePanel.enhanceButtons[i].interactable = true;
            }
            skillEnhancePanelGO.SetActive(true);
        }
    }

    public void LoadEnhanceTab()
    {
        skillEnhanceTab.tabHeader.text = "능력 강화";
        skillEnhanceTab.thumbnail.sprite = SpecialAbilities[currentAbilityNo].image;
        skillEnhanceTab.header.text = SpecialAbilities[currentAbilityNo].title;
        skillEnhanceTab.percentage.text = SpecialAbilities[currentAbilityNo].prob.ToString() + "%";
        skillEnhanceTab.cost.text = SpecialAbilities[currentAbilityNo].enhanceCost.ToString() + "%";
        skillEnhanceTabGO.SetActive(true);
    }

    public void UnlockNewAbility()
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

    public void EnhanceAbility()
    {
        if (currentAbility.skillLevel < 5 && foodLoaded && crystalLoaded && FinanceManager.coins > currentAbility.enhanceCost)
        {
            if (Dods_ChanceMaker.GetThisChanceResult_Percentage(currentAbility.enhanceProb))
            {
                int count;
                RecipeManager.foodInventory.TryGetValue(currentMenuSO,out count);
                RecipeManager.foodInventory[currentMenuSO] = count - 1;
                FinanceManager.coins -= currentAbility.enhanceCost;
                FinanceManager.crystal -= currentAbility.enhanceStoneCost;

                currentAbility.enhanceProb -= 3;
                currentAbility.skillLevel++;
                currentAbility.enhanceCost += 2000;
                currentAbility.enhanceStoneCost += 2;

                PlayerDataSaver.SaveAll();
            }
        }
    }

    public void SelectFood()
    {
 
                currentMenuSO = RecipeManager.completeFoodList[0];
                if (!currentAbility.isUnlocked)
                {
                    unlockFoodIcon.sprite = currentMenuSO.image;
                }
                else
                {
                    enhanceFoodIcon.sprite = currentMenuSO.image;
                }

        foodLoaded = true;
    }

    public void SelectCrystal()
    {
        if (FinanceManager.crystal > currentAbility.enhanceStoneCost)
        {
            crystalImage.SetActive(true);
            crystalLoaded = true;
        }
    }
}
