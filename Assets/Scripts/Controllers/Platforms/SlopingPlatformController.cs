using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopingPlatformController : PlatformController
{
    void Start()
    {
        _platformType = Define.PlatformType.Sloping;
    }
}
