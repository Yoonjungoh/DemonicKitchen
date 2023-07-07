using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyInformationManager : MonoBehaviour
{
    public Image HPBar;
    public Image DEFBar;
    public Image ATKBar;

    public TMP_Text HPTxt;
    public TMP_Text DEFTxt;
    public TMP_Text ATKTxt;

    public TMP_Text HighestScore;

    private int MaxHP = 2450;
    private int MaxDEF = 245;
    private int MaxATK = 245;

    // Start is called before the first frame update
    void Start()
    {
        syncTxt();
        syncSliders();
    }

    // Update is called once per frame
    void Update()
    {
        syncTxt();
        syncSliders();
    }

    public void syncTxt()
    {
        HPTxt.text = PlayerPrefs.GetInt("PlayerMaxHP").ToString();
        DEFTxt.text = PlayerPrefs.GetInt("PlayerDefense").ToString();
        ATKTxt.text = PlayerPrefs.GetInt("PlayerAttack").ToString();

        HighestScore.text = PlayerDataSaver.LoadScore().ToString();
    }

    public void syncSliders()
    {
        HPBar.fillAmount = PlayerPrefs.GetInt("PlayerMaxHP") / (float)MaxHP;
        DEFBar.fillAmount = PlayerPrefs.GetInt("PlayerDefense") / (float)MaxDEF;
        ATKBar.fillAmount = PlayerPrefs.GetInt("PlayerAttack") / (float)MaxATK;
    }
}
