using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterController : CreatureController
{
    protected SoundManager soundManager;
    protected GameManager gameManager;
    // ������ ��ġ�� �÷��� ������Ʈ
    [SerializeField]
    protected GameObject _spawnedPlatform;
    protected Rigidbody2D _rb;
    protected Animator _animator;
    protected GameObject _enemy;
    protected GameObject _hpBarObject;
    protected HPSliderController _hpBar;
    // ���Ͱ� ����ϴ� �Ϲ� ����� ����Ʈ ���
    protected List<IngredientInfo> _dropCommonIngredientList = new List<IngredientInfo>();
    // ���Ͱ� ����ϴ� ���̷��� ����Ʈ ���
    protected List<IngredientInfo> _dropCondimentIngredientList = new List<IngredientInfo>();
    // ���Ͱ� ����ϴ� ���� ����� ����Ʈ ���
    protected List<IngredientInfo> _dropMainIngredientList = new List<IngredientInfo>();
    // ���Ͱ� ����ϴ� ����� ����� ����Ʈ ���
    protected List<IngredientInfo> _dropSpecialIngredientList = new List<IngredientInfo>();

    // ���Ͱ� ����ϴ� �Ϲ� ����� Ȯ��
    protected List<float> _commonIngredientProbabilityList = new List<float>();
    // ���Ͱ� ����ϴ� ���̷��� Ȯ��
    protected List<float> _condimentIngredientProbabilityList = new List<float>();
    // ���Ͱ� ����ϴ� ���� ����� Ȯ��
    protected List<float> _mainIngredientProbabilityList = new List<float>();
    // ���Ͱ� ����ϴ� Ư�� ����� Ȯ��
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
    // ���Ͱ� chasing �ϱ��� ��ǥ��
    Vector3 _originalPos;
    // ���Ͱ� chasing �ϴ� ��ǥ���� ��ǥ��
    Vector3 _destPos;
    [SerializeField]
    // �߻�ü ��� ������
    private float _ratingDelay = 1f;
    // ���͵��� ���� ���� ���� X�� (���� Running�� ������)
    [SerializeField]
    private float _attackRangeX = 1.0f;
    // ���͵��� ���� ���� ���� Y�� (���� Running�� ������)
    [SerializeField]
    private float _attackRangeY = 1.0f;
    // ���͵��� ���� ���� ���� Height �̵� �� (���� Running�� ������)
    [SerializeField]
    private float _attackRangeHeight = 1.0f;
    // ���͵��� ���� ���� ���� Height �̵� �� (���� Running�� ������)
    [SerializeField]
    private float _attackRangeWidth = 1.0f;
    [SerializeField]
    // ���� ũ�� �� ����
    protected float _monsterScale = 1.0f;
    [SerializeField]
    // ���� ��������Ʈ ������ ��� true���ֱ�
    protected bool _isReverse = true;
    [SerializeField]
    // hp bar�� x scale
    protected float _hpBarScaleX = 0.01f;
    [SerializeField]
    // hp bar�� y scale
    protected float _hpBarScaleY = 0.01f;
    [SerializeField]
    // hb bar�� ���� ��ġ ����
    protected float _hpBarHeight = 0.5f;
    [SerializeField]
    // hb bar�� ���� ��ġ ����
    protected float _hpBarWeight = 0.0f;
    [SerializeField]
    protected int _projectileNumber;
    [SerializeField]
    protected float _projectileTimer = 2f;
    [SerializeField]
    // �߻�ü �ε���
    protected int _projectileSpriteIndex = 0;
    [SerializeField]
    // �߻�ü ������
    protected int _projectileDamage = 0;
    [SerializeField]
    // �߻�ü ������
    protected float _rateSpeed = 1f;
    // ����ü�� �밢������ ������ ��
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
    // ���̾� �������ִ� �Լ�
    protected void SetMonsterLayer()
    {
        gameObject.layer = 7; // ���� ���̾�
        // �ڽĵ� ���� ���̾� ����
        if (gameObject.transform.childCount != 0 && gameObject.transform.GetChild(0).gameObject.layer == 0)
            gameObject.transform.GetChild(0).gameObject.layer = 7; // ���� ���̾�
    }
    // �÷��� ������ ���� �Ʒ��� Ray�� ���� spawnPlatform ������Ʈ ����
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

        //// ���߿� �߱� ����
        _rb.gravityScale = 0;
        _monsterScanState = Define.MonsterScanState.Patrol;
        _monsterState = Define.MonsterState.Fly;
        _dir = Define.MoveDir.Right;
    }
    // ������Ʈ���� ��������
    protected void FollowPlayer()
    {
        // ���� �ʾҰ� ���󰡴� �÷��� ���
        if (!isDead && _canFollow)
        {
            // ���� ���� ���� ����
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
        // patrol ��
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
        // ���� �������� ��
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
                        // ��Ʈ�� �̵�
                        transform.position += new Vector3(Vector2.left.x * _stat.MaxSpeed * Time.deltaTime, 0, 0);
                        // �÷��� ���� ����� �� ���
                        Debug.DrawRay(gameObject.transform.position + new Vector3(-_patrolRayLength, 0, 0), Vector3.down, Color.red);
                        RaycastHit2D hitLeft = Physics2D.Raycast(gameObject.transform.position + new Vector3(-_patrolRayLength, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));

                        if (hitLeft.collider != null)
                            //Debug.Log(hitLeft.collider.name);

                        // �÷��� ray ���� ��Ȳ ó�� �ϴ� ��
                        if (hitLeft.collider != null && (hitLeft.collider.tag == "Switch" || hitLeft.collider.tag == "Monster"))
                                break;

                        // �÷��� �� ������ ���� ��ȯ
                        if (hitLeft.collider == null ||
                            hitLeft.collider.name != _spawnedPlatform.name)
                        {
                            _dir = Define.MoveDir.Left;
                        }
                        break;
                    case Define.MoveDir.Left:
                        // ��Ʈ�� �̵�
                        transform.position += new Vector3(Vector2.right.x * _stat.MaxSpeed * Time.deltaTime, 0, 0);
                        // �÷��� ���� ����� �� ���
                        Debug.DrawRay(gameObject.transform.position + new Vector3(_patrolRayLength, 0, 0), Vector3.down, Color.red);
                        RaycastHit2D hitRight = Physics2D.Raycast(gameObject.transform.position + new Vector3(_patrolRayLength, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));

                        if (hitRight.collider != null)
                            //Debug.Log(hitRight.collider.name);

                        // �÷��� ray ���� ��Ȳ ó�� �ϴ� ��
                        if (hitRight.collider != null && (hitRight.collider.tag == "Switch" || hitRight.collider.tag == "Monster"))
                                break;

                        // �÷��� �� ������ ���� ��ȯ
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
                        // ��Ʈ�� �̵�
                        transform.position += new Vector3(Vector2.left.x * _stat.MaxSpeed * Time.deltaTime, 0, 0);
                        // �÷��� ���� ����� �� ���
                        Debug.DrawRay(gameObject.transform.position + new Vector3(-1, 0, 0), Vector3.down, Color.red);
                        RaycastHit2D hitLeft = Physics2D.Raycast(gameObject.transform.position + new Vector3(-1, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));

                        if (hitLeft.collider != null)
                            //Debug.Log(hitLeft.collider.tag);

                            // �÷��� ray ���� ��Ȳ ó�� �ϴ� ��
                            if (hitLeft.collider != null && (hitLeft.collider.tag == "Switch" || hitLeft.collider.tag == "Monster"))
                                break;

                        // �÷��� �� ������ ���� ��ȯ
                        if (hitLeft.collider == null ||
                            hitLeft.collider.name != _spawnedPlatform.name)
                        {
                            _dir = Define.MoveDir.Right;
                        }
                        break;
                    case Define.MoveDir.Right:
                        // ��Ʈ�� �̵�
                        transform.position += new Vector3(Vector2.right.x * _stat.MaxSpeed * Time.deltaTime, 0, 0);
                        // �÷��� ���� ����� �� ���
                        Debug.DrawRay(gameObject.transform.position + new Vector3(1, 0, 0), Vector3.down, Color.red);
                        RaycastHit2D hitRight = Physics2D.Raycast(gameObject.transform.position + new Vector3(1, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));

                        if (hitRight.collider != null)
                            //Debug.Log(hitRight.collider.tag);

                            // �÷��� ray ���� ��Ȳ ó�� �ϴ� ��
                            if (hitRight.collider != null && (hitRight.collider.tag == "Switch" || hitRight.collider.tag == "Monster"))
                                break;

                        // �÷��� �� ������ ���� ��ȯ
                        if (hitRight.collider == null ||
                            hitRight.collider.name != _spawnedPlatform.name)
                        {
                            _dir = Define.MoveDir.Left;
                        }
                        break;
                }
            }
            // �÷��̾� ����
            if (_enemy != null && _enemy.GetComponent<PlayerController>().CurSteppedPlatform != null)
            {
                if (_spawnedPlatform.name == _enemy.GetComponent<PlayerController>().CurSteppedPlatform.name)
                    _monsterScanState = Define.MonsterScanState.Chase;
            }

        }
    }   
    protected void PatrolFixed()
    {
        // �÷��̾� ����
        if (_enemy.GetComponent<PlayerController>().CurSteppedPlatform != null)
        {
            if (_spawnedPlatform.name == _enemy.GetComponent<PlayerController>().CurSteppedPlatform.name)
                _monsterScanState = Define.MonsterScanState.Chase;
        }
    }
    protected void PatrolFlying()
    {
        // �÷��̾� ��ĵ
        // ������ y ���� �� ũ�� ����
        if (transform.position.y <= _enemy.transform.position.y)
            _monsterScanState = Define.MonsterScanState.Chase;
    }

    public float _curTime = 0f;
    protected void FightRunning()
    {
        // ���� ���� ���� ����
        _originalPos = transform.position;
        _destPos = _enemy.transform.position;
        _destPos = new Vector3(_destPos.x, _originalPos.y, 0);
        transform.position = Vector2.MoveTowards(_originalPos, _destPos, _stat.MaxSpeed * Time.deltaTime);
        if (_curTime <= 0 && _canCommonAttack)
        {
            _canCommonAttack = false;
            // ��ó�� ������ ���� �� ����
            Collider2D[] collidersLeft = Physics2D.OverlapBoxAll(transform.position + new Vector3(_attackRangeWidth, _attackRangeHeight), new Vector3(_attackRangeX, _attackRangeY, 0), 0);
            foreach (Collider2D collider in collidersLeft)
            {
                if (collider.tag == "Player")
                {
                    // ���� ���� ��ǿ� ���缭 �ѹ��� SummonDamageViewer �ϱ�
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
                    // attack ��� �κ�
                    _monsterState = Define.MonsterState.Attack;
                    break;
                }
            }
            StartCoroutine("CoChangeStateDelay");
            // ��Ÿ�� �ʱ�ȭ �κ�
            _curTime = _commonAttackDelay;
        }
        else
        {
            //_monsterState = Define.MonsterState.Run;
            _curTime -= Time.deltaTime;
        }

        // �÷��� ���� ����� �� ���
        Debug.DrawRay(gameObject.transform.position + new Vector3(-1, 0, 0), Vector3.down, Color.red);
        RaycastHit2D hitLeft = Physics2D.Raycast(gameObject.transform.position + new Vector3(-1, 0, 0), Vector3.down, LayerMask.GetMask("Platform"));


        // ������ �ٸ� �÷��� ����� �� �ٽ� patrol�� ���ư��� chase ����
        if (_spawnedPlatform.gameObject.name != _enemy.GetComponent<PlayerController>().CurSteppedPlatform.name)
        {
            _monsterScanState = Define.MonsterScanState.Patrol;
            _monsterState = Define.MonsterState.Run;
        }
    }
    // ATTACK ��ǿ��� RUN���� ���ƿ��� ������ �ڷ�ƾ �Լ�
    [SerializeField]
    public float _changeStateDelay = 0.7f;
    // �Ϲ� ���� ������
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
        // ���� ���� ���� ����
        // �������� �ʿ� ����
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
            //// ���� ���� ��ǿ� ���缭 �ѹ��� SummonDamageViewer �ϱ�
            //if (_isCommonAttackDelay)
            //    StartCoroutine(Util.CoSummonDamageViewer(_enemy, _stat.Attack, _commonAttackDelayForCo));
            //else
            //    Util.SummonDamageViewer(_enemy, _stat.Attack);
            // attack ��� �κ�
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
    // �� ������ ������ �Դ� Ÿ�̹� ���� ����
    protected float _commonAttackDelayForCo = 0.3f;
    protected void FightFlying()
    {
        // ���� ���� ���� ����
        //_monsterState = Define.MonsterState.Attack;
        _originalPos = transform.position;
        _destPos = _enemy.transform.position;
        transform.position = Vector2.MoveTowards(_originalPos, _destPos + new Vector3(0, anchorHeight, 0), _stat.MaxSpeed * Time.deltaTime);

        // ��ó�� ������ ���� �� ����
        if (_curTime <= 0 && _canCommonAttack)
        {
            _canCommonAttack = false;
            // ��ó�� ������ ���� �� ����
            Collider2D[] collidersLeft = Physics2D.OverlapBoxAll(transform.position + new Vector3(_attackRangeWidth, _attackRangeHeight), new Vector3(_attackRangeX, _attackRangeY, 0), 0);
            foreach (Collider2D collider in collidersLeft)
            {
                if (collider.tag == "Player")
                {
                    // ���� ���� ��ǿ� ���缭 �ѹ��� SummonDamageViewer �ϱ�
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
                    // attack ��� �κ�
                    _monsterState = Define.MonsterState.Attack;
                    break;
                }
            }
            StartCoroutine("CoChangeStateDelay");
            // ��Ÿ�� �ʱ�ȭ �κ�
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
        //// MonsterScanState�� �߰����� �� ���͵��� ��������Ʈ�� ������ ���⿡ �°� ����
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
            // ����ġ ȹ��
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
    // id = ��� id , probability = Ȯ��
    protected void MakeCommonIngredient(int[] id, float[] probability)
    {
        for(int i = 0; i < id.Length; i++)
        {
            _dropCommonIngredientList.Add(IngredientData.data[id[i]]);
            _commonIngredientProbabilityList.Add(probability[i]);
        }
    }
    // id = ��� id , probability = Ȯ��
    protected void MakeCondimentIngredient(int[] id, float[] probability)
    {
        for (int i = 0; i < id.Length; i++)
        {
            _dropCondimentIngredientList.Add(IngredientData.data[id[i]]);
            _condimentIngredientProbabilityList.Add(probability[i]);
        }
    }
    // id = ��� id , probability = Ȯ��
    protected void MakeMainIngredient(int[] id, float[] probability)
    {
        for (int i = 0; i < id.Length; i++)
        {
            _dropMainIngredientList.Add(IngredientData.data[id[i]]);
            _mainIngredientProbabilityList.Add(probability[i]);
        }
    }
    // id = ��� id , probability = Ȯ��
    protected void MakeSpecialIngredient(int[] id, float[] probability)
    {
        for (int i = 0; i < id.Length; i++)
        {
            _dropSpecialIngredientList.Add(IngredientData.data[id[i]]);
            _specialIngredientProbabilityList.Add(probability[i]);
        }
    }

    // ��� ȹ�� �Լ�
    protected void DropGold()
    {
        GameObject gold = Util.Instantiate("DropItems/Gold");
        gold.transform.position = transform.position;
        gold.GetComponent<DropGoldController>()._gold = _gold;
    }
    //// ��� ȹ�� �Լ�
    //protected void DropIngredientObject()
    //{
    //    GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
    //    ingredient.transform.position = transform.position;
    //}
    // ���Ͱ� ���� �� �÷��̾�� ��ᰡ ���� ���� ���ϴ� �Լ�
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
        // ���� ������ ������ �Լ� ����
        if (dropItemList.Count == 0)
            return;
        // ���� �������� �ϳ��� ȹ��
        int randomIndex = Random.Range(0, dropItemList.Count);

        // ������ ���� ��� �ð�ȭ �ϴ� �κ� start
        GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
        ingredient.transform.position = transform.position;
        ingredient.GetComponent<DropIngredientController>()._ingredient = dropItemList[randomIndex];
        // end

        Debug.Log(dropItemList[randomIndex]._ingredientName);
        // �÷��̾� �������� gameManager�� ��� ���� ����
        // ���Ŀ� �ٸ� ������ ���� ex gold, exp, score TODO
        gameManager.ingredientDataList = _enemy.GetComponent<PlayerController>()._ingredientDataList;
    }
    // ���Ͱ� ���� �� �÷��̾�� ��ᰡ ���� ���� ���ϴ� �Լ�
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
        // ���� ������ ������ �Լ� ����
        if (dropItemList.Count == 0)
            return;
        // ���� �������� �ϳ��� ȹ��
        int randomIndex = Random.Range(0, dropItemList.Count);

        // ������ ���� ��� �ð�ȭ �ϴ� �κ� start
        GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
        ingredient.transform.position = transform.position;
        ingredient.GetComponent<DropIngredientController>()._ingredient = dropItemList[randomIndex];
        // end

        Debug.Log(dropItemList[randomIndex]._ingredientName);
        // �÷��̾� �������� gameManager�� ��� ���� ����
        // ���Ŀ� �ٸ� ������ ���� ex gold, exp, score TODO
        gameManager.ingredientDataList = _enemy.GetComponent<PlayerController>()._ingredientDataList;
    }
    // ���Ͱ� ���� �� �÷��̾�� ��ᰡ ���� ���� ���ϴ� �Լ�
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
        // ���� ������ ������ �Լ� ����
        if (dropItemList.Count == 0)
            return;
        // ���� �������� �ϳ��� ȹ��
        int randomIndex = Random.Range(0, dropItemList.Count);

        // ������ ���� ��� �ð�ȭ �ϴ� �κ� start
        GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
        ingredient.transform.position = transform.position;
        ingredient.GetComponent<DropIngredientController>()._ingredient = dropItemList[randomIndex];
        // end

        Debug.Log(dropItemList[randomIndex]._ingredientName);
        // �÷��̾� �������� gameManager�� ��� ���� ����
        // ���Ŀ� �ٸ� ������ ���� ex gold, exp, score TODO
        gameManager.ingredientDataList = _enemy.GetComponent<PlayerController>()._ingredientDataList;
    }
    // ���Ͱ� ���� �� �÷��̾�� ��ᰡ ���� ���� ���ϴ� �Լ�
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
        // ���� ������ ������ �Լ� ����
        if (dropItemList.Count == 0)
            return;
        // ���� �������� �ϳ��� ȹ��
        int randomIndex = Random.Range(0, dropItemList.Count);

        // ������ ���� ��� �ð�ȭ �ϴ� �κ� start
        GameObject ingredient = Util.Instantiate("DropItems/Ingredient");
        ingredient.transform.position = transform.position;
        ingredient.GetComponent<DropIngredientController>()._ingredient = dropItemList[randomIndex];
        // end

        Debug.Log(dropItemList[randomIndex]._ingredientName);
        // �÷��̾� �������� gameManager�� ��� ���� ����
        // ���Ŀ� �ٸ� ������ ���� ex gold, exp, score TODO
        gameManager.ingredientDataList = _enemy.GetComponent<PlayerController>()._ingredientDataList;
    }
    // ���� �ð� �� ������ ��ü ġ��� �Լ�
    public float _setOffDelay = 1.0f;
    IEnumerator CoSetOffCorpse()
    {
        yield return new WaitForSecondsRealtime(_setOffDelay);
        // ���� ���Ͱ� ���� ���͸� ��Ż ����
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
        // �� �Լ��� ���� �� ������ �ڿ� �͵��� ������ �ȵ�.
        gameObject.SetActive(false);
        _enemy.GetComponent<PlayerController>().CheckCanLevelUp();
    }
    [SerializeField]
    // �浹 ������ ������
    protected float _collisionDelay = 2f;
    float _collisionTimer = 0f;
    [SerializeField]
    protected bool _wantCollisionDamage = false;
    protected void CollisionDamage()
    {
        if (_wantCollisionDamage)
        {
            // �ڽ� �浹�� ������ ���� �� ����
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
