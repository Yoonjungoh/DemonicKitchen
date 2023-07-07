using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_203_Controller : WeaponController
{
    public float _rateSpeed;
    public GameObject _knife;
    [SerializeField]
    protected Vector3 _knifeScale;
    void Start()
    {
        _knife = transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        WeaponAttack();
    }
    protected override void WeaponAttack()
    {
        if (_attackDelay * Mathf.Pow(0.9f, (_knife.GetComponent<WeaponController>()._level)) <= _attackTimer)
        {
            StartCoroutine(CoSpawnKnife());
        }
        else
        {
            _attackTimer += Time.deltaTime;
        }
    }
    protected IEnumerator CoSpawnKnife()
    {
        GameObject knife = Instantiate(_knife, transform.position, Quaternion.identity);
        knife.SetActive(true);

        _attackTimer = 0;

        yield return null;
    }

}
