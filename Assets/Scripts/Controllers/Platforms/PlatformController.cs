using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    // ÇÃ·§ÆûÀ» ¹âÀº ÇÃ·¹ÀÌ¾î
    public GameObject steppedPlayer;
    [SerializeField]
    protected Define.PlatformType _platformType;
    public Define.PlatformType PlatformType
    {
        get { return _platformType; }   
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            steppedPlayer = collision.gameObject;
        }
    }
}
