using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingPlatformController : PlatformController
{
    BoxCollider2D _boxCollider2D;
    [SerializeField]
    // 플랫폼이 활성화 됐나 비활성화 됐나 알아보는 상태
    Define.BlinkingPlatformType _type;
    // 스프라이트 원래 색깔 값
    Color _originalColor;
    float _timer = 0f;
    [SerializeField]
    // 사라졌다가 다시 나타나는 시간
    float _blinkingDelay = 2f;
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _originalColor = gameObject.GetComponent<SpriteRenderer>().color;
        _platformType = Define.PlatformType.Blinking;
        _type = Define.BlinkingPlatformType.Active;
    }
    void Update()
    {
        // 게임 시작하자마자 타이머 시작
        switch (_type)
        {
            case Define.BlinkingPlatformType.Active:
                InactiveBlinkingPlatform();
                break;
            case Define.BlinkingPlatformType.Inactive:
                ActiveBlinkingPlatform();
                break;
        }
    }
    void ActiveBlinkingPlatform()
    {
        _timer += Time.deltaTime;
        if (_timer >= _blinkingDelay)
        {
            gameObject.GetComponent<SpriteRenderer>().color = _originalColor;
            _boxCollider2D.isTrigger = false;
            _type = Define.BlinkingPlatformType.Active;
            _timer = 0;
        }
    }
    void InactiveBlinkingPlatform()
    {
        _timer += Time.deltaTime;
        if (_timer >= _blinkingDelay / 2f)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            _boxCollider2D.isTrigger = true;
            _type = Define.BlinkingPlatformType.Inactive;
        }
    }
}
