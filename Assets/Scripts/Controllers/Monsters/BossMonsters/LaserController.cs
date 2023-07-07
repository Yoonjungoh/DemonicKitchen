using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private int _laserDamage = 1000;
    public GameObject masterMonster;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            masterMonster = GameObject.Find("Monster_2001");
            Util.SummonDamageViewer(collision.gameObject,_laserDamage);
            collision.GetComponent<PlayerController>().monsterHittedMe = masterMonster;
        }
    }
}
