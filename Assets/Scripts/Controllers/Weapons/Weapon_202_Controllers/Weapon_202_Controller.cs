using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_202_Controller : WeaponController
{
    Animator animator;
    [SerializeField]
    protected float _attackRadious;
    [SerializeField]
    protected float _spinSpeed = 50f;
    protected void Init()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        _playerStat = _player.GetComponent<Stat>();
        _layerMask = 7; // ���� ���̾� ����ũ


    }


    void Update()
    {
        // ȸ��
        transform.Rotate(new Vector3(0f, 0f, _spinSpeed) * Time.deltaTime);
    }
}
