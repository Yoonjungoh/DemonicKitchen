using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGoldController : DropItemController
{
    private void Start()
    {
        _dropItemType = Define.DropItem.Gold;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TotalGold += _gold;
            gameObject.SetActive(false);
        }
    }
}
