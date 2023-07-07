using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tab;
    public string tutorialName;
    // Start is called before the first frame update
    void Start()
    {
        if (tutorialName == "Kitchen")
        {
            if (!PlayerDataSaver.LoadSeenKitchen())
            {
                tab.SetActive(true);
                PlayerDataSaver.s_hasSeenKitchen = true;
                PlayerDataSaver.SaveSeenKitchen();
            }
            else
            {
                tab.SetActive(false);
            }
        }
        else if (tutorialName == "Skills")
        {
            if (!PlayerDataSaver.LoadSeenSkills())
            {
                tab.SetActive(true);
                PlayerDataSaver.s_hasSeenSkills = true;
                PlayerDataSaver.SaveSeenSkills();
            }
            else
            {
                tab.SetActive(false);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
