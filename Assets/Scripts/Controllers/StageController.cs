using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject[] Stages; 
    void Awake()
    {
        int randIndex = Random.Range(0, Stages.Length);
        Stages[randIndex].SetActive(true);
    }
    public void click()
    {
        Debug.Log("Asd");
    }
}
