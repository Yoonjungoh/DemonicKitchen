using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingPlatformController : PlatformController
{
    BoxCollider2D _boxCollider2D;
    [SerializeField]
    // �÷����� Ȱ��ȭ �Ƴ� ��Ȱ��ȭ �Ƴ� �˾ƺ��� ����
    Define.BlinkingPlatformType _type;
    // ��������Ʈ ���� ���� ��
    Color _originalColor;
    float _timer = 0f;
    [SerializeField]
    // ������ٰ� �ٽ� ��Ÿ���� �ð�
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
        // ���� �������ڸ��� Ÿ�̸� ����
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
