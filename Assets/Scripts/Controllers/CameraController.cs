using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    GameObject player;
    // ī�޶�� �÷��̾� ������ �Ÿ�
    [SerializeField]
    private float _height = 0f;
    // ī�޶�� �÷��̾� ������ �Ÿ� - ���� ����
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
            // ����
            case Define.SceneType.Main:
                break;
            // ����
            case Define.SceneType.Kitchen:
                break;
            // x�� ���� y���� �ְ��� �ݿ��Ͽ� ���� ���
            case Define.SceneType.Game:
                float playerHighestHeight = player.GetComponent<PlayerController>().HighestHeight;
                transform.position = new Vector3(0, playerHighestHeight + _height, _originalCameraPosition.z);
                break;
            // �÷��̾ ������ ����
            case Define.SceneType.Boss:
                transform.position = player.transform.position + new Vector3(0, _heightBossCamera, -10);
                //Camera.main.aspect = 18f / 9f;
                break;
        }
    }

}
