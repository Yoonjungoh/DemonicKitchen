using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField]
    private int _gold;
    private void Start()
    {
        _gold = transform.parent.GetComponent<MonsterController>().Gold;
        // Instantiate 되고 골드 정보 저장된 후 바로 모습 false
        // 이렇게 해야 나중에 부모 관계 해제 될 떄 null 에러 안생김
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TotalGold += _gold;
            gameObject.SetActive(false);
        }
    }
}
