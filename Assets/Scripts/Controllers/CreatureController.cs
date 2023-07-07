using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CreatureController : MonoBehaviour
{
    public bool isDead = false;
    public HPSliderController _hpBarSlider;
    public Stat _stat;
    protected Define.CreatureType _creatureType;
    public int _minDamage = 5;
    public virtual int OnDamaged(int damage)
    {
        damage -= _stat.Defense;
        if (_creatureType == Define.CreatureType.Player && damage <= _minDamage)
            damage = _minDamage;
        _stat.HP -= damage;
        // 磷绰 何盒
        if (_stat.HP <= 0)
        {
            _stat.HP = 0;
            // 磷绰 葛记 贸府
            isDead = true;
        }

        return damage;
    }
    protected abstract void UpdateAnimation();
    protected virtual void Init()
    {
        _stat = GetComponent<Stat>();

    }
}
