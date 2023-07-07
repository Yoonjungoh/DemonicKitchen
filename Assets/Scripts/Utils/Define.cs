using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum SpecialAbility
    {
        None,
        Resurrection,
        DoubleJump,
        EarthQuake,
        HPPotion,
        Reflection,
    }
    public enum DropItem
    {
        None,
        Gold,
        Ingredient,
        Potion,
    }
    public enum Sound
    {
        None,
        Bgm,
        Effect,
    }
    public enum StageType
    {
        None,
        Devildom,
        HumanWorld,
        Heaven,
    }
    public enum BossSkillType
    {
        None,
        Summon,
        Laser,
        ThrowSpear,
        Chaos,
        DeadFlying,
    }
    public enum BossName
    {
        None,
        Cerberus,
    }
    public enum BossType
    {
        None,
        Devildom,
        HumanWorld,
        Heaven,
    }
    // 몬스터의 종류에 따른 상태
    public enum MonsterType
    {
        None,
        Running,
        Fixed,
        Flying,
    }
    // 깜빡거리는 플랫폼의 상태
    public enum BlinkingPlatformType
    {
        None,
        Active,
        Inactive,
    }
    // 이동하는 플랫폼의 이동 방향에 따른 상태
    public enum MovingPlatformType
    {
        None,
        GoToDest,
        GoToStart,
    }
    // 플랫폼의 종류를 알기 위한 상태
    public enum PlatformType
    {
        None,
        Normal,
        Moving,
        Sloping,
        Broken,
        Blinking,
    }
    // Scene이 어떤 상태인지 알기위한 상태
    public enum SceneType
    {
        None,
        Main,
        Shop,
        Kitchen,
        Game,
        Boss,
    }
    // Creature의 이동 방향
    public enum MoveDir
    {
        None,
        Right,
        Left,
        Up,
    }
    // 몬스터가 적을 추적중인가 싸움상태인가 알아보는 상태
    public enum MonsterScanState
    {
        None,
        Patrol,
        Chase,
    }
    // 몬스터 상태가 Skill일때 해당 스킬의 상태
    public enum MonsterSkillState
    {
        None,
        // 보스1 스킬 id:2000
        Meteor,
        Lightning,
        Lance,
        // 보스2 스킬 id:2001
    }
    // 몬스터가 공격중인가 뛰고 있나 알아보는 상태
    public enum MonsterState
    {
        None,
        Idle,
        Attack,
        Run,
        Jump,
        Fly,
        Skill,
        Death,
    }

    // 플레이어가 공격중인가 뛰고 있나 알아보는 상태
    public enum PlayerState
    {
        None,
        Idle,
        Attack,
        Run,
        Jump,
        Skill,
        Death,
    }
    // 죽었나 살아있나 상태
    public enum CreatureState
    {
        None,
        Alive,
        Dead,
    }
    // 몬스터인지 플레이어인지 상태
    public enum CreatureType
    {
        None,
        Player,
        Monster,
    }
}