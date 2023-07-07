using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    GameObject player;
    // 카메라와 플레이어 사이의 거리
    [SerializeField]
    private float _height = 0f;
    // 카메라와 플레이어 사이의 거리 - 보스 전용
    [SerializeField]
    private float _heightBossCamera = 2f;
    
    Vector3 _originalCameraPosition;
    void Start()
    {
        Init();
    }
    void Update()
    {
        UpdateCameraPosition();
    }
    void Init()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        _originalCameraPosition = transform.position;
    }
    void UpdateCameraPosition()
    {
        switch (gameManager.sceneType)
        {
            // 고정
            case Define.SceneType.Main:
                break;
            // 고정
            case Define.SceneType.Kitchen:
                break;
            // x축 고정 y축의 최고값만 반영하여 위로 상승
            case Define.SceneType.Game:
                float playerHighestHeight = player.GetComponent<PlayerController>().HighestHeight;
                transform.position = new Vector3(0, playerHighestHeight + _height, _originalCameraPosition.z);
                break;
            // 플레이어를 무한정 따라감
            case Define.SceneType.Boss:
                transform.position = player.transform.position + new Vector3(0, _heightBossCamera, -10);
                //Camera.main.aspect = 18f / 9f;
                break;
        }
    }

}
