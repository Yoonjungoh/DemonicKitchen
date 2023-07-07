using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_1002_Controller : MonsterController
{
    // start문에 넣을 몬스터 설정 메뉴얼
    // 1. monsterType 설정
    // 2. base.Init()에 타입별로 필수적으로 초기화 해야 할 것들을 설정 해놓음
    // 3. 드롭 골드
    // 4. 드롭 경험치
    // 5. MakeIngredient() 함수들 이용해서 드롭 아이템들 ID와 확률 정해주기
    void Start()
    {
        _monsterType = Define.MonsterType.Running;
        base.Init();
        _score = 14;

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

        MakeMainIngredient(new int[] { 31, 32 }, new float[] { 30f, 30f });
        SetUpgradeToCycle();
    }

    void Update()
    {
        PatrolEnemy();
        //CollisionDamage();
        UpdateAnimation();
    }
    public override int OnDamaged(int damage)
    {
        int dmg = base.OnDamaged(damage);
        if (isDead)
        {
            soundManager.effect.PlayOneShot(soundManager.sheepDieEffect);
        }
        return dmg;
    }
}
