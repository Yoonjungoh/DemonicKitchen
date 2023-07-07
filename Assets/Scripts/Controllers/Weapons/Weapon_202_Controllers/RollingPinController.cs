using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingPinController : Weapon_202_Controller
{
    SoundManager soundManager;
    void Start()
    {
        base.Init();
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
    }
    // 데미지 설정
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_attackDelay <= _attackTimer)
        {
            if (collision.tag == "Monster")
            {
                soundManager.effect.PlayOneShot(soundManager.rollingPinEffect);
                StartCoroutine(Util.CoSummonDamageViewer(collision.gameObject, (_weaponAttack * _level) + _playerStat.Attack));
                _attackTimer = 0.0f;
            }

        }
        else
        {
            _attackTimer += Time.deltaTime;
        }
    }
    public void LevelUp()
    {
        _level++;
    }
}
