using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Actor : ScriptableObject
{
    public string Name;
    public List<Sprite> sprites;
}