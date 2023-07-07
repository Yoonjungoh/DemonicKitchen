using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PortalController : MonoBehaviour
{
    protected Animator _animator;
    protected GameManager gameManager;
    protected GameObject _player;

    protected bool isEnter = false;
    protected bool isFirstEnter = true;

    protected void Init()
    {
        _player = GameObject.Find("Player");
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
        _animator = transform.GetComponent<Animator>();
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        isEnter = true;
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        isEnter = false;
    }
    public abstract void EnterPortal();
}
