using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// start문에 넣을 몬스터 설정 메뉴얼
// 1. monsterType 설정
// 2. base.Init()에 타입별로 필수적으로 초기화 해야 할 것들을 설정 해놓음
// 3. 드롭 골드
// 4. 드롭 경험치
// 5. MakeIngredient() 함수 이용해서 드롭 아이템들 ID와 확률 정해주기

public class Monster_2002_Controller : MonsterController
{
    [SerializeField]
    GameObject[] _spawnPoint;
    private float _projectileTimerDelay = 1f;
    [SerializeField]
    private Define.BossSkillType _bossSkillType = Define.BossSkillType.None;
    [SerializeField]
    private float _skillThrowSpearDelay = 10f;
    [SerializeField]
    private float _skillThrowSpearTimer = 10f;
    [SerializeField]
    private GameObject _spear;
    // 흑화 진입하면서 발동하는 즉사기 사용 여부
    bool _canDeadFlying = false;
    void Start()
    {
        _monsterType = Define.MonsterType.Running;
        base.Init();
        _projectileNumber = 5;
        _score = 25;
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

        MakeMainIngredient(new int[] { 37 }, new float[] { 30f, 30f });
        MakeSpecialIngredient(new int[] { 42, 43 }, new float[] { 5f, 5f });


        _projectileDamage = 1000;
        _projectileTimer = 1f;
        _isFireDiagonal = true;
        // 강화 부분
        SetUpgradeToCycle();
        _spear.GetComponent<SpearController>()._damage *= GameManager.killLuciferCount;
    }
    // 스킬 내용
    IEnumerator CoSkillThrowSpear()
    {
        //_monsterState = Define.MonsterState.Skill;
        _bossSkillType = Define.BossSkillType.ThrowSpear;

        soundManager.effect.PlayOneShot(soundManager.judgementEffect);
        _spear.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _bossSkillType = Define.BossSkillType.None;
    }
    // 스킬 내용
    IEnumerator CoChangeChaos()
    {
        //_monsterState = Define.MonsterState.Skill;
        //_bossSkillType = Define.BossSkillType.Chaos;
        yield return new WaitForSeconds(1f);
        monsterId = 2003;
        // TODO 흑화 버프
        //_projectileDamage *= 2;
        //_spear.GetComponent<SpearController>()._damage *= 2;

        _canDeadFlying = true;
        _bossSkillType = Define.BossSkillType.None;
        _monsterState = Define.MonsterState.Run;
    }
    

    void Update()
    {
        if (!isDead)
        {

            _monsterScanState = Define.MonsterScanState.Chase;
            CollisionDamage();
            FollowPlayer();
            UpdateAnimation();

            if ((_stat.HP / _stat.MaxHP) * 100f <= 50f)
            {
                //StartCoroutine(CoChangeChaos());
                _projectileDamage *= 2;
                _spear.GetComponent<SpearController>()._damage *= 2;
            }

            if (_skillThrowSpearTimer <= 0)
            {
                StartCoroutine(CoSkillThrowSpear());
                _skillThrowSpearTimer = _skillThrowSpearDelay;
            }
            else
            {
                _skillThrowSpearTimer -= Time.deltaTime;
            }

            if (_projectileTimer <= 0)
            {
                StartCoroutine(CoFireProjectile());
                _projectileTimer = _projectileTimerDelay;
            }
            else
            {
                _projectileTimer -= Time.deltaTime;
            }
        }
    }
    public override int OnDamaged(int damage)
    {
        int dmg = base.OnDamaged(damage);
        if (isDead)
        {
            //monsterId = 2002;
            // TODO 우선 클리어 함
            soundManager.effect.PlayOneShot(soundManager.luciferDieEffect);
            GameManager.killLuciferCount++;
        }
        return dmg;
    }
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
    }
}
