using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private Sprite[] ProjectileSprites;
    GameObject _target;
    Vector3 _startPos;
    Vector3 _destPos;
    Rigidbody2D _rb;
    [SerializeField]
    // 발사체 속도
    private float _rateSpeed = 1.5f;
    [SerializeField]
    // 발사체 대미지
    private int _projectileDamage = 0;
    // 대각선으로 쏘고 싶을때
    public bool _isFireDiagonal = false;
    public GameObject masterMonster;
    void Start()
    {
        GameObject projectileFolder = GameObject.Find("Projectiles");
        if (projectileFolder == null)
            projectileFolder = new GameObject { name = "Projectiles" };


        _target = GameObject.FindWithTag("Player");
        _startPos = transform.position;
        if (_target != null) 
        _destPos = _target.transform.position;
        _rb = GetComponent<Rigidbody2D>();

        // 발사체 부모 설정
        transform.parent = projectileFolder.transform;
    }
    public void Init(int index, int projectileDamage, float rateSpeed)
    {
        //gameObject.GetComponent<SpriteRenderer>().sprite =ProjectileSprites[index];
        _projectileDamage = projectileDamage;
        _rateSpeed = rateSpeed;
    }
    void Update()
    {
        if (_target != null)
        {
            if (_destPos.x - _startPos.x >= 0 && gameObject.name == "Projectile_5(Clone)")
            {
                _rb.velocity = new Vector2(1 * _rateSpeed, 0);
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (_destPos.x - _startPos.x < 0 && gameObject.name == "Projectile_5(Clone)")
            {
                _rb.velocity = new Vector2((-1) * _rateSpeed, 0);
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            if(_destPos.x - _startPos.x >= 0 && gameObject.name != "Projectile_5(Clone)")
            {
                _rb.velocity = new Vector2(1 * _rateSpeed, 0);
                // 각 그대로 유지
            }
            else if (_destPos.x - _startPos.x < 0 && gameObject.name != "Projectile_5(Clone)")
            {
                _rb.velocity = new Vector2((-1) * _rateSpeed, 0);
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        else
            Debug.Log("Player Not Found during update");
        if (_isFireDiagonal)
        {
            if (_target != null)
            {
                Vector2.MoveTowards(transform.position, _target.transform.position, _rateSpeed);
                if (_target.transform.position.x - transform.position.x >= 0 && gameObject.name == "Projectile_5(Clone)")
                {
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                }
                else if(_target.transform.position.x - transform.position.x < 0 && gameObject.name == "Projectile_5(Clone)")
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                if (_target.transform.position.x - transform.position.x >= 0 && gameObject.name != "Projectile_5(Clone)")
                {
                    // 각 그대로 유지
                }
                else if(_target.transform.position.x - transform.position.x < 0 && gameObject.name != "Projectile_5(Clone)")
                {
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                }
            }
            else
                Debug.Log("Player Not Found during update");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 태그에만 반응
        if (collision.gameObject.CompareTag("Player"))
        {
            //// 데미지 계산 후 데미지 뷰어 소환하는 부분
            //int damage = _target.GetComponent<PlayerController>().OnDamaged(projectileDamage);
            //GameObject damageText = Resources.Load("Prefabs/UI/DamageText") as GameObject;
            //damageText.GetComponent<DamageViewer>().damage = damage;
            //Vector3 instantiatePos = new Vector3(_target.transform.position.x,
            //    _target.transform.position.y + (_target.GetComponent<BoxCollider2D>().size.y),
            //    0);
            //Instantiate(damageText, instantiatePos, Quaternion.identity);
            ////Debug.Log($"{_target.name}가 {damage}데미지를 입음");

            Util.SummonDamageViewer(collision.gameObject, _projectileDamage);
            collision.GetComponent<PlayerController>().monsterHittedMe = masterMonster;
            // 발사체 비활성화
            gameObject.SetActive(false);
        }
    }
}