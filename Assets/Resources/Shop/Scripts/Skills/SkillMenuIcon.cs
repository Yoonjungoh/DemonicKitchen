using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillMenuIcon : MonoBehaviour
{
    public Image skillMenuThumbnail;
    public TMP_Text skillMenuRank;

    public void setThumbnail(Sprite sprite)
    {
        this.skillMenuThumbnail.sprite = sprite;
    }

    public void setRank(string rank)
    {
        this.skillMenuRank.text = rank;
    }

}
