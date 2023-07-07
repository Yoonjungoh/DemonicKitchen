using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : MonoBehaviour
{
    public GameObject masterMonster;
    public int _damage = 2000;
    Transform _spawnPoint;
    Transform player;
    void Start()
    {
        masterMonster = GameObject.Find("Monster_2002");
        _spawnPoint = GameObject.Find("SpearSpawnPoint").transform;
        player = GameObject.Find("Player").transform;
        transform.position = new Vector3(player.position.x, transform.position.y, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform") 
        {
            gameObject.SetActive(false);
            transform.position = _spawnPoint.position;
        }
        if (collision.tag == "Player")
        {
            Util.SummonDamageViewer(collision.gameObject, _damage);
            collision.GetComponent<PlayerController>().monsterHittedMe = masterMonster;
            transform.position = _spawnPoint.position;
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (player != null) 
            transform.position = new Vector3(player.position.x, transform.position.y, 0);
    }
}
