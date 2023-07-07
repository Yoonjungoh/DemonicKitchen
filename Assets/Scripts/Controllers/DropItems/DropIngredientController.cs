using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropIngredientController : DropItemController
{
    private void Start()
    {
        _dropItemType = Define.DropItem.Ingredient;
        GetComponent<SpriteRenderer>().sprite = _ingredient._ingredientSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>()._ingredientDataList.Add(_ingredient);
            collision.gameObject.GetComponent<PlayerController>().TotalIngredient += 1;
            gameObject.SetActive(false);
        }
    }
}
