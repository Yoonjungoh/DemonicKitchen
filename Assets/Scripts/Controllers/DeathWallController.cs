using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWallController : MonoBehaviour
{
    GameManager gameManager;
    GameObject player;
    // 벽과 플레이어 사이의 거리
    [SerializeField]
    private float _height = 0f;
    Vector3 _originalWallPosition;
    void Start()
    {
        Init();
    }
    void Update()
    {
        //UpdateDeathWallPosition();
    }
    void Init()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
        player = GameObject.Find("Player");
    }
    void UpdateDeathWallPosition()
    {
        float playerHighestHeight = player.GetComponent<PlayerController>().HighestHeight;
        transform.position = new Vector3(0, playerHighestHeight + _height, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Die cause DeathWall");
            //gameObject.GetComponent<PlayerController>().OnDamaged(int.MaxValue);
        }
    }
}
