using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPortalController : PortalController
{
    [SerializeField]
    protected Define.BossType _bossType = Define.BossType.None;
    private void Start()
    {
        base.Init();
        switch (_bossType)
        {
            case Define.BossType.Devildom:
                _animator.Play("BOSSPORTAL_DEVILDOM");
                break;
            case Define.BossType.HumanWorld:
                _animator.Play("BOSSPORTAL_DEVILDOM");
                break;
            case Define.BossType.Heaven:
                _animator.Play("BOSSPORTAL_DEVILDOM");
                break;
        }
    }
    public override void EnterPortal()
    {
        Debug.Log("click");
        gameManager.toatlScore += _player.GetComponent<PlayerController>().HighestHeight;
        _player.GetComponent<PlayerController>()._highestHeight = 0f;
        // 씬타입 변경
        gameManager.sceneType = Define.SceneType.Boss;
        // BossScene 이름 만들 때 컨벤션에 맞춰 만들기
        SceneManager.LoadScene($"{_bossType}Boss");
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        isFirstEnter = false;
        gameManager.toatlScore += _player.GetComponent<PlayerController>().HighestHeight;
        _player.GetComponent<PlayerController>()._highestHeight = 0f;
        // 씬타입 변경
        gameManager.sceneType = Define.SceneType.Boss;
        // BossScene 이름 만들 때 컨벤션에 맞춰 만들기
        SceneManager.LoadScene($"{_bossType}Boss");
    }
}
