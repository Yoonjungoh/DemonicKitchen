using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// start���� ���� ���� ���� �޴���
// 1. monsterType ����
// 2. base.Init()�� Ÿ�Ժ��� �ʼ������� �ʱ�ȭ �ؾ� �� �͵��� ���� �س���
// 3. ��� ���
// 4. ��� ����ġ
// 5. MakeIngredient() �Լ� �̿��ؼ� ��� �����۵� ID�� Ȯ�� �����ֱ�

public class Monster_2000_Controller : MonsterController
{
    [SerializeField]
    GameObject[] _spawnPoint;
    [SerializeField]
    private Define.BossSkillType _bossSkillType = Define.BossSkillType.None;
    private float _skillSummonTimer = 10f;
    // ��ȯ ��ų ������
    [SerializeField]
    private float _skillSummonDelay = 10f;
    // ��ȯ ���� ��
    [SerializeField]
    private int _summonMonsterCount = 2;
    // �߻�ü ������ �θ����� ���� _projectileTimer
    // ��ȯ ��ų ������
    [SerializeField]
    private float _projectileTimerDelay = 2f;
    private bool _isCharging = false;
    void Start()
    {
        _skillSummonTimer = _skillSummonDelay;
        _monsterType = Define.MonsterType.Running;
        base.Init();
        _projectileNumber = 1;
        _score = 10;
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

        MakeMainIngredient(new int[] { 21, 22 }, new float[] { 30f, 30f });
        MakeSpecialIngredient(new int[] { 38, 39 }, new float[] { 5f, 5f });
    }
    // ��ų ��� ����
    IEnumerator CoSkillSummon()
    {
        _isCharging = true;
        yield return new WaitForSeconds(1.5f);
        _isCharging = false;
        _monsterState = Define.MonsterState.Skill;
        _bossSkillType = Define.BossSkillType.Summon;
        for (int i = 0; i < _summonMonsterCount; i++)
        {
            GameObject Monster = Util.Instantiate("Monsters/CommonMonsters/Flying/Monster_1200");
            Monster.GetComponent<MonsterController>().MonsterScanState = Define.MonsterScanState.Chase;
            Monster.transform.position = _spawnPoint[i].transform.position;
        }

        soundManager.effect.PlayOneShot(soundManager.summonEffect);

        yield return new WaitForSeconds(0.5f);
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
            if (_skillSummonTimer <= 0)
            {
                StartCoroutine(CoSkillSummon());
                _skillSummonTimer = _skillSummonDelay;
            }
            else
            {
                _skillSummonTimer -= Time.deltaTime;
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
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        if (_monsterState == Define.MonsterState.Skill)
        {
            if (_bossSkillType == Define.BossSkillType.Summon)
            {
                // Monster_2000���״� ��ų ����� ���� ��� �����൵ ��
                //switch (_dir)
                //{
                //    case Define.MoveDir.Right:
                //        _animator.Play($"MONSTER_{monsterId}_SKILL_SUMMON_RIGHT");
                //        transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                //        break;
                //    case Define.MoveDir.Left:
                //        _animator.Play($"MONSTER_{monsterId}_RUN_SUMMON_RIGHT");
                //        transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                //        break;
                //}
            }
        }

    }
    public override int OnDamaged(int damage)
    {
        int dmg = base.OnDamaged(damage);
        if (isDead)
        {
            soundManager.effect.PlayOneShot(soundManager.dogDieEffect);
        }
        return dmg;
    }
}
