using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutCamera : MonoBehaviour
{
    public bool _isOut = true;
    // 카메라에서 오브젝트가 사라질 때 함수
    // Scene, Game 카메라 둘다에 안보여야함
    // Destroy하면 플레이어를 참고하는 모든 코드들이 null 에러 발생하여 비활성화

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
