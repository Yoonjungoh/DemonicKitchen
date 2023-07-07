using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementSO", menuName = "Scriptable Objects/New Achievement", order = 3)]
public class AchievementSO : ScriptableObject
{
    public string title;
    public string description;
    public string reward;
    public int rewardQty;
}
