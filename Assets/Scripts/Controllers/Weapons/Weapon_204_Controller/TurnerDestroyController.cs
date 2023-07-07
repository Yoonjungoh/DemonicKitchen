using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnerDestroyController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Turner(Clone)") 
        {
            collision.gameObject.SetActive(false);
        }
    }
}
