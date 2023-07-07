using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinanceManager : MonoBehaviour
{
    public static int coins;
    public static int ruby;
    public static int crystal;
    public static int stamina;
    public static int enhanceScroll;
    public static int ingredientScroll;

    public TMP_Text coinUI;
    public TMP_Text rubyTxt;
    public TMP_Text staminaTxt;
    // Start is called before the first frame update
    void Start()
    {
        PlayerDataSaver.LoadMoney();
        PlayerDataSaver.LoadRuby();
        syncAll();
    }

    // Update is called once per frame
    void Update()
    {
        syncAll();
    }

    public void syncAll()
    {
        syncCoinUI();
        syncRubyUI();
        syncStaminaUI();
    }

    public void syncCoinUI()
    {
        coinUI.text = coins.ToString();
    }

    public void syncRubyUI()
    { 
        rubyTxt.text = ruby.ToString();
    }

    public void syncStaminaUI()
    { 
        staminaTxt.text = stamina.ToString();  
    }
}
