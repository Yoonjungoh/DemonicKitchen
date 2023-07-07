using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutCamera : MonoBehaviour
{
    public bool _isOut = true;
    // ī�޶󿡼� ������Ʈ�� ����� �� �Լ�
    // Scene, Game ī�޶� �Ѵٿ� �Ⱥ�������
    // Destroy�ϸ� �÷��̾ �����ϴ� ��� �ڵ���� null ���� �߻��Ͽ� ��Ȱ��ȭ

    private void Start()
    {
        if (gameObject.tag == "Player" || gameObject.tag == "Platform")
            _isOut = true;
        else
            _isOut = false;
    }
    void OnBecameInvisible()
    {
        if (_isOut)
        {
            if (gameObject.tag == "Player")
            {
                //Debug.Log("Die cause outCamera");
                gameObject.GetComponent<PlayerController>().OnDamaged(int.MaxValue);
            }
            else if (gameObject.tag == "Platform")
            {
                //Debug.Log("set off cause outCamera");
                gameObject.SetActive(false);
            }
        }
    }   
}
