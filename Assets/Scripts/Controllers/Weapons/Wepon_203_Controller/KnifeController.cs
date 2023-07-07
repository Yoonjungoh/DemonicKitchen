using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : Weapon_203_Controller
{
    Rigidbody2D _rb;
    [SerializeField]
    private float _rotationKnifeAngle;
    bool _isRight;
    SoundManager soundManager;
    void Start()
    {
        GameObject projectileFolder = GameObject.Find("Projectiles");
        if (projectileFolder == null)
            projectileFolder = new GameObject { name = "Projectiles" };

        // �߻�ü �θ� ����
        transform.parent = projectileFolder.transform;

        _rb = transform.GetComponent<Rigidbody2D>();
        // ���� �ɸ��� ����ȭ TODO
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (_player.Dir == Define.MoveDir.Right)
            _isRight = true;
        else
            _isRight = false;
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        ThrowKnife();
    }
    void ThrowKnife()
    {
        // ������
        if (_isRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, _rotationKnifeAngle);
            _rb.velocity = new Vector2(1 * _rateSpeed, 0);
        }
        // ����
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, _rotationKnifeAngle);
            _rb.velocity = new Vector2(-1 * _rateSpeed, 0);
        }
    }
    // ������ ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            soundManager.effect.PlayOneShot(soundManager.knifetEffect);
            StartCoroutine(Util.CoSummonDamageViewer(collision.gameObject, (_weaponAttack * _level) + _player.GetComponent<Stat>().Attack));
        }

        if (collision.tag == "Wall")
            gameObject.SetActive(false);
    }
    public void LevelUp()
    {
        _level++;
    }
}