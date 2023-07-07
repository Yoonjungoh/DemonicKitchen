using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_1101_Controller : MonsterController
{
    // start���� ���� ���� ���� �޴���
    // 1. monsterType ����
    // 2. base.Init()�� Ÿ�Ժ��� �ʼ������� �ʱ�ȭ �ؾ� �� �͵��� ���� �س���
    // 3. ��� ���
    // 4. ��� ����ġ
    // 5. MakeIngredient() �Լ� �̿��ؼ� ��� �����۵� ID�� Ȯ�� �����ֱ�
    void Start()
    {
        _monsterType = Define.MonsterType.Fixed;
        base.Init();
        _projectileNumber = 2;
        _score = 7;
        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            MakeCommonIngredient(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
            new float[] { 30f, 30f, 30f, 30f, 30f, 30f, 30f, 30f, 30f });
        }
        else
        {
            MakeCondimentIngredient(new int[] { 9, 10, 11, 12, 13, 14 }, new float[] { 30f, 30f, 30f, 30f, 30f, 30f });
        }

        MakeMainIngredient(new int[] { 25, 26 }, new float[] { 30f, 30f });
    }

    void Update()
    {
        PatrolEnemy();
        UpdateAnimation();
    }
    public override int OnDamaged(int damage)
    {
        int dmg = base.OnDamaged(damage);
        if (isDead)
        {
            soundManager.effect.PlayOneShot(soundManager.bugDieEffect);
        }
        return dmg;
    }
}
