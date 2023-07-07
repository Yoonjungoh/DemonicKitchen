using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageViewer : MonoBehaviour
{
    public Define.CreatureType _type;
    TextMeshPro _damageText;
    public TextMeshPro DamageText { get { return _damageText; } }
    public Color _color;
    [SerializeField]
    // 텍스트 이동 속도
    private float _moveSpeed = 2f;
    [SerializeField]
    // 투명도 변환 속도
    private float _alphaSpeed = 2f;
    [SerializeField]
    // 데미지 뷰어 오브젝트 Hide 시간
    private float _hidingTime = 2f;
    public int damage;
    void Start()
    {
        GameObject damageViewerFolder = GameObject.Find("DamageViewers");
        if (damageViewerFolder == null)
            damageViewerFolder = new GameObject { name = "DamageViewers" };

        // 숨겨져 있는 데미지 뷰어 우선 보여주기
        gameObject.SetActive(true);
        _damageText = GetComponent<TextMeshPro>();
        _damageText.text = damage.ToString();
        _color = _damageText.color;

        if (_type == Define.CreatureType.Player)
            _color = Color.red;
        else if (_type == Define.CreatureType.Monster)
            _color = Color.white;

        Invoke("HideDamageViewer", _hidingTime);

        // 데미지 뷰어 부모 설정
        transform.parent = damageViewerFolder.transform;
    }

    void Update()
    {
        transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));

        _color.a = Mathf.Lerp(_color.a, 0, Time.deltaTime * _alphaSpeed);

        _damageText.color = _color;
    }
    // 오브젝트 풀링을 위한 데미지 뷰어 숨기기
    void HideDamageViewer()
    {
        gameObject.SetActive(false);
    }
}
