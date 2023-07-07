using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackgroundController : MonoBehaviour
{
    public Transform _changeMorningPoint;
    public Transform _changeEveningPoint;
    public Transform _changeNightPoint;

    public GameObject player;
    public BackGroundController backgroundController;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        backgroundController = GameObject.Find("BackGroundCanvas").GetComponent<BackGroundController>();
    }

    void Update()
    {
        CheckChangeBackground();
    }
    void CheckChangeBackground()
    {
        if (_changeMorningPoint.position.y <= player.transform.position.y && player.transform.position.y <= _changeEveningPoint.position.y)
        {
            backgroundController.ChangeMorningImage();
            Debug.Log("morning");
        }
        else if (_changeEveningPoint.position.y < player.transform.position.y && player.transform.position.y <= _changeNightPoint.position.y)
        {
            backgroundController.ChangeEveningImage();
            Debug.Log("evening");
        }
        else if (_changeNightPoint.position.y < player.transform.position.y)
        {
            backgroundController.ChangeNightImage();
            Debug.Log("night");
        }
    }
}
