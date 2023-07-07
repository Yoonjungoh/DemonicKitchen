using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatformController : PlatformController
{
    float _timer = 0f;
    [SerializeField]
    // �μ����� �ð�
    float _brokenDelay = 1f;
    bool isPlayerStepped = false;
    void Start()
    {
        _platformType = Define.PlatformType.Broken;
    }
    private void Update()
    {
        // ��� ���� Ÿ�̸� ����
        if (gameObject.activeInHierarchy && isPlayerStepped)
        {
            _timer += Time.deltaTime;

            if (_timer > _brokenDelay)
            {
                gameObject.SetActive(false);
                _timer = 0;
                isPlayerStepped = false;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // �÷��̾ �÷����� ��� �ְ� �ӵ��� 0 �� ��
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            isPlayerStepped = true;
        }
    }

}
