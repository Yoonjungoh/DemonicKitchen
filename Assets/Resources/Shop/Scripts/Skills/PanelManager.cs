using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;
    public Button leftButton;
    public Button rightButton;

    private int currentPanelIdx;

    private void Start()
    {
        currentPanelIdx = 0;
    }

    public void movePanelRight()
    {
        panels[currentPanelIdx].gameObject.SetActive(false);
        currentPanelIdx++;
        panels[currentPanelIdx].gameObject.SetActive(true);
        if (currentPanelIdx+1 == panels.Length)
        {
            rightButton.interactable = false;
            leftButton.interactable = true;
        }
    }

    public void movePanelLeft()
    {
        panels[currentPanelIdx].gameObject.SetActive(false);
        currentPanelIdx--;
        panels[currentPanelIdx].gameObject.SetActive(true);
        if (currentPanelIdx == 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = true;
        }
    }
}
