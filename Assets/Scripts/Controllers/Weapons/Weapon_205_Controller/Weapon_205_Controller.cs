using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_205_Controller : WeaponController
{
    public GameObject _pan;
    // �ִϸ��̼� ������ Ÿ�ֿ̹� ���缭 pan ������Ʈ ����
    SoundManager soundManager;
    [SerializeField]
    protected float _animationTime = 0.5f;
    private void Start()
    {
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
    }
    void Update()
    {
        WeaponAttack();
    }
    protected override void WeaponAttack()
    {

        if (_attackDelay <= _attackTimer)
        {
            soundManager.effect.PlayOneShot(soundManager.panEffect);
            StartCoroutine(CoSpawnPan());
        }
        else
        {
            _attackTimer += Time.deltaTime;
        }
    }
    IEnumerator CoSpawnPan()
    {
        GameObject pan = Instantiate(_pan);
        pan.SetActive(true);
        pan.transform.SetParent(transform);
        pan.transform.position = transform.position;
        _attackTimer = 0.0f;
        yield return null;
    }
}
