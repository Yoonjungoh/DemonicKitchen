using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialAbilities", menuName = "Scriptable Objects/New Ability", order = 5)]
public class SpecialAbilities : ScriptableObject
{
    public int ID;
    public Sprite image;
    public string title;
    public string rank;
    public int unlockCost;
    public int enhanceCost;
    public int enhanceStoneCost;
    public int prob;
    public int enhanceProb;
    public bool isUnlocked;
    public int skillLevel;
}
