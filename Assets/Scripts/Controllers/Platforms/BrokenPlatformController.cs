using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatformController : PlatformController
{
    float _timer = 0f;
    [SerializeField]
    // 부서지는 시간
    float _brokenDelay = 1f;
    bool isPlayerStepped = false;
    void Start()
    {
        _platformType = Define.PlatformType.Broken;
    }
    private void Update()
    {
        // 밟는 순간 타이머 시작
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
        // 플레이어가 플랫폼을 밟고 있고 속도가 0 일 때
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            isPlayerStepped = true;
        }
    }

}
