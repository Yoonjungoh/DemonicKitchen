using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonPortalController : PortalController
{
    [SerializeField]
    public Define.StageType _stageType = Define.StageType.None;
    private void Start()
    {
        base.Init();
        switch (_stageType)
        {
            case Define.StageType.Devildom:
                _animator.Play("BOSSPORTAL_DEVILDOM");
                break;
            case Define.StageType.HumanWorld:
                _animator.Play("BOSSPORTAL_DEVILDOM");
                break;
            case Define.StageType.Heaven:
                _animator.Play("BOSSPORTAL_DEVILDOM");
                break;
        }
    }
    public override void EnterPortal()
    {
        // ��Ÿ�� ����
        gameManager.sceneType = Define.SceneType.Game;
        // ��Ÿ�� ����
        gameManager.stageType = _stageType;

        SceneManager.LoadScene(_stageType.ToString());
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        isFirstEnter = false;
        // ��Ÿ�� ����
        gameManager.sceneType = Define.SceneType.Game;
        // ��Ÿ�� ����
        gameManager.stageType = _stageType;

        SceneManager.LoadScene(_stageType.ToString());
    }
}
