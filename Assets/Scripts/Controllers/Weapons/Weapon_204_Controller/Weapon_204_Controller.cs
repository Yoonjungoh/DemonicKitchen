using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_204_Controller : WeaponController
{
    public float _rateSpeed;
    public GameObject _turner;
    [SerializeField]
    protected Vector3 _turnerScale;
    [SerializeField]
    protected int _turnerCount = 3;
    void Start()
    {
        _turner = transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        WeaponAttack();
    }

    protected override void WeaponAttack()
    {
        // 공속 계산 공식
        if (_attackDelay * Mathf.Pow(0.9f, (_turner.GetComponent<WeaponController>()._level)) <= _attackTimer)
        {
            StartCoroutine(CoSpawnTurner());
        }
        else
        {
            _attackTimer += Time.deltaTime;
        }
    }
    protected IEnumerator CoSpawnTurner()
    {
        for(int i = 0; i < _turnerCount; i++)
        {
            GameObject turner = Instantiate(_turner, transform.position, Quaternion.identity);
            // 스폰 포인트에 터너 소환
            turner.transform.position = transform.GetChild(i + 1).transform.position;
            turner.SetActive(true);
        }
        _attackTimer = 0;

        yield return null;
    }

}
