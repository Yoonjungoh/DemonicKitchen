using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public Stat(int maxHp, float hp, int attack, int defense, float maxSpeed)
    {
        _maxHp = maxHp;
        _hp = hp;
        _attack = attack;
        _defense = defense;
        _maxSpeed = maxSpeed;
    }
    [SerializeField]
    public int _maxHp;
    [SerializeField]
    public float _hp;
    [SerializeField]
    public int _attack;
    [SerializeField]
    public int _defense;
    [SerializeField]
    public float _maxSpeed = 1f;
    [SerializeField]
    private float _jumpSpeed = 5f;
    // 충돌시 데미지 주기 - 몬스터 전용
    [SerializeField]
    private int _collisionAttack;

    public int MaxHP { get { return _maxHp; } set { _maxHp = value; } }
    public float HP { get { return _hp; } set { _hp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
    public float JumpSpeed { get { return _jumpSpeed; } set { _jumpSpeed = value; } }
    public int CollisionAttack { get { return _collisionAttack; } set { _collisionAttack = value; } }
}
