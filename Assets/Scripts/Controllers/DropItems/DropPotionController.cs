using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPotionController : DropItemController
{
    private void Start()
    {
        _dropItemType = Define.DropItem.Potion;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Heal();
            gameObject.SetActive(false);
        }
    }
}
