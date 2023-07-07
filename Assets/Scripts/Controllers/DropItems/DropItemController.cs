using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemController : MonoBehaviour
{
    public int _gold;
    public IngredientInfo _ingredient;
    [SerializeField]
    protected Define.DropItem _dropItemType;
}
