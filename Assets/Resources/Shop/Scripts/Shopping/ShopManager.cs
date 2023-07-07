using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public FinanceManager financeManager;
    public SoundManager soundManager;
    public ShopItemSO[] shopItemsSO;
    public GameObject[] shopPanelsGO;
    public Stat statManager;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;
    //Make sure to dequeue everything after a single play as all of the buffs wear off 
    private static Dictionary<int, ShopItemSO> myPurchaseQueue = new Dictionary<int, ShopItemSO>();
    public TMP_Text[] myPurchaseQueueName;
    private int purchaseCounter;
    public TMP_Text[] shopItemPriceTxt;


    // Start is called before the first frame update
    void Start()
    {
        purchaseCounter = 0;
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
        financeManager.syncCoinUI();
        LoadPanels();
        CheckPurchaseable();
        UpdatePurchaseQueueUI();
        statManager = GameObject.Find("@StatManager").GetComponent<Stat>();
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
    }

    public void SaveShopStat()
    {
        // statManager의 스탯
        PlayerPrefs.SetInt("PlayerMaxHP", statManager._maxHp);
        PlayerPrefs.SetFloat("PlayerHP", statManager._hp);
        PlayerPrefs.SetInt("PlayerAttack", statManager._attack);
        PlayerPrefs.SetInt("PlayerDefense", statManager._defense);
        PlayerPrefs.SetFloat("PlayerMaxSpeed", statManager._maxSpeed);
        PlayerPrefs.Save();
    }

    public void AddCoins()
    {
        FinanceManager.coins += 500;
        financeManager.syncCoinUI();
        CheckPurchaseable();
    }

    public void CheckPurchaseable()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if (FinanceManager.coins >= shopItemsSO[i].baseCost)
            {
                myPurchaseBtns[i].interactable = true;
            }
            else 
            {
                myPurchaseBtns[i].interactable = false;
            }

            if (purchaseCounter >= 3)
            {
                for (int j = 0; j < shopItemsSO.Length; j++)
                {
                    if (shopItemsSO[j].isBuffItem) 
                    {
                        myPurchaseBtns[j].interactable = false;
                    }
                }
            }
        }
    }

    public void PurchaseItem(int btnNo)
    {
        if (FinanceManager.coins >= shopItemsSO[btnNo].baseCost)
        {
            FinanceManager.coins = FinanceManager.coins - shopItemsSO[btnNo].baseCost;
            financeManager.syncCoinUI();
            if (shopItemsSO[btnNo].isBuffItem && myPurchaseQueue.Count < 3)
            {
                myPurchaseQueue.Add(purchaseCounter, shopItemsSO[btnNo]);
                myPurchaseQueueName[purchaseCounter].text = shopItemsSO[btnNo].title.ToString();
                purchaseCounter++;
            }
            else if (!shopItemsSO[btnNo].isBuffItem)
            {
                increaseStats(btnNo);
            }
            CheckPurchaseable();
            PlayerDataSaver.SaveAll();
        }

    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemsSO[i].title;
            shopPanels[i].descriptionTxt.text = shopItemsSO[i].description;
            if (PlayerDataSaver.LoadPricebyIdx(i) > shopItemsSO[i].baseCost)
            {
                shopItemsSO[i].baseCost = PlayerDataSaver.LoadPricebyIdx(i);
                shopPanels[i].costTxt.text = PlayerDataSaver.LoadPricebyIdx(i).ToString();
                Debug.Log("Loading Playercost: " + PlayerDataSaver.LoadPricebyIdx(i));
            }
            else
            {
                shopPanels[i].costTxt.text = shopItemsSO[i].baseCost.ToString();
                Debug.Log("Loading Basecost" + shopItemsSO[i].baseCost);
            }
            shopPanels[i].thumbnail.sprite = shopItemsSO[i].thumbnail;
        }
    }

    public void UpdatePurchaseQueueUI()
    {
        for (int i = 0; i < myPurchaseQueue.Count; i++)
        {
            myPurchaseQueueName[i].text = myPurchaseQueue[i].title;
        }
    }

    //Reset the purchaseQueue after a single round of play
    public void ResetPurchaseQueue()
    { 
        myPurchaseQueue.Clear();
        UpdatePurchaseQueueUI();
    }

    //스탯 강화
    public void increaseStats(int itemIdx)
    {
        if (itemIdx == 0)
        {
            //HP
            soundManager.effect.PlayOneShot(soundManager.clickEffect);
            statManager._maxHp += shopItemsSO[itemIdx].enhanceAmount;
            statManager._hp += shopItemsSO[itemIdx].enhanceAmount;
            shopItemsSO[itemIdx].baseCost += 350;
            shopItemPriceTxt[itemIdx].text = shopItemsSO[itemIdx].baseCost.ToString();
            PlayerDataSaver.SaveHPPrice(shopItemsSO[itemIdx].baseCost);
            SaveShopStat();
        }
        else if (itemIdx == 1)
        {
            //DF
            soundManager.effect.PlayOneShot(soundManager.clickEffect);
            statManager._defense += shopItemsSO[itemIdx].enhanceAmount;
            shopItemsSO[itemIdx].baseCost += 500;
            shopItemPriceTxt[itemIdx].text = shopItemsSO[itemIdx].baseCost.ToString();
            PlayerDataSaver.SaveDEFPrice(shopItemsSO[itemIdx].baseCost);
            SaveShopStat();
        }
        else if (itemIdx == 2)
        {
            //ATK
            soundManager.effect.PlayOneShot(soundManager.clickEffect);
            statManager._attack += shopItemsSO[itemIdx].enhanceAmount;
            shopItemsSO[itemIdx].baseCost += 500;
            shopItemPriceTxt[itemIdx].text = shopItemsSO[itemIdx].baseCost.ToString();
            PlayerDataSaver.SaveATKPrice(shopItemsSO[itemIdx].baseCost);
            SaveShopStat();
        }
    }

    public void DropRateUp()
    { 
        //드랍률 변수+=드랍률 증가수치;
    }

    public void DecreaseCoolDown()
    { 
        //쿨타임 변수-=쿨타임 감소수치;
    }

    public void ShieldingOn()
    {
        //기존 콜라이더 해제 후
        //데미지 받지않는 임시 콜라이더 부여
        int shieldCounter = 3;
        while (shieldCounter > 0)
        {
            /**
             //if (임시 콜라이더로 피격 당했을 경우)
            {
                shieldCounter--;
            }
             */
        }
        //임시 콜라이더 비활성화
        //기존 콜라이더 활성화
    }
}
