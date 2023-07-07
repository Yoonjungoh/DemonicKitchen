using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_201_Controller : WeaponController
{
    Animator animator1;
    Animator animator2;
    SoundManager soundManager;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        _playerStat = _player.GetComponent<Stat>();
        _layerMask = 7; // 몬스터 레이어 마스크
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
        animator1 = transform.GetChild(0).GetComponent<Animator>();
        animator2 = transform.GetChild(1).GetComponent<Animator>();


        animator1.Play("WEAPON_201_ATTACK_EFFECT_1_RIGHT");
        animator2.Play("WEAPON_201_ATTACK_EFFECT_2_RIGHT");
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        WeaponAttack();
    }

    public void LevelUp()
    {
        _level++;
    }
    protected override void WeaponAttack()
    {
        if (_attackDelay <= _attackTimer)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_attackRangeBox.transform.position + _attackBoxPos, _attackRangeBox.transform.localScale + _attackBoxSize, _layerMask);
            foreach (Collider2D monster in colliders)
            {
                if (monster.tag == "Monster")
                {
                    soundManager.effect.PlayOneShot(soundManager.cleaverEffect);
                    StartCoroutine(Util.CoSummonDamageViewer(monster.gameObject, (_weaponAttack * _level) + _playerStat.Attack));
                    _attackTimer = 0.0f;
                }
            }
        }
        else
        {
            _attackTimer += Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackRangeBox.transform.position + _attackBoxPos, _attackRangeBox.transform.localScale + _attackBoxSize);
        //Gizmos.DrawWireCube(_attackBoxPos, _attackBoxSize);
    }
}
