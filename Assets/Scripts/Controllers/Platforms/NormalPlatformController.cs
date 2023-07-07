using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlatformController : PlatformController
{
    public GameManager gameManager;
    void Update()
    {
        _platformType = Define.PlatformType.Normal;

        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();

        if (gameManager.sceneType == Define.SceneType.Boss)
        {
            gameObject.GetComponent<OutCamera>()._isOut = false;
        }
    }

}
