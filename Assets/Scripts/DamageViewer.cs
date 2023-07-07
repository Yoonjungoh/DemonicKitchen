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
    // �ؽ�Ʈ �̵� �ӵ�
    private float _moveSpeed = 2f;
    [SerializeField]
    // ���� ��ȯ �ӵ�
    private float _alphaSpeed = 2f;
    [SerializeField]
    // ������ ��� ������Ʈ Hide �ð�
    private float _hidingTime = 2f;
    public int damage;
    void Start()
    {
        GameObject damageViewerFolder = GameObject.Find("DamageViewers");
        if (damageViewerFolder == null)
            damageViewerFolder = new GameObject { name = "DamageViewers" };

        // ������ �ִ� ������ ��� �켱 �����ֱ�
        gameObject.SetActive(true);
        _damageText = GetComponent<TextMeshPro>();
        _damageText.text = damage.ToString();
        _color = _damageText.color;

        if (_type == Define.CreatureType.Player)
            _color = Color.red;
        else if (_type == Define.CreatureType.Monster)
            _color = Color.white;

        Invoke("HideDamageViewer", _hidingTime);

        // ������ ��� �θ� ����
        transform.parent = damageViewerFolder.transform;
    }

    void Update()
    {
        transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));

        _color.a = Mathf.Lerp(_color.a, 0, Time.deltaTime * _alphaSpeed);

        _damageText.color = _color;
    }
    // ������Ʈ Ǯ���� ���� ������ ��� �����
    void HideDamageViewer()
    {
        gameObject.SetActive(false);
    }
}
