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
    // ������ ������ ���� ����
    public enum MonsterType
    {
        None,
        Running,
        Fixed,
        Flying,
    }
    // �����Ÿ��� �÷����� ����
    public enum BlinkingPlatformType
    {
        None,
        Active,
        Inactive,
    }
    // �̵��ϴ� �÷����� �̵� ���⿡ ���� ����
    public enum MovingPlatformType
    {
        None,
        GoToDest,
        GoToStart,
    }
    // �÷����� ������ �˱� ���� ����
    public enum PlatformType
    {
        None,
        Normal,
        Moving,
        Sloping,
        Broken,
        Blinking,
    }
    // Scene�� � �������� �˱����� ����
    public enum SceneType
    {
        None,
        Main,
        Shop,
        Kitchen,
        Game,
        Boss,
    }
    // Creature�� �̵� ����
    public enum MoveDir
    {
        None,
        Right,
        Left,
        Up,
    }
    // ���Ͱ� ���� �������ΰ� �ο�����ΰ� �˾ƺ��� ����
    public enum MonsterScanState
    {
        None,
        Patrol,
        Chase,
    }
    // ���� ���°� Skill�϶� �ش� ��ų�� ����
    public enum MonsterSkillState
    {
        None,
        // ����1 ��ų id:2000
        Meteor,
        Lightning,
        Lance,
        // ����2 ��ų id:2001
    }
    // ���Ͱ� �������ΰ� �ٰ� �ֳ� �˾ƺ��� ����
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

    // �÷��̾ �������ΰ� �ٰ� �ֳ� �˾ƺ��� ����
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
    // �׾��� ����ֳ� ����
    public enum CreatureState
    {
        None,
        Alive,
        Dead,
    }
    // �������� �÷��̾����� ����
    public enum CreatureType
    {
        None,
        Player,
        Monster,
    }
}