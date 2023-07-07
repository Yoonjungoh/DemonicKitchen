using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : PlatformController
{
    public PlayerController playerController;
    private bool isGrounded = false;
    private GameObject contactPlatform;
    private Vector3 platformPosition;
    private Vector3 distance;

    public GameManager gameManager;
    public Transform _desPos;
    public Transform _startPos;

    private Define.MovingPlatformType _type;
    [SerializeField]
    private float _lerpTime = 2f;
    private float _currentTime = 0f;
    private bool _isAttach = false;
    void Start()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _platformType = Define.PlatformType.Moving;
        _type = Define.MovingPlatformType.GoToDest;
        transform.position = _startPos.position;
        //transform.localScale = new Vector3(0.5859744f, 0.2f, 0f);
        //gameObject.GetComponent<BoxCollider2D>().size += new Vector2(4.937955f, 0);
    }

    void Update()
    {
        UpdateMovingPlatform();
        //Debug.Log(playerController.Rb.velocity.y);
    }
    void UpdateMovingPlatform()
    {
        switch (_type)
        {
            case Define.MovingPlatformType.GoToDest:
                _currentTime += Time.deltaTime;
                if (_currentTime >= _lerpTime)
                    _currentTime = _lerpTime;
                transform.position = Vector2.Lerp(_startPos.position, _desPos.position, _currentTime / _lerpTime);

                if (transform.position.x == _desPos.position.x)
                {
                    _type = Define.MovingPlatformType.GoToStart;
                    _currentTime = 0f;
                }
                break;

            case Define.MovingPlatformType.GoToStart:
                _currentTime += Time.deltaTime;
                if (_currentTime >= _lerpTime)
                    _currentTime = _lerpTime;
                transform.position = Vector2.Lerp(_desPos.position, _startPos.position, _currentTime / _lerpTime);

                if (transform.position.x == _startPos.position.x)
                {
                    _type = Define.MovingPlatformType.GoToDest;
                    _currentTime = 0f;
                }
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player" )
        //{
        //    collision.transform.SetParent(transform);
        //    collision.gameObject.GetComponent<PlayerController>().IsOnMovingPlatform = true;
        //}
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    Collider2D collider2D = Physics2D.OverlapBox(playerController.jumpCollision.transform.position + playerController.checkBoxPos, playerController.checkBoxSize, playerController.isLayer);

        //    if (collider2D == null)
        //    {
        //        return;
        //    }
        //    if (playerController == null)
        //    {
        //        return;
        //    }
        //    if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0 &&
        //    collider2D.GetComponent<PlatformController>().PlatformType == Define.PlatformType.Moving)
        //    {
        //        collision.transform.SetParent(transform, true);
        //        collision.gameObject.GetComponent<PlayerController>().IsOnMovingPlatform = true;
        //    }
        //}
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    collision.transform.SetParent(null);
        //    collision.gameObject.GetComponent<PlayerController>().IsOnMovingPlatform = false;
        //    DontDestroyOnLoad(collision.gameObject);
        //}
    }
}