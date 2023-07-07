using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public List<GameObject> potions = new List<GameObject>();
    void Awake()
    {
        if (GameObject.Find("@Managers") == null)
        {
            GameObject managers = new GameObject { name = "@Managers" };
            managers.AddComponent<GameManager>();
            DontDestroyOnLoad(managers);
        }
        if (GameObject.Find("@EventSystem") == null)
        {
            GameObject eventSystem = Util.Instantiate("@EventSystem");
            DontDestroyOnLoad(eventSystem);
        }
    }
    public void ActivePotions()
    {
        if (potions.Count != 0)
        {
            foreach (GameObject potion in potions)
            {
                potion.SetActive(true);
            }
        }
    }
}
