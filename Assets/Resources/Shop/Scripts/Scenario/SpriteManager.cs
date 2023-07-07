using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] List<RectTransform> imagesRT;
    List<Image> images;

    private void Awake()
    {
        images = new List<Image>();
        for (int i = 0; i < imagesRT.Count; i++)
        {
            images.Add(imagesRT[i].GetComponent<Image>());
        }
    }

    public void Set(Sprite s, int id)
    {
        images[id].gameObject.SetActive(true);
        images[id].sprite = s;
    }

    public Image Get(int idx)
    {
        return images[idx];
    }

    public void Hide(int id)
    {
        images[id].gameObject.SetActive(false);
    }

    //이미지 셰이딩 (화자가 아닌 캐릭터를 어둡게 처리)
    public void ActiveShadow(int id)
    {
        for (int i = 0; i < images.Count; i++)
        {
            var shadow = images[i].transform.GetChild(0).GetComponent<Image>();
            if (i == id)
            {
                shadow.enabled = false;
            }
            else
            {
                shadow.enabled = true;
            }
        }
    }
}
