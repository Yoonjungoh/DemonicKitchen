using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : CreatureController
{
    [SerializeField]
    public GameObject UI_GameController;
    [SerializeField]
    private GameObject _levelUpRewardPanel;
    [SerializeField]
    private GameObject deathPanel;
    GameManager gameManager;
    [SerializeField]
    GameObject _curSteppedPlatform;
    GameObject _weapons;
    GameObject _deathWall;
    [SerializeField]
    PhysicsMaterial2D frictionless;
    [SerializeField]
    PhysicsMaterial2D slope;
    public GameObject CurSteppedPlatform { get { return _curSteppedPlatform; } }
    Rigidbody2D _rb;
    public Rigidbody2D Rb { get { return _rb; } }
    Animator _animator;

    [SerializeField]
    // 플레이어 레벨
    private int _level = 1;
    public int Level { get { return _level; } set { _level = value; } }

    [SerializeField]
    // 플레이어 경험치
    private int _totalExp = 0;
    public int TotalExp { get { return _totalExp; } set { _totalExp = value; } }
    [SerializeField]
    // 플레이어 골드
    private int _totalGold = 0;
    public int TotalGold { get { return _totalGold; } set { _totalGold = value; } }

    [SerializeField]
    // 플레이어가 획득한 재료 수   
    private int _totalIngredient = 0;
    public int TotalIngredient { get { return _totalIngredient; } set { _totalIngredient = value; } }
    public List<IngredientInfo> _ingredientDataList = new List<IngredientInfo>();

    // 플레이어의 최고 높이
    public float _highestHeight;
    public float HighestHeight { get { return _highestHeight; } }
    // 총 최고 높이 = 점수
    public float _totalHighestHeight;
    public float TotalHighestHeight { get { return _totalHighestHeight; } }

    [SerializeField]
    // 땅에 닿았나 확인
    private bool _isGrounded = false;
    // 충돌중인가 확인
    bool _isCollising = false;
    private GameObject contactPlatform;
    private Vector3? platformPosition;
    private Vector3? distance;

    [SerializeField]
    private bool _isOnMovingPlatform = false;
    public bool IsOnMovingPlatform { get { return _isOnMovingPlatform; } set { _isOnMovingPlatform = value; } }

    [SerializeField]
    private bool _canJump = false;
    public bool CanJump { get { return _canJump; } }
    // 터치 버튼 점프 횟수
    public int _touchJump = 0;

    bool _canMoveRight = false;
    bool _canMoveLeft = false;

    [SerializeField]
    // 공격 관련 플래그
    bool _canAttack = true;
    public bool CanAttack { get { return _canAttack; } }
    private float _curTime = 0f;
    // 공격 시간 딜레이
    [SerializeField]
    private float _attackDelay = 0.25f;
    // 공격 모션 딜레이 (모션 끝나는 시간에 맞춰서 수작업으로 설정중)
    [SerializeField]
    float _changeStateDelay = 0.6f;

    // 미끄러운 지역에서 활성화 TODO
    bool _isSlippery = false;
    // 미끄러움에 저항하지 못하는 정도. 클 수록 더 미끄러워짐.
    [SerializeField]
    private float _moveResistance = 0.5f;
    // hp bar 부분
    GameObject _hpBarObject;
    [SerializeField]
    float _hpBarWeight = 0f;
    [SerializeField]
    float _hpBarHeight = 0f;
    [SerializeField]
    float _hpBarScaleX = 0.01f;
    [SerializeField]
    float _hpBarScaleY = 0.01f;

    public float _playerScale = 1f;

    [SerializeField]
    Define.PlayerState _playerState = Define.PlayerState.Idle;
    [SerializeField]
    Define.MoveDir _dir = Define.MoveDir.Right;
    public Define.MoveDir Dir { get { return _dir; } }
    
    [SerializeField]
    GameObject _weapon;
    public GameObject Weapon { get { return _weapon; } }

    GameObject _buttonPanel;
    public GameObject _backGroundCanvas;
    // 유저가 현재 들고 있는 무기의 ID
    public List<int> _weaponIDList = new List<int>();
    // 상점에서 사온 스탯 적용하는 메니져
    public Stat statManager;

    SoundManager soundManager;
    
    // 벽에 붙어서 점프하면 무한으로 돼서 막아놓기
    public bool _isTouchingWall = false;
    public bool _canUsePortal = false;
    // 갖고 있는 특수 능력
    public List<Define.SpecialAbility> _specialAbilityList = new List<Define.SpecialAbility>();
    public bool isResurrection = false;
    public GameObject monsterHittedMe;
    void Awake()
    {
        base.Init();
        _creatureType = Define.CreatureType.Player;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _highestHeight = (float)transform.position.y;
        _totalHighestHeight = _highestHeight;
        _curTime = _attackDelay;
        _weapons = transform.Find("Weapons").gameObject;
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
        SetHPBar();
        SetButtonPanel();
        SetBackGroundImage();
        SetStageProgress();
        // 기본 무기 클레버 장착
        _weaponIDList.Add(201);
        SetWeapon();
        SetStat();
        SetSpecialAbillity();
        deathPanel = UI_GameController.transform.Find("DeathPanel").gameObject;
    }
    void Update()
    {
        MovePlayer();
        JumpPlayer();
        UpdateHighestHeight();
        PullDropItems();

        UpdateAnimation();

        // 게임중 뒤로가기 버튼 활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI_GameController.GetComponent<UI_GameController>().ActiveCheckPanel();
        }
        //Debug.Log(_rb.velocity.y);
    }
    // 상점 레시피 정보 가져와서 _specialAbilityList 정보 추가
    public void SetSpecialAbillity()
    {
        _specialAbilityList = GameManager.SpecialAbilityList;
        //foreach (Define.SpecialAbility ability in _specialAbilityList)
        //    Debug.Log($"{ability} 능력이 해금됐습니다");
    }
    // 상점 스탯과 연동
    public void SetStat()
    {
        statManager = GameObject.Find("@StatManager").GetComponent<Stat>();
        if (statManager != null) 
        {
            transform.GetComponent<Stat>()._maxHp += statManager._maxHp;
            transform.GetComponent<Stat>()._hp += statManager._hp;
            transform.GetComponent<Stat>()._attack += statManager._attack;
            transform.GetComponent<Stat>()._defense += statManager._defense;
            transform.GetComponent<Stat>()._maxSpeed += statManager._maxSpeed;
        }
    }
    public void SetButtonPanel()
    {
        // UI_GameController 셋팅 하고 buttonPanel에 event trigger 셋팅
        if (GameObject.Find("UI_GameController") == null)
        {
            UI_GameController = Util.Instantiate("UI/UI_GameController");
            DontDestroyOnLoad(UI_GameController);
        }

        _buttonPanel = UI_GameController.transform.Find("ButtonPanel").gameObject;

        GameObject leftButton = _buttonPanel.transform.GetChild(0).gameObject;

        EventTrigger eventTriggerLeft = leftButton.GetComponent<EventTrigger>();

        EventTrigger.Entry entryPointerLeftDown = new EventTrigger.Entry();
        entryPointerLeftDown.eventID = EventTriggerType.PointerDown;
        entryPointerLeftDown.callback.AddListener((data) => { PermitMoveLeft(); });
        eventTriggerLeft.triggers.Add(entryPointerLeftDown);

        EventTrigger.Entry entryPointerLeftUp = new EventTrigger.Entry();
        entryPointerLeftUp.eventID = EventTriggerType.PointerUp;
        entryPointerLeftUp.callback.AddListener((data) => { BanMoveLeft(); });
        eventTriggerLeft.triggers.Add(entryPointerLeftUp);


        GameObject rightButton = _buttonPanel.transform.GetChild(1).gameObject;

        EventTrigger eventTriggerRight = rightButton.GetComponent<EventTrigger>();

        EventTrigger.Entry entryPointerRightDown = new EventTrigger.Entry();
        entryPointerRightDown.eventID = EventTriggerType.PointerDown;
        entryPointerRightDown.callback.AddListener((data) => { PermitMoveRight(); });
        eventTriggerRight.triggers.Add(entryPointerRightDown);

        EventTrigger.Entry entryPointerRightUp = new EventTrigger.Entry();
        entryPointerRightUp.eventID = EventTriggerType.PointerUp;
        entryPointerRightUp.callback.AddListener((data) => { BanMoveRight(); });
        eventTriggerRight.triggers.Add(entryPointerRightUp);


        GameObject jumpButton = _buttonPanel.transform.GetChild(2).gameObject;

        EventTrigger eventTriggerJump = jumpButton.GetComponent<EventTrigger>();

        EventTrigger.Entry entryPointerJumpDown = new EventTrigger.Entry();
        entryPointerJumpDown.eventID = EventTriggerType.PointerDown;
        entryPointerJumpDown.callback.AddListener((data) => { PermitJump(); });
        eventTriggerJump.triggers.Add(entryPointerJumpDown);

        EventTrigger.Entry entryPointerJumpUp = new EventTrigger.Entry();
        entryPointerJumpUp.eventID = EventTriggerType.PointerUp;
        entryPointerJumpUp.callback.AddListener((data) => { BanJump(); });
        eventTriggerJump.triggers.Add(entryPointerJumpUp);

        //GameObject portalButton = _buttonPanel.transform.GetChild(3).gameObject;

        //EventTrigger eventTriggerPortal = portalButton.GetComponent<EventTrigger>();

        //EventTrigger.Entry entryPointerPortalDown = new EventTrigger.Entry();
        //entryPointerPortalDown.eventID = EventTriggerType.PointerDown;
        //entryPointerPortalDown.callback.AddListener((data) => { PermitPortal(); });
        //eventTriggerPortal.triggers.Add(entryPointerPortalDown);

        //EventTrigger.Entry entryPointerPortalUp = new EventTrigger.Entry();
        //entryPointerPortalUp.eventID = EventTriggerType.PointerUp;
        //entryPointerPortalUp.callback.AddListener((data) => { BanPortal(); });
        //eventTriggerPortal.triggers.Add(entryPointerPortalUp);
    }
    // Game에서 Stage 바뀔때마다 갱신
    public void SetBackGroundImage()
    {
        if (GameObject.Find("BackGroundCanvas") == null) 
        {
            _backGroundCanvas = Util.Instantiate("UI/BackGroundCanvas");
            // 초기 배경 화면은 마계로 설정
            DontDestroyOnLoad(_backGroundCanvas);
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                if (gameManager.stageType == Define.StageType.Devildom)
                {
                    if (i == 0) 
                    {
                        _backGroundCanvas.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        _backGroundCanvas.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else if (gameManager.stageType == Define.StageType.HumanWorld)
                {
                    if (i == 1)
                    {
                        _backGroundCanvas.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        _backGroundCanvas.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else if (gameManager.stageType == Define.StageType.Heaven)
                {
                    if (i == 2)
                    {
                        _backGroundCanvas.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        _backGroundCanvas.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    void SetStageProgress()
    {
        // gameManager 에게 stage progress 넘겨주기
        gameManager.stageProgress = UI_GameController.transform.Find("StageProgress").GetComponent<StageProgressController>();
    }
    // 장착 무기 최신화 함수
    public void SetWeapon()
    {
        for (int i = 0; i < _weapons.transform.childCount; i++)
        {
            // 오브젝트 tag가 Weapon 이고 _weaponIDList에 무기 id 가 추가 돼 있을때만 장착
            if (_weapons.transform.GetChild(i).tag == "Weapon" && _weaponIDList.Contains(_weapons.transform.GetChild(i).GetComponent<WeaponController>()._id))
            {
                // tag가 weapon이면 전부 작동
                GameObject weapon = _weapons.transform.GetChild(i).gameObject;
                weapon.SetActive(true);
            }
            // 테스트용 나중에 없애기 TODO
            else if(_weapons.transform.GetChild(i).tag == "Weapon")
            {
                GameObject weapon = _weapons.transform.GetChild(i).gameObject;
                weapon.SetActive(false);
            }
        }
    }
    protected void SetHPBar()
    {
        GameObject hpBarSlider = Resources.Load("Prefabs/UI/WorldSpace/HPBar") as GameObject;
        base._hpBarSlider = hpBarSlider.GetComponentInChildren<HPSliderController>();
        Vector3 instantiatePos = new Vector3(transform.position.x + _hpBarWeight,
            transform.position.y + _hpBarHeight,
            0);
        _hpBarObject = Instantiate(hpBarSlider, instantiatePos, Quaternion.identity, transform);
        _hpBarObject.transform.localScale = new Vector3(_hpBarScaleX, _hpBarScaleY, 1);
    }
    public void PermitMoveRight()
    {
        //Debug.Log("right");
        _canMoveRight = true;
    }
    public void PermitMoveLeft()
    {
        //Debug.Log("left");
        _canMoveLeft = true;
    }
    public void PermitJump()
    {
        //Debug.Log("jump");
        _canJump = true;
        _touchJump++;
    }
    public void PermitAttack()
    {
        ////Debug.Log("attack");
        //// 예전 공격 로직(공격 버튼 누를시 공격함)
        ////_canAttack = true;
        //if (_weapon.activeInHierarchy)
        //{
        //    _weapon.SetActive(false);
        //    _canAttack = false;
        //}
        //else
        //{
        //    _weapon.SetActive(true);
        //    _canAttack = false;
        //}
    }
    public void PermitPortal()
    {
        _canUsePortal = true;
    }
    public void BanMoveRight()
    {
        _canMoveRight = false;
        _rb.velocity = new Vector2(_rb.velocity.normalized.x * _moveResistance, _rb.velocity.y);
        // 멈춘 상태 표시. IDLE로 먼저 해줘야 None이 적용됨.
        _playerState = Define.PlayerState.Idle;
    }
    public void BanMoveLeft()
    {
        _canMoveLeft = false;
        _rb.velocity = new Vector2(_rb.velocity.normalized.x * _moveResistance, _rb.velocity.y);
        // 멈춘 상태 표시. IDLE로 먼저 해줘야 None이 적용됨.
        _playerState = Define.PlayerState.Idle;
    }
    public void BanJump()
    {
        _canJump = false;
    }
    public void BanAttack()
    {
        // 형식만 갖춤
        //_canAttack = false;
    }
    public void BanPortal()
    {
        _canUsePortal = false;
    }
    public void MovePlayer()
    {
        // 오른쪽 이동
        if (_canMoveRight)
        {
            _rb.AddForce(Vector2.right * (1), ForceMode2D.Impulse);
            _rb.velocity = new Vector2(_stat.MaxSpeed, _rb.velocity.y);
            _dir = Define.MoveDir.Right;
            _playerState = Define.PlayerState.Run;
        }
        // 왼쪽 이동
        if (_canMoveLeft)
        {
            _rb.AddForce(Vector2.right * (-1), ForceMode2D.Impulse);
            _rb.velocity = new Vector2(_stat.MaxSpeed * (-1), _rb.velocity.y);
            _dir = Define.MoveDir.Left;
            _playerState = Define.PlayerState.Run;
        }
    }
    public GameObject jumpCollision;
    public Vector2 checkBoxSize;
    public Vector3 checkBoxPos;
    public LayerMask isLayer;
    public void JumpPlayer()
    {
        _isGrounded = Physics2D.OverlapBox(jumpCollision.transform.position + checkBoxPos, checkBoxSize, isLayer);
        Collider2D[] groundedObjects = Physics2D.OverlapBoxAll(jumpCollision.transform.position + checkBoxPos, checkBoxSize, isLayer);

        foreach (Collider2D groundedObject in groundedObjects)
        {
            if (groundedObject.tag == "Platform")
                _curSteppedPlatform = groundedObject.gameObject;

            if (_isGrounded && _canJump && _rb.velocity.y == 0)
            {
                _rb.velocity = Vector2.up * _stat.JumpSpeed;
                _playerState = Define.PlayerState.Jump;
                soundManager.effect.PlayOneShot(soundManager.jumpEffect);
            }
            // sloping에서 점프하기 위한 용도
            else if (_isGrounded && _canJump &&
                groundedObject.GetComponent<PlatformController>() != null &&
                groundedObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Sloping)
            {
                _rb.velocity = Vector2.up * _stat.JumpSpeed;
                _playerState = Define.PlayerState.Jump;
                soundManager.effect.PlayOneShot(soundManager.jumpEffect);
                Debug.Log("jumping above sloping");
            }
        }
    }
    
    // 플레이어의 최고 높이 갱신하는 함수 TODO 호출 횟수 줄여서 최적화 가능
    void UpdateHighestHeight()
    {
        if (_highestHeight < (float)transform.position.y)
        {
            _highestHeight = (float)transform.position.y;
        }
    }
    // ATTACK 모션에서 IDLE로 돌아오는 딜레이 코루틴 함수
    IEnumerator CoChangeStateDelay()
    {
        yield return new WaitForSeconds(_changeStateDelay);
        _playerState = Define.PlayerState.Idle;
        _canAttack = false;
    }
    // 플레이어가 레벨업 가능한 상태인지 체크하는 함수
    // 몬스터가 죽을 때 마다 몬스터 쪽에서 호출해주기
    public void CheckCanLevelUp()
    {
        // 만렙 달성
        if (_level == LevelData.data.Count)
        {
            Debug.Log("만렙");
            return;
        }
        while (_totalExp >= LevelData.data[_level - 1])
        {
            _totalExp -= LevelData.data[_level - 1];
            LevelUp();
            StartCoroutine(CoSpawnLevelUpEffect());
        }
        //while (_totalExp >= LevelData.data[_level - 1])
        //{
        //    _totalExp -= LevelData.data[_level - 1];
        //    // 레벨업 되면 추가하는 부분
        //    LevelUp();
        //    StartCoroutine(CoSpawnLevelUpEffect());
        //}
    }
    IEnumerator CoSpawnLevelUpEffect()
    {
        GameObject levelUp = Util.Instantiate("LevelUp");
        levelUp.transform.position = transform.position;
        yield return new WaitForSeconds(0.5f);
        Destroy(levelUp);
    }
    // 레벨 업 시 스펙 추가 부분
    void LevelUp()
    {
        _level++;
        ShowRewardCards();
    }
    // 레벨업 보상 카드 보여주는 부분
    // 이부분은 뷰어 쪽에서 선택이 됐을 때 다시 소환하는 방식으로 바꾸기
    // 플레이어 측에선 한번만 부르고 뷰어쪽에서 나머지 소환하는 방식으로 바꾸기
    void ShowRewardCards()
    {
        GameObject UI = GameObject.FindWithTag("GameCanvas");
        //GameObject go = Util.Instantiate("UI/LevelUpRewardPanel", UI.transform);
        GameObject go = Util.Instantiate(_levelUpRewardPanel, UI.transform);
        go.SetActive(true);
    }
    // 애니메이션 추가할 때 마다 상태 정의 해주고  복붙해서 붙여주기만 하면 끝

    // 아이템 자석처럼 끌어들이는 함수
    public Vector3 _pullRangeOrigin;
    public float _pullRangeRadious;
    public float _pullSpeed = 0.5f;
    void PullDropItems()
    {
        Collider2D[] dropItmes = Physics2D.OverlapCircleAll(transform.position + _pullRangeOrigin, _pullRangeRadious);
        foreach (Collider2D dropItem in dropItmes)
        {
            if (dropItem.tag == "DropItem")
            {
                dropItem.transform.position = Vector2.MoveTowards(dropItem.transform.position, transform.position, _pullSpeed * Time.deltaTime);
            }
        }
    }

    public Vector3 _earthQuakeRangeOrigin;
    public Vector3 _earthQuakeSize;
    public int _earthQuakeDamage = 50;
    void CastingEarthQuake()
    {
        if (_specialAbilityList.Contains(Define.SpecialAbility.EarthQuake))
        {
            Shake();
            Collider2D[] enemies1 = Physics2D.OverlapBoxAll(transform.position + _earthQuakeRangeOrigin, _earthQuakeSize, 0f);
            foreach (Collider2D enemy in enemies1)
            {
                Debug.Log(enemy.name);
                if (enemy.tag == "Monster")
                {
                    StartCoroutine(Util.CoSummonDamageViewer(enemy.gameObject, _earthQuakeDamage));
                }
            }
            Debug.Log("Casting EarthQuake");
        }
        Shake();
        Collider2D[] enemies2 = Physics2D.OverlapBoxAll(transform.position + _earthQuakeRangeOrigin, _earthQuakeSize, 0f);
        foreach (Collider2D enemy in enemies2)
        {
            Debug.Log(enemy.name);
            if (enemy.tag == "Monster")
            {
                StartCoroutine(Util.CoSummonDamageViewer(enemy.gameObject, _earthQuakeDamage));
            }
        }
        Debug.Log("Casting EarthQuake");
    }
    protected override void UpdateAnimation()
    {
        if (_playerState == Define.PlayerState.Idle)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play("PLAYER_IDLE_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(_playerScale, _playerScale, 1.0f);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play("PLAYER_IDLE_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(-1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(-_playerScale, _playerScale, 1.0f);
                    break;
            }
        }
        else if (_playerState == Define.PlayerState.Run)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play("PLAYER_RUN_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(_playerScale, _playerScale, 1.0f);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play("PLAYER_RUN_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(-1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(-_playerScale, _playerScale, 1.0f);
                    break;
            }
        }
        else if (_playerState == Define.PlayerState.Jump)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play("PLAYER_JUMP_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(_playerScale, _playerScale, 1.0f);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play("PLAYER_JUMP_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(-1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(-_playerScale, _playerScale, 1.0f);
                    break;
            }
        }
        else if (_playerState == Define.PlayerState.Attack)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play("PLAYER_ATTACK_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(_playerScale, _playerScale, 1.0f);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play("PLAYER_ATTACK_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(-1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(-_playerScale, _playerScale, 1.0f);
                    break;
            }
        }
        else if (_playerState == Define.PlayerState.Death)
        {
            switch (_dir)
            {
                case Define.MoveDir.Right:
                    _animator.Play("PLAYER_DEATH_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(_playerScale, _playerScale, 1.0f);
                    break;
                case Define.MoveDir.Left:
                    _animator.Play("PLAYER_DEATH_RIGHT");
                    if (IsOnMovingPlatform)
                        transform.localScale = new Vector3(-1.706559f, 5f, 0);
                    else
                        transform.localScale = new Vector3(-_playerScale, _playerScale, 1.0f);
                    break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (_dir == Define.MoveDir.Right)
        {
            Gizmos.color = Color.blue;
        }
        else if (_dir == Define.MoveDir.Left)
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawWireCube(jumpCollision.transform.position + checkBoxPos, checkBoxSize);
        Gizmos.DrawWireSphere(transform.position + _pullRangeOrigin, _pullRangeRadious);
        Gizmos.DrawWireCube(transform.position + _earthQuakeRangeOrigin, _earthQuakeSize); 
    }

    // healingRatio 퍼센트만큼 회복
    public float healingRatio = 15f;
    public void Heal()
    {
        _stat._hp += (_stat._maxHp * (healingRatio / 100f));

        // 최대 체력 이상 회복 불가능
        if (_stat._hp > _stat._maxHp) 
        {
            _stat._hp = _stat._maxHp;
        }
    }

    public Camera mainCamera;
    Vector3 cameraPos;

    [SerializeField][Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [SerializeField][Range(0.1f, 1f)] float duration = 0.2f;

    public void Shake()
    {
        if (UI_GameController.GetComponent<UI_GameController>()._wantShaking)
        {
            mainCamera = Camera.main;
            cameraPos = mainCamera.transform.position;
            InvokeRepeating("StartShake", 0f, 0.005f);
            Invoke("StopShake", duration);
        }
    }

    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCamera.transform.position = cameraPos;
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
        mainCamera.transform.position = cameraPos;
    }
    public int _reflectionProbability = 10;
    public int _reflectionRatio = 100;
    public void Reflection(GameObject enemy, int dmg)
    {
        if (Random.Range(1, 100) <= _reflectionProbability)
        {
            Util.SummonDamageViewer(enemy, dmg * (_reflectionRatio / 100));
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 밟은 플랫폼 리스트에 저장
        if (collision.gameObject.tag == "Platform")
        {
            //_curSteppedPlatform = collision.gameObject;
            //if(collision.transform.GetComponent<PlatformController>().PlatformType != Define.PlatformType.Sloping)
            //{
            //    _rb.sharedMaterial.friction = 1;
            //    Debug.Log("asd");
            //}
        }
        // jumping 처리 부분 //
        if (collision.gameObject.CompareTag("Platform") && _rb.velocity.y == 0)
        {
            _playerState = Define.PlayerState.Idle;
            _touchJump = 0;
            _canJump = false;
            //Debug.Log("Enter!!");
            if (_curSteppedPlatform.gameObject == collision.gameObject) 
                CastingEarthQuake();
        }
        // sloping에서 점프후 착지해도 땅에 닿은 거 처럼 초기화
        if (collision.gameObject.CompareTag("Platform") && collision.gameObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Sloping)
        {
            _playerState = Define.PlayerState.Idle;
            _touchJump = 0;
            _canJump = false;
            if (_curSteppedPlatform.gameObject == collision.gameObject)
                CastingEarthQuake();
        }
        // jumping 처리 부분 //

        //// moving 플랫폼 처리 부분 //
        ////플랫폼이 45도 이내의 기울기일 때에만 바닥으로 판정
        //if (collision.contacts[0].normal.y > 0.7f)
        //{
        //    _isGrounded = true;
        //    //접촉한 오브젝트의 태그가 platform 일 때,
        //    if (collision.gameObject.tag == "Platform" &&
        //        collision.gameObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving)
        //    {
        //        //접촉한 순간의 오브젝트 위치를 저장
        //        contactPlatform = collision.gameObject;
        //        platformPosition = contactPlatform.transform.position;
        //        //접촉한 순간의 오브젝트 위치와 캐릭터 위치의 차이를 distance에 저장
        //        distance = platformPosition - transform.position;
        //    }
        //}
        //// moving 플랫폼 처리 부분 //
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //_isTouchingWall = true;
            transform.GetComponent<Rigidbody2D>().sharedMaterial = frictionless;
        }
        else
        {
            //_isTouchingWall = false;
            transform.GetComponent<Rigidbody2D>().sharedMaterial = slope;
        }
        // jumping 처리 부분 //
        if (collision.gameObject.CompareTag("Platform") && _rb.velocity.y == 0)
        {
            if (_playerState != Define.PlayerState.Attack)
                _playerState = Define.PlayerState.Idle;
            //if (collision.gameObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving)
            //{
            //    transform.SetParent(collision.transform);
            //    IsOnMovingPlatform = true;
            //    Debug.Log("in");
            //}
            //// 플랫폼에 닿을때 모션 예외처리
            //if (_playerState != Define.PlayerState.Attack || _playerState != Define.PlayerState.Jump)
            //{
            //    _playerState = Define.PlayerState.Idle;
            //    _touchJump = 0;
            //    _canJump = false;
            //}
            //else if (_playerState == Define.PlayerState.Attack)
            //{
            //    // CoChangeStateDelay 함수에 의해 일정 딜레이 후에 idle로 바뀜
            //    _touchJump = 0;
            //    _canJump = false;
            //}
        }
        // jumping 처리 부분 //

        // moving 플랫폼 처리 부분 //
        if (_curSteppedPlatform != null &&
            _curSteppedPlatform.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving &&
            _isGrounded && _rb.velocity.y == 0 && collision.gameObject.tag == "Platform" &&
                collision.gameObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving)
        {
            transform.SetParent(collision.transform, true);
            IsOnMovingPlatform = true;
        }
        //if (collision.contacts[0].normal.y > 0.7f)
        //{
        //    //접촉한 오브젝트의 태그가 platform 일 때, 출동중 처리
        //    if (_isGrounded && _rb.velocity.y == 0 && collision.gameObject.tag == "Platform" &&
        //        collision.gameObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving)
        //    {
        //        _isCollising = true;
        //        contactPlatform = collision.gameObject;
        //        platformPosition = contactPlatform.transform.position;
        //        //충돌중인 오브젝트 위치와 캐릭터 위치의 차이를 실시간으로 distance에 저장
        //        distance = platformPosition - transform.position;
        //        //Debug.Log("attach");
        //    }
    }
        // moving 플랫폼 처리 부분 //
    void OnCollisionExit2D(Collision2D collision)
    {
        //// jumping 처리 부분 //
        //if (collision.gameObject.CompareTag("Platform") && _rb.velocity.y == 0)
        //{
        //    _playerState = Define.PlayerState.Idle;
        //    _touchJump = 0;
        //    _canJump = false;
        //    //Debug.Log("Exit!!");
        //}
        //// jumping 처리 부분 //

        // moving 플랫폼 처리 부분 //
        //접촉한 오브젝트의 태그가 platform 일 때, EXIT하면 moving 관련 변수 전부 초기화
        if (collision.gameObject.tag == "Platform" &&
            collision.gameObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving)
        {
            _isGrounded = false;
            _isCollising = false;
            contactPlatform = null;
            platformPosition = null;
            distance = null;

            //transform.SetParent(null);
            //IsOnMovingPlatform = false;
            //Debug.Log("exit");
        }
        //if (collision.gameObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving)
        //{
        //}

        if (collision.gameObject.tag == "Platform" && collision.gameObject.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving)
        {
            transform.SetParent(null);
            IsOnMovingPlatform = false;
            DontDestroyOnLoad(gameObject);
        }
        // moving 플랫폼 처리 부분 //
    }
    public override int OnDamaged(int damage)
    {
        int dmg = base.OnDamaged(damage);
        if (monsterHittedMe != null && _specialAbilityList.Contains(Define.SpecialAbility.Reflection))
            Reflection(monsterHittedMe, dmg);
        if (isDead)
        {
            //// 부활 특수 능력
            if (_specialAbilityList.Contains(Define.SpecialAbility.Resurrection) && isResurrection == false)
            {
                _stat._hp = _stat._maxHp;
                isDead = false;
                isResurrection = true;
            }
            // 부활 특수 능력
            if (isResurrection == false)
            {
                _stat._hp = _stat._maxHp;
                isDead = false;
                isResurrection = true;
            }
            else
            {
                _playerState = Define.PlayerState.Death;
                gameObject.tag = "DeadPlayer";

                soundManager.effect.PlayOneShot(soundManager.bellDieEffect);

                deathPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        return dmg;
    }
    void InitWhenSceneLoaded()
    {
        _highestHeight = 0;
        // 땅에 닿았나 확인
        _isGrounded = false;
        // 충돌중인가 확인
        _isCollising = false;
        _canJump = false;
        _canMoveRight = false;
        _canMoveLeft = false;
        // 공격 관련 플래그
        _canAttack = true;
        _curTime = 0f;
        // 미끄러운 지역에서 활성화 TODO
        _isSlippery = false;
        SetSpawnPoint();
        SetBackGroundImage();

        // 물약 활성화
        GameObject.Find("Main Camera").GetComponent<GameStarter>().ActivePotions();
        if (_specialAbilityList.Contains(Define.SpecialAbility.HPPotion))
            GameObject.Find("Main Camera").GetComponent<GameStarter>().ActivePotions();
    }

    // 씬 로드 될 때 마다 스폰 포인트 지정
    void SetSpawnPoint()
    {
        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");
        if (spawnPoint != null)
            transform.position = spawnPoint.transform.position;
    }
    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitWhenSceneLoaded();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}