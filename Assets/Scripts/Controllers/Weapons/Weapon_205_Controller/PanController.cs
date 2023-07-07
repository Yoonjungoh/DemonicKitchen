using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanController : Weapon_205_Controller
{
    float _outTimer = 0f;
    SoundManager soundManager;
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        if (_outTimer >= _animationTime)
        {
            _outTimer = 0;
            gameObject.SetActive(false);
        }
        else
        {
            _outTimer += Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
            Util.SummonDamageViewer(collision.gameObject, (_weaponAttack * _level) + _player.GetComponent<Stat>().Attack);
    }
    public void LevelUp()
    {
        _level++;
    }
}
