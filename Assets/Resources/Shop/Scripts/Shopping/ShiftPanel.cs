using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftPanel : MonoBehaviour
{
    public ScrollRect scrollRect;

    public void ScrolltoSpot(float vPos)
    {
        scrollRect.content.position = new Vector2(scrollRect.content.position.x,vPos);
    }
}
