using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterController : CreatureController
{
    protected SoundManager soundManager;
    protected GameManager gameManager;
    // 스폰된 위치의 플랫폼 오브젝트
    [SerializeField]
    protected GameObject _spawnedPlatform;
    protected Rigidbody2D _rb;
    protected Animator _animator;
    protected GameObject _enemy;
    protected GameObject _hpBarObject;
    protected HPSliderController _hpBar;
    // 몬스터가 드롭하는 일반 재료의 리스트 목록
    protected List<IngredientInfo> _dropCommonIngredientList = new List<IngredientInfo>();
    // 몬스터가 드롭하는 조미료의 리스트 목록
    protected List<IngredientInfo> _dropCondimentIngredientList = new List<IngredientInfo>();
    // 몬스터가 드롭하는 메인 재료의 리스트 목록
    protected List<IngredientInfo> _dropMainIngredientList = new List<IngredientInfo>();
    // 몬스터가 드롭하는 스페셜 재료의 리스트 목록
    protected List<IngredientInfo> _dropSpecialIngredientList = new List<IngredientInfo>();

    // 몬스터가 드롭하는 일반 재료의 확률
    protected List<float> _commonIngredientProbabilityList = new List<float>();
    // 몬스터가 드롭하는 조미료의 확률
    protected List<float> _condimentIngredientProbabilityList = new List<float>();
    // 몬스터가 드롭하는 메인 재료의 확률
    protected List<float> _mainIngredientProbabilityList = new List<float>();
    // 몬스터가 드롭하는 특수 재료의 확률
    protected List<float> _specialIngredientProbabilityList = new List<float>();



    [SerializeField]
    protected Define.MonsterType _monsterType;
    [SerializeField]
    protected Define.MonsterState _monsterState = Define.MonsterState.Idle;
    [SerializeField]
    protected Define.MonsterScanState _monsterScanState = Define.MonsterScanState.Patrol;
    public Define.MonsterScanState MonsterScanState { get { return _monsterScanState; } set { _monsterScanState = value; } }
    [SerializeField]
    protected Define.MoveDir _dir = Define.MoveDir.Left;
    [SerializeField]
    protected int monsterId;
    [SerializeField]
    protected int _exp;
    public int Exp { get { return _exp; } set { _exp = value; } }
    [SerializeField]
    protected int _gold;
    public int Gold { get { return _gold; } set { _gold = value; } }
    // 몬스터가 chasing 하기전 좌표값
    Vector3 _originalPos;
    // 몬스터가 chasing 하는 목표물의 좌표값
    Vector3 _destPos;
    [SerializeField]
    // 발사체 쏘는 딜레이
    private float _ratingDelay = 1f;
    // 몬스터들의 공격 인지 범위 X축 (현재 Running만 적용중)
    [SerializeField]
    private float _attackRangeX = 1.0f;
    // 몬스터들의 공격 인지 범위 Y축 (현재 Running만 적용중)
    [SerializeField]
    private float _attackRangeY = 1.0f;
    // 몬스터들의 공격 인지 범위 Height 이동 걸 (현재 Running만 적용중)
    [SerializeField]
    private float _attackRangeHeight = 1.0f;
    // 몬스터들의 공격 인지 범위 Height 이동 걸 (현재 Running만 적용중)
    [SerializeField]
    private float _attackRangeWidth = 1.0f;
    [SerializeField]
    // 몬스터 크기 값 설정
    protected float _monsterScale = 1.0f;
    [SerializeField]
    // 몬스터 스프라이트 뒤집현 경우 true해주기
    protected bool _isReverse = true;
    [SerializeField]
    // hp bar의 x scale
    protected float _hpBarScaleX = 0.01f;
    [SerializeField]
    // hp bar의 y scale
    protected float _hpBarScaleY = 0.01f;
    [SerializeField]
    // hb bar의 높이 위치 조절
    protected float _hpBarHeight = 0.5f;
    [SerializeField]
    // hb bar의 넓이 위치 조절
    protected float _hpBarWeight = 0.0f;
    [SerializeField]
    protected int _projectileNumber;
    [SerializeField]
    protected float _projectileTimer = 2f;
    [SerializeField]
    // 발사체 인덱스
    protected int _projectileSpriteIndex = 0;
    [SerializeField]
    // 발사체 데미지
    protected int _projectileDamage = 0;
    [SerializeField]
    // 발사체 데미지
    protected float _rateSpeed = 1f;
    // 투사체를 대각선으로 쏴야할 때
    protected bool _isFireDiagonal = false;
    [SerializeField]
    protected bool _canFollow = true;
    protected int _score;
    protected override void Init()
    {   
        base.Init();
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
        _creatureType = Define.CreatureType.Monster;
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
        gameObject.tag = "Monster";
        SetMonsterLayer();
        SetHPBar();
        SetMonsterInfo();
    }
    // 레이어 지정해주는 함수
    protected void SetMonsterLayer()
    {
        gameObject.layer = 7; // 몬스터 레이어
        // 자식도 몬스터 레이어 지정
        if (gameObject.transform.childCount != 0 && gameObject.transform.GetChild(0).gameObject.layer == 0)
            gameObject.transform.GetChild(0).gameObject.layer = 7; // 몬스터 레이어
    }
    // 플랫폼 감지를 위해 아래로 Ray를 쏴서 spawnPlatform 오브젝트 감지
    [SerializeField]
    float _rayLength = 10f;
    public void ScanObject()
    {
        Debug.DrawRay(gameObject.transform.position, Vector3.down * _rayLength, Color.red);
        RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position, Vector3.down * _rayLength, LayerMask.GetMask("Platform"));
        foreach (RaycastHit2D hit in hits)
        {
            if(hit.collider.gameObject.tag == "Platform")
            {
                _spawnedPlatform = hit.collider.gameObject;
                return;
            }
            else
                _spawnedPlatform = null;
        }
    }
    protected void SetHPBar()
    {
        Transform hpBar = transform.Find("HPBar");
        if (hpBar == null)
        {
            GameObject hpBarSlider = Resources.Load("Prefabs/UI/WorldSpace/HPBar") as GameObject;
            base._hpBarSlider = hpBarSlider.GetComponentInChildren<HPSliderController>();
            Vector3 instantiatePos = new Vector3(transform.position.x + _hpBarWeight,
                transform.position.y + _hpBarHeight,
                0);
            _hpBarObject = Instantiate(hpBarSlider, instantiatePos, Quaternion.identity, transform);
            _hpBarObject.transform.localScale = new Vector3(_hpBarScaleX, _hpBarScaleY, 1);
        }
        else
        {
            base._hpBarSlider = hpBar.GetComponentInChildren<HPSliderController>();
        }
    }
    protected void SetMonsterInfo()
    {
        switch (_monsterType)
        {
            case Define.MonsterType.Running:
                SetRunningMonster();
                break;
            case Define.MonsterType.Fixed:
                SetFixedMonster();
                break;
            case Define.MonsterType.Flying:
                SetFlyingMonster();
                break;
        }
    }
    protected void SetUpgradeToCycle()
    {
        gameObject.GetComponent<Stat>().MaxHP *= GameManager.killLuciferCount;
        gameObject.GetComponent<Stat>().HP *= GameManager.killLuciferCount;
        gameObject.GetComponent<Stat>().Attack *= GameManager.killLuciferCount;
        gameObject.GetComponent<Stat>().Defense *= GameManager.killLuciferCount;
        _projectileDamage *= GameManager.killLuciferCount;
        _exp *= GameManager.killLuciferCount;
        _gold *= GameManager.killLuciferCount;
    }
    protected void SetRunningMonster()
    {
        ScanObject();
        _enemy = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        _monsterScanState = Define.MonsterScanState.Patrol;
        _monsterState = Define.MonsterState.Run;
        _dir = Define.MoveDir.Right;
    }
    protected void SetFixedMonster()
    {
        ScanObject();
        _enemy = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        _monsterScanState = Define.MonsterScanState.Patrol;
        _monsterState = Define.MonsterState.Run;
        _dir = Define.MoveDir.Right;
    }
    protected void SetFlyingMonster()
    {
        _enemy = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        //// 공중에 뜨기 위함
        _rb.gravityScale = 0;
        _monsterScanState = Define.MonsterScanState.Patrol;
        _monsterState = Define.MonsterState.Fly;
        _dir = Define.MoveDir.Right;
    }
    // 업데이트에서 돌려야함
    protected void FollowPlayer()
    {
        // 죽지 않았고 따라가는 플래그 허락
        if (!isDead && _canFollow)
        {
            // 유저 향해 가는 로직
            _originalPos = transform.position;
            _destPos = _enemy.transform.position;
            _destPos = new Vector3(_destPos.x, _originalPos.y, 0);
            transform.position = Vector2.MoveTowards(_originalPos, _destPos, _stat.MaxSpeed * Time.deltaTime);
        }
    }
    protected void PatrolEnemy()
    {
        if (_monsterState == Define.MonsterState.Death)
            return;
        CollisionDamage();
        // patrol 중
        if (_monsterScanState == Define.MonsterScanState.Patrol)
        {
            switch (_monsterType)
            {
                case Define.MonsterType.Running:
                    PatrolRunning();
                    break;
                case Define.MonsterType.Fixed:
                    PatrolFixed();
                    break;
                case Define.MonsterType.Flying:
                    PatrolFlying();
                    break;
            }
        }
        // 적을 감지했을 때
        else if (_monsterScanState == Define.MonsterScanState.Chase)
        {
            switch (_monsterType)
            {
                case Define.MonsterType.Running:
                    FightRunning();
                    break;
                case Define.MonsterType.Fixed:
                    FightFixed();
                    break;
                case Define.MonsterType.Flying:
                    FightFlying();
                    break;
            }
        }
    }
    [SerializeField]
    protected float _patrolRayLength = 1.0f;
    protected void PatrolRunning()
    {
        if (_spawnedPlatform != null)
        {
            if (_isReverse == true)
            {
                switch (_dir)
                {
                    case Define.MoveDir.Right:
                        // 패트롤 이동
                        transform.position += new Vector3(Vector2.left.x * _stat.MaxSpeed * Time.deltaTime, 0, 0);
                        // 플랫폼 끝에 닿았을 때 계산
                        Debug.DrawRay(gameObject.transform.position + new Vector3(-_patrolRayLength, 0, 0), Vector3.down, Color.red);
                        RaycastHit2D hitLeft = Physics2D.Raycast(gameObject.transform.position + new Vector3(-_patrolRayLength, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));

                        if (hitLeft.collider != null)
                            //Debug.Log(hitLeft.collider.name);

                        // 플랫폼 ray 예외 상황 처리 하는 곳
                        if (hitLeft.collider != null && (hitLeft.collider.tag == "Switch" || hitLeft.collider.tag == "Monster"))
                                break;

                        // 플랫폼 끝 만나면 방향 전환
                        if (hitLeft.collider == null ||
                            hitLeft.collider.name != _spawnedPlatform.name)
                        {
                            _dir = Define.MoveDir.Left;
                        }
                        break;
                    case Define.MoveDir.Left:
                        // 패트롤 이동
                        transform.position += new Vector3(Vector2.right.x * _stat.MaxSpeed * Time.deltaTime, 0, 0);
                        // 플랫폼 끝에 닿았을 때 계산
                        Debug.DrawRay(gameObject.transform.position + new Vector3(_patrolRayLength, 0, 0), Vector3.down, Color.red);
                        RaycastHit2D hitRight = Physics2D.Raycast(gameObject.transform.position + new Vector3(_patrolRayLength, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));

                        if (hitRight.collider != null)
                            //Debug.Log(hitRight.collider.name);

                        // 플랫폼 ray 예외 상황 처리 하는 곳
                        if (hitRight.collider != null && (hitRight.collider.tag == "Switch" || hitRight.collider.tag == "Monster"))
                                break;

                        // 플랫폼 끝 만나면 방향 전환
                        if (hitRight.collider == null ||
                            hitRight.collider.name != _spawnedPlatform.name)
                        {
                            _dir = Define.MoveDir.Right;
                        }
                        break;
                }
            }
            else
            {
                switch (_dir)
                {
                    case Define.MoveDir.Left:
                        // 패트롤 이동
                        transform.position += new Vector3(Vector2.left.x * _stat.MaxSpeed * Time.deltaTime, 0, 0);
                        // 플랫폼 끝에 닿았을 때 계산
                        Debug.DrawRay(gameObject.transform.position + new Vector3(-1, 0, 0), Vector3.down, Color.red);
                        RaycastHit2D hitLeft = Physics2D.Raycast(gameObject.transform.position + new Vector3(-1, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));

                        if (hitLeft.collider != null)
                            //Debug.Log(hitLeft.collider.tag);

                            // 플랫폼 ray 예외 상황 처리 하는 곳
                            if (hitLeft.collider != null && (hitLeft.collider.tag == "Switch" || hitLeft.collider.tag == "Monster"))
                                break;

                        // 플랫폼 끝 만나면 방향 전환
                        if (hitLeft.collider == null ||
                            hitLeft.collider.name != _spawnedPlatform.name)
                        {
                            _dir = Define.MoveDir.Right;
                        }
                        break;
                    case Define.MoveDir.Right:
                        // 패트롤 이동
                        transform.position += new Vector3(Vector2.right.x * _stat.MaxSpeed * Time.deltaTime, 0, 0);
                        // 플랫폼 끝에 닿았을 때 계산
                        Debug.DrawRay(gameObject.transform.position + new Vector3(1, 0, 0), Vector3.down, Color.red);
                        RaycastHit2D hitRight = Physics2D.Raycast(gameObject.transform.position + new Vector3(1, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));

                        if (hitRight.collider != null)
                            //Debug.Log(hitRight.collider.tag);

                            // 플랫폼 ray 예외 상황 처리 하는 곳
                            if (hitRight.collider != null && (hitRight.collider.tag == "Switch" || hitRight.collider.tag == "Monster"))
                                break;

                        // 플랫폼 끝 만나면 방향 전환
                        if (hitRight.collider == null ||
                            hitRight.collider.name != _spawnedPlatform.name)
                        {
                            _dir = Define.MoveDir.Left;
                        }
                        break;
                }
            }
            // 플레이어 추적
            if (_enemy != null && _enemy.GetComponent<PlayerController>().CurSteppedPlatform != null)
            {
                if (_spawnedPlatform.name == _enemy.GetComponent<PlayerController>().CurSteppedPlatform.name)
                    _monsterScanState = Define.MonsterScanState.Chase;
            }

        }
    }   
    protected void PatrolFixed()
    {
        // 플레이어 추적
        if (_enemy.GetComponent<PlayerController>().CurSteppedPlatform != null)
        {
            if (_spawnedPlatform.name == _enemy.GetComponent<PlayerController>().CurSteppedPlatform.name)
                _monsterScanState = Define.MonsterScanState.Chase;
        }
    }
    protected void PatrolFlying()
    {
        // 플레이어 스캔
        // 유저의 y 값이 저 크면 따라감
        if (transform.position.y <= _enemy.transform.position.y)
            _monsterScanState = Define.MonsterScanState.Chase;
    }

    public float _curTime = 0f;
    protected void FightRunning()
    {
        // 유저 향해 가는 로직
        _originalPos = transform.position;
        _destPos = _enemy.transform.position;
        _destPos = new Vector3(_destPos.x, _originalPos.y, 0);
        transform.position = Vector2.MoveTowards(_originalPos, _destPos, _stat.MaxSpeed * Time.deltaTime);
        if (_curTime <= 0 && _canCommonAttack)
        {
            _canCommonAttack = false;
            // 근처에 유저가 있을 때 공격
            Collider2D[] collidersLeft = Physics2D.OverlapBoxAll(transform.position + new Vector3(_attackRangeWidth, _attackRangeHeight), new Vector3(_attackRangeX, _attackRangeY, 0), 0);
            foreach (Collider2D collider in collidersLeft)
            {
                if (collider.tag == "Player")
                {
                    // 몬스터 공격 모션에 맞춰서 한번만 SummonDamageViewer 하기
                    if (_isCommonAttackDelay)
                    {
                        StartCoroutine(Util.CoSummonDamageViewer(_enemy, _stat.Attack, _commonAttackDelayForCo));
                        _enemy.GetComponent<PlayerController>().monsterHittedMe = this.gameObject;
                    }
                    else
                    {
                        Util.SummonDamageViewer(_enemy, _stat.Attack);
                        _enemy.GetComponent<PlayerController>().monsterHittedMe = this.gameObject;
                    }
                    // attack 모션 부분
                    _monsterState = Define.MonsterState.Attack;
                    break;
                }
            }
            StartCoroutine("CoChangeStateDelay");
            // 쿨타임 초기화 부분
            _curTime = _commonAttackDelay;
        }
        else
        {
            //_monsterState = Define.MonsterState.Run;
            _curTime -= Time.deltaTime;
        }

        // 플랫폼 끝에 닿았을 때 계산
        Debug.DrawRay(gameObject.transform.position + new Vector3(-1, 0, 0), Vector3.down, Color.red);
        RaycastHit2D hitLeft = Physics2D.Raycast(gameObject.transform.position + new Vector3(-1, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));


        // 유저가 다른 플랫폼 밟았을 때 다시 patrol로 돌아가고 chase 종료
        if (_spawnedPlatform.gameObject.name != _enemy.GetComponent<PlayerController>().CurSteppedPlatform.name)
        {
            _monsterScanState = Define.MonsterScanState.Patrol;
            _monsterState = Define.MonsterState.Run;
        }
    }
    // ATTACK 모션에서 RUN으로 돌아오는 딜레이 코루틴 함수
    [SerializeField]
    public float _changeStateDelay = 0.7f;
    // 일반 공격 딜레이
    [SerializeField]
    protected float _commonAttackDelay = 1.0f;
    [SerializeField]
    protected bool _canCommonAttack = true;
    [SerializeField]
    protected bool _canRun = true;
    IEnumerator CoChangeStateDelay()
    {
        yield return new WaitForSeconds(_changeStateDelay);
        if (_monsterType == Define.MonsterType.Flying) 
            _monsterState = Define.MonsterState.Fly;
        else if (_monsterType == Define.MonsterType.Running)
            _monsterState = Define.MonsterState.Run;
        _canCommonAttack = true;
    }

    protected void FightFixed()
    {
        // 유저 향해 가는 로직
        // 고정형은 필요 없음
        FireProjectile();
        if(_spawnedPlatform.gameObject.name != _enemy.GetComponent<PlayerController>().CurSteppedPlatform.name)
        {
            _monsterScanState = Define.MonsterScanState.Patrol;
            _monsterState = Define.MonsterState.Idle;
        }
    }
    protected void FireProjectile()
    {
        _projectileTimer += Time.deltaTime;

        if (_projectileTimer > _ratingDelay)
        {
            //// 몬스터 공격 모션에 맞춰서 한번만 SummonDamageViewer 하기
            //if (_isCommonAttackDelay)
            //    StartCoroutine(Util.CoSummonDamageViewer(_enemy, _stat.Attack, _commonAttackDelayForCo));
            //else
            //    Util.SummonDamageViewer(_enemy, _stat.Attack);
            // attack 모션 부분
            //_monsterState = Define.MonsterState.Attack;
            _animator.Play($"MONSTER_{monsterId}_ATTACK_RIGHT");
            Vector3 pos = transform.GetChild(0).position;
            if (transform.Find("ProjectileSpawnPoint") != null) 
            {
                pos = transform.Find("ProjectileSpawnPoint").position;
            }
            GameObject projectile = Resources.Load<GameObject>($"Prefabs/Projectile_{_projectileNumber}");
            projectile.GetComponent<ProjectileController>().Init(_projectileSpriteIndex, _projectileDamage, _rateSpeed);
            projectile.GetComponent<ProjectileController>().masterMonster = this.gameObject;
            Instantiate(projectile, pos, Quaternion.identity);
            _projectileTimer = 0;
        }
    }
    protected IEnumerator CoFireProjectile()
    {
        //_monsterState = Define.MonsterState.Attack;
        _animator.Play($"MONSTER_{monsterId}_ATTACK_RIGHT");
        Vector3 pos = transform.GetChild(0).position;
        if (transform.Find("ProjectileSpawnPoint") != null)
        {
            pos = transform.Find("ProjectileSpawnPoint").position;
        }
        GameObject projectile = Resources.Load<GameObject>($"Prefabs/Projectile_{_projectileNumber}");
        if (_isFireDiagonal)
        {
            projectile.GetComponent<ProjectileController>()._isFireDiagonal = true;
            projectile.GetComponent<ProjectileController>().Init(_projectileSpriteIndex, _projectileDamage, _rateSpeed);
        }
        else
            projectile.GetComponent<ProjectileController>().Init(_projectileSpriteIndex, _projectileDamage, _rateSpeed);

        Instantiate(projectile, pos, Quaternion.identity);
        _projectileTimer = 0;
        yield return new WaitForSeconds(0.5f);
        _monsterState = Define.MonsterState.Run;
    }
    [SerializeField]
    protected float anchorHeight = 0f;
    protected float _commonAttackTimer = 0f;
    [SerializeField]
    protected bool _isCommonAttackDelay = true;
    [SerializeField]
    // 이 변수로 데미지 입는 타이밍 통제 가능
    protected float _commonAttackDelayForCo = 0.3f;
    protected void FightFlying()
    {
        // 유저 향해 가는 로직
        //_monsterState = Define.MonsterState.Attack;
        _originalPos = transform.position;
        _destPos = _enemy.transform.position;
        transform.position = Vector2.MoveTowards(_originalPos, _destPos + new Vector3(0, anchorHeight, 0), _stat.MaxSpeed * Time.deltaTime);

        // 근처에 유저가 있을 때 공격
        if (_curTime <= 0 && _canCommonAttack)
        {
            _canCommonAttack = false;
            // 근처에 유저가 있을 때 공격
            Collider2D[] collidersLeft = Physics2D.OverlapBoxAll(transform.position + new Vector3(_attackRangeWidth, _attackRangeHeight), new Vector3(_attackRangeX, _attackRangeY, 0), 0);
            foreach (Collider2D collider in collidersLeft)
            {
                if (collider.tag == "Player")
                {
                    // 몬스터 공격 모션에 맞춰서 한번만 SummonDamageViewer 하기
                    if (_isCommonAttackDelay)
                    {
                        StartCoroutine(Util.CoSummonDamageViewer(_enemy, _stat.Attack, _commonAttackDelayForCo));
                        _enemy.GetComponent<PlayerController>().monsterHittedMe = this.gameObject;
                    }
                    else
                    {
                        Util.SummonDamageViewer(_enemy, _stat.Attack);
                        _enemy.GetComponent<PlayerController>().monsterHittedMe = this.gameObject;
                    }
                    // attack 모션 부분
                    _monsterState = Define.MonsterState.Attack;
                    break;
                }
            }
            StartCoroutine("CoChangeStateDelay");
            // 쿨타임 초기화 부분
            _curTime = _commonAttackDelay;
        }
        else
        {
            //_monsterState = Define.MonsterState.Run;
            _curTime -= Time.deltaTime;
        }
    }
    protected override void UpdateAnimation()
    {
        if (isDead)
            return;
        if (_monsterState == Define.MonsterState.Idle)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play($"MONSTER_{monsterId}_IDLE_RIGHT");
                    transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play($"MONSTER_{monsterId}_IDLE_RIGHT");
                    transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                    break;
            }
        }
        else if (_monsterState == Define.MonsterState.Run)
        {

            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play($"MONSTER_{monsterId}_RUN_RIGHT");
                    transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play($"MONSTER_{monsterId}_RUN_RIGHT");
                    transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                    break;
            }
        }
        else if (_monsterState == Define.MonsterState.Jump)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play($"MONSTER_{monsterId}_JUMP_RIGHT");
                    transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play($"MONSTER_{monsterId}_JUMP_RIGHT");
                    transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                    break;
            }
        }
        else if (_monsterState == Define.MonsterState.Fly)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play($"MONSTER_{monsterId}_FLIGHT_RIGHT");
                    transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play($"MONSTER_{monsterId}_FLIGHT_RIGHT");
                    transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                    break;
            }
        }
        else if (_monsterState == Define.MonsterState.Attack)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play($"MONSTER_{monsterId}_ATTACK_RIGHT");
                    transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play($"MONSTER_{monsterId}_ATTACK_RIGHT");
                    transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                    break;
            }
        }
        else if (_monsterState == Define.MonsterState.Death)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play($"MONSTER_{monsterId}_DEATH_RIGHT");
                    transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play($"MONSTER_{monsterId}_DEATH_RIGHT");
                    transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                    break;
            }
        }
        //// MonsterScanState가 추격중일 때 몬스터들의 스프라이트를 유저의 방향에 맞게 변경
        if (_monsterScanState == Define.MonsterScanState.Chase)
        {
            if(_isReverse == false)
            {
                if (_enemy.transform.position.x > transform.position.x)
                    _dir = Define.MoveDir.Right;
                else
                    _dir = Define.MoveDir.Left;
            }
            else
            {
                if (_enemy.transform.position.x < transform.position.x)
                    _dir = Define.MoveDir.Right;
                else
                    _dir = Define.MoveDir.Left;
            }
        }
    }
    public override int OnDamaged(int damage)
    {
        int dmg = base.OnDamaged(damage);
        if (isDead)
        {
            _monsterState = Define.MonsterState.Death;
            _monsterScanState = Define.MonsterScanState.None;
            gameObject.tag = "DeadMonster";
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play($"MONSTER_{monsterId}_DEATH_RIGHT");
                    transform.localScale = new Vector3(_monsterScale, _monsterScale, _monsterScale);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play($"MONSTER_{monsterId}_DEATH_RIGHT");
                    transform.localScale = new Vector3(-_monsterScale, _monsterScale, _monsterScale);
                    break;
            }
            // 경험치 획득
            _enemy.GetComponent<PlayerController>().TotalExp += Exp;
            DropGold();
            DropCommonIngredient();
            DropCondimentIngredient();
            DropMainIngredient();
            gameManager.toatlScore += _score;
            StartCoroutine(CoSetOffCorpse());
        }
        return dmg;
    }
    // id = 재료 id , probability = 확률
    protected void MakeCommonIngredient(int[] id, float[] probability)
    {
        for(int i = 0; i < id.Length; i++)
        {
            _dropCommonIngredientList.Add(IngredientData.data[id[i]]);
            _commonIngredientProbabilityList.Add(probability[i]);
        }
    }
    // id = 재료 id , probability = 확률
    protected void MakeCondimentIngredient(int[] id, float[] probability)
    {
        for (int i = 0; i < id.Length; i++)
        {
            _dropCondimentIngredientList.Add(IngredientData.data[id[i]]);
            _condimentIngredientProbabilityList.Add(probability[i]);
        }
    }
    // id = 재료 id , probability = 확률
    protected void MakeMainIngredient(int[] id, float[] probability)
    {
        for (int i = 0; i < id.Length; i++)
        {
            _dropMainIngredientList.Add(IngredientData.data[id[i]]);
            _mainIngredientProbabilityList.Add(probability[i]);
        }
    }
    // id = 재료 id , probability = 확률
    protected void MakeSpecialIngredient(int[] id, float[] probability)
    {
        for (int i = 0; i < id.Length; i++)
        {
            _dropSpecialIngredientList.Add(IngredientData.data[id[i]]);
            _specialIngredientProbabilityList.Add(probability[i]);
        }
    }

    // 골드 획득 함수
    protected void DropGold()
    {
        GameObject gold = Util.Instantiate("DropItems/Gold");
        gold.transform.position = transform.position;
        gold.GetComponent<DropGoldController>()._gold = _gold;
    }
    //// 골드 획득 함수
    //protected void DropIngredientObject()
    //{
    //    GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
    //    ingredient.transform.position = transform.position;
    //}
    // 몬스터가 죽은 후 플레이어에게 재료가 갈지 말지 정하는 함수
    protected void DropCommonIngredient()
    {
        List<IngredientInfo> dropItemList = new List<IngredientInfo>();
        for (int i = 0; i < _dropCommonIngredientList.Count; i++)
        {
            float randNumber = Random.Range(1f, 100f);
            if (randNumber <= _commonIngredientProbabilityList[i])
            {
                dropItemList.Add(_dropCommonIngredientList[i]);
            }
        }
        // 얻은 아이템 없으면 함수 종료
        if (dropItemList.Count == 0)
            return;
        // 얻은 아이템중 하나만 획득
        int randomIndex = Random.Range(0, dropItemList.Count);

        // 실제로 얻은 재료 시각화 하는 부분 start
        GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
        ingredient.transform.position = transform.position;
        ingredient.GetComponent<DropIngredientController>()._ingredient = dropItemList[randomIndex];
        // end

        Debug.Log(dropItemList[randomIndex]._ingredientName);
        // 플레이어 없어져도 gameManager에 재료 정보 저장
        // 추후에 다른 정보도 저장 ex gold, exp, score TODO
        gameManager.ingredientDataList = _enemy.GetComponent<PlayerController>()._ingredientDataList;
    }
    // 몬스터가 죽은 후 플레이어에게 재료가 갈지 말지 정하는 함수
    protected void DropCondimentIngredient()
    {
        List<IngredientInfo> dropItemList = new List<IngredientInfo>();
        for (int i = 0; i < _dropCondimentIngredientList.Count; i++)
        {
            float randNumber = Random.Range(1f, 100f);
            if (randNumber <= _condimentIngredientProbabilityList[i])
            {
                dropItemList.Add(_dropCondimentIngredientList[i]);
            }
        }
        // 얻은 아이템 없으면 함수 종료
        if (dropItemList.Count == 0)
            return;
        // 얻은 아이템중 하나만 획득
        int randomIndex = Random.Range(0, dropItemList.Count);

        // 실제로 얻은 재료 시각화 하는 부분 start
        GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
        ingredient.transform.position = transform.position;
        ingredient.GetComponent<DropIngredientController>()._ingredient = dropItemList[randomIndex];
        // end

        Debug.Log(dropItemList[randomIndex]._ingredientName);
        // 플레이어 없어져도 gameManager에 재료 정보 저장
        // 추후에 다른 정보도 저장 ex gold, exp, score TODO
        gameManager.ingredientDataList = _enemy.GetComponent<PlayerController>()._ingredientDataList;
    }
    // 몬스터가 죽은 후 플레이어에게 재료가 갈지 말지 정하는 함수
    protected void DropMainIngredient()
    {
        List<IngredientInfo> dropItemList = new List<IngredientInfo>();
        for (int i = 0; i < _dropMainIngredientList.Count; i++)
        {
            float randNumber = Random.Range(1f, 100f);
            if (randNumber <= _mainIngredientProbabilityList[i])
            {
                dropItemList.Add(_dropMainIngredientList[i]);
            }
        }
        // 얻은 아이템 없으면 함수 종료
        if (dropItemList.Count == 0)
            return;
        // 얻은 아이템중 하나만 획득
        int randomIndex = Random.Range(0, dropItemList.Count);

        // 실제로 얻은 재료 시각화 하는 부분 start
        GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
        ingredient.transform.position = transform.position;
        ingredient.GetComponent<DropIngredientController>()._ingredient = dropItemList[randomIndex];
        // end

        Debug.Log(dropItemList[randomIndex]._ingredientName);
        // 플레이어 없어져도 gameManager에 재료 정보 저장
        // 추후에 다른 정보도 저장 ex gold, exp, score TODO
        gameManager.ingredientDataList = _enemy.GetComponent<PlayerController>()._ingredientDataList;
    }
    // 몬스터가 죽은 후 플레이어에게 재료가 갈지 말지 정하는 함수
    protected void DropSpecialIngredient()
    {
        List<IngredientInfo> dropItemList = new List<IngredientInfo>();
        for (int i = 0; i < _dropSpecialIngredientList.Count; i++)
        {
            float randNumber = Random.Range(1f, 100f);
            if (randNumber <= _specialIngredientProbabilityList[i])
            {
                dropItemList.Add(_dropSpecialIngredientList[i]);
            }
        }
        // 얻은 아이템 없으면 함수 종료
        if (dropItemList.Count == 0)
            return;
        // 얻은 아이템중 하나만 획득
        int randomIndex = Random.Range(0, dropItemList.Count);

        // 실제로 얻은 재료 시각화 하는 부분 start
        GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
        ingredient.transform.position = transform.position;
        ingredient.GetComponent<DropIngredientController>()._ingredient = dropItemList[randomIndex];
        // end

        Debug.Log(dropItemList[randomIndex]._ingredientName);
        // 플레이어 없어져도 gameManager에 재료 정보 저장
        // 추후에 다른 정보도 저장 ex gold, exp, score TODO
        gameManager.ingredientDataList = _enemy.GetComponent<PlayerController>()._ingredientDataList;
    }
    // 일정 시간 후 몬스터의 시체 치우는 함수
    public float _setOffDelay = 1.0f;
    IEnumerator CoSetOffCorpse()
    {
        yield return new WaitForSecondsRealtime(_setOffDelay);
        // 죽은 몬스터가 보스 몬스터면 포탈 생성
        if (monsterId == 2000 || monsterId == 2001 || monsterId == 2002)
        {
            GameObject portal = Util.Instantiate("CommonPortal");
            portal.transform.position = transform.position + new Vector3(0f, 1.1f, 0);
            switch (monsterId)
            {
                case 2000:
                    portal.GetComponent<CommonPortalController>()._stageType = Define.StageType.HumanWorld;
                    break;
                case 2001:
                    portal.GetComponent<CommonPortalController>()._stageType = Define.StageType.Heaven;
                    break;
                case 2002:
                    portal.GetComponent<CommonPortalController>()._stageType = Define.StageType.Heaven;
                    break;
            }
        }
        // 이 함수가 실행 될 때마다 뒤에 것들이 실행이 안됨.
        gameObject.SetActive(false);
        _enemy.GetComponent<PlayerController>().CheckCanLevelUp();
    }
    [SerializeField]
    // 충돌 데미지 딜레이
    protected float _collisionDelay = 2f;
    float _collisionTimer = 0f;
    [SerializeField]
    protected bool _wantCollisionDamage = false;
    protected void CollisionDamage()
    {
        if (_wantCollisionDamage)
        {
            // 박스 충돌에 유저가 있을 때 공격
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + new Vector3(_attackRangeWidth, _attackRangeHeight), new Vector3(_attackRangeX, _attackRangeY, 0), 0);
            foreach (Collider2D collider in colliders)
            {
                if (gameObject.tag == "Monster" && collider.tag == "Player" && _collisionTimer <= 0)
                {
                    Util.SummonDamageViewer(collider.gameObject, _stat.CollisionAttack);
                    _enemy.GetComponent<PlayerController>().monsterHittedMe = this.gameObject;
                    _collisionTimer = _collisionDelay;
                }
                else
                {
                    _collisionTimer -= Time.deltaTime;
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(_attackRangeWidth, _attackRangeHeight), new Vector3(_attackRangeX, _attackRangeY, 0));
    }
}
