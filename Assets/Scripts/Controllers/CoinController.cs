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
        // Instantiate �ǰ� ��� ���� ����� �� �ٷ� ��� false
        // �̷��� �ؾ� ���߿� �θ� ���� ���� �� �� null ���� �Ȼ���
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
