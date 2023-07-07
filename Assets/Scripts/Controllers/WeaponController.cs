using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    // ������Ʈ ���� ����� ��
    [SerializeField]
    protected GameObject _attackRangeBox;
    protected PlayerController _player;
    [SerializeField]
    protected Stat _playerStat;
    // ���߿� json���� ����
    public int _id;
    public int _level = 1;
    public int _weaponAttack;
    public int _increaseAttackRatio;
    public float _attackDelay;
    protected float _attackTimer = 0f;
    public int _increaseAttackDealyRatio;
    public Vector3 _attackBoxSize;
    public Vector3 _attackBoxPos;
    public LayerMask _layerMask;

    protected virtual void WeaponAttack()
    {

    }
}
