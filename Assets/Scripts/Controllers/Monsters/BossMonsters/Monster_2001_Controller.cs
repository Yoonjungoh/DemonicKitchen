using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// start문에 넣을 몬스터 설정 메뉴얼
// 1. monsterType 설정
// 2. base.Init()에 타입별로 필수적으로 초기화 해야 할 것들을 설정 해놓음
// 3. 드롭 골드
// 4. 드롭 경험치
// 5. MakeIngredient() 함수 이용해서 드롭 아이템들 ID와 확률 정해주기

public class Monster_2001_Controller : MonsterController
{
    [SerializeField]
    GameObject[] _spawnPoint;
    private float _projectileTimerDelay = 1f;
    [SerializeField]
    private Define.BossSkillType _bossSkillType = Define.BossSkillType.None;
    private float _skillLaserTimer = 0f;
    [SerializeField]
    private float _skillLaserDelay = 15f;
    [SerializeField]
    private GameObject _laser;

    private float _chargingTimer = 1f;
    [SerializeField]
    private float _chargingDelay = 1f;

    private bool _canUseLaser = true;
    private bool _canFireProjectile = true;
    void Start()
    {
        _monsterType = Define.MonsterType.Running;
        base.Init();
        _projectileNumber = 3;
        _score = 15;
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

        MakeMainIngredient(new int[] { 29, 30 }, new float[] { 30f, 30f });
        MakeSpecialIngredient(new int[] { 40, 41 }, new float[] { 5f, 5f });


        _projectileDamage = 300;
        _projectileTimer = 1f;
        _isFireDiagonal = true;
        _skillLaserTimer = _skillLaserDelay;
        _chargingTimer = _chargingDelay;
    }
    // 스킬 내용
    IEnumerator CoSkillLaser()
    {
        _monsterState = Define.MonsterState.Idle;
        yield return new WaitForSeconds(_chargingDelay);
        _monsterState = Define.MonsterState.Skill;
        _bossSkillType = Define.BossSkillType.Laser;

        soundManager.effect.PlayOneShot(soundManager.laserEffect);

        _laser.SetActive(true);
        switch (_dir)
        {
            case Define.MoveDir.Right:
                _animator.Play("MONSTER_2001_ATTACK_RIGHT");
                transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                break;
            case Define.MoveDir.Left:
                _animator.Play("MONSTER_2001_ATTACK_RIGHT");
                transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                break;
        }
        _canFollow = true;
        _canUseLaser = true;
        _canFireProjectile = true;
        _skillLaserTimer = _skillLaserDelay;
        _projectileTimer = _projectileTimerDelay;
        yield return new WaitForSeconds(0.5f);
        _bossSkillType = Define.BossSkillType.None;
        _monsterState = Define.MonsterState.Run;
        _laser.SetActive(false);
    }
    
    void Update()
    {
        if (!isDead)
        {
            _monsterScanState = Define.MonsterScanState.Chase;
            CollisionDamage();
            FollowPlayer();
            UpdateAnimation();

            if (_skillLaserTimer <= 0 && _canUseLaser)
            {
                _canFollow = false;
                _canUseLaser = false;
                _canFireProjectile = false;
                StartCoroutine(CoSkillLaser());
            }
            else
            {
                _skillLaserTimer -= Time.deltaTime;
            }

            if (_projectileTimer <= 0 && _canFireProjectile)
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
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        //if (_monsterState == Define.MonsterState.Skill)
        //{
        //    if (_bossSkillType == Define.BossSkillType.Laser)
        //    {
        //        switch (_dir)
        //        {
        //            case Define.MoveDir.Right:
        //                _animator.Play($"MONSTER_{monsterId}_SKILL_ATTACK_RIGHT");
        //                transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
        //                break;
        //            case Define.MoveDir.Left:
        //                _animator.Play($"MONSTER_{monsterId}_RUN_ATTACK_RIGHT");
        //                transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
        //                break;
        //        }
        //    }
        //}

    }
    public override int OnDamaged(int damage)
    {
        int dmg = base.OnDamaged(damage);
        if (isDead)
        {
            soundManager.effect.PlayOneShot(soundManager.pigDieEffect);
        }
        return dmg;
    }
}
