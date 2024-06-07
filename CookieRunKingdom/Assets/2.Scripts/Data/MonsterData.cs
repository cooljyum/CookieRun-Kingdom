using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; // ���� �ĺ���
    public string Name; // �̸�
    public MonsterType Type; // ���� ���� (�ٰŸ�, ���Ÿ�, ���� ��)

    [Header("Stats")]
    public float Hp; // ������ ü��
    public float Attack; // ������ ���ݷ�
    public float Defence; // ������ ����
    public float Critical; // ������ ġ��Ÿ Ȯ��
    public float Speed; // ������ �̵� �ӵ�

    [Header("Attack Info")]
    public AttackType AttackType; // ���� ���� (����, ����, ���Ÿ�, �ٰŸ�)
    public float AttackDamage; // ���� ������
    public float AttackInterval; // ���� ���� (�� ����)
    public List<SpecialAttack> SpecialAttacks; // Ư�� ���� ���

    [Header("Rewards")]
    public int ExperiencePoints; // óġ �� ��� ����ġ
    public List<string> LootTable; // ��� ������ ��� �� Ȯ��

    [Header("Appearance")]
    public SkeletonDataAsset SkeletonDataAsset; // Spine �ִϸ��̼� ������
}

// ���� ���� ������
public enum MonsterType
{
    Melee, // �ٰŸ�
    Ranged, // ���Ÿ�
    Boss // ����
}

// ���� ���� ������
public enum AttackType
{
    Melee, // �ٰŸ� ����
    Ranged, // ���Ÿ� ����
    Magical // ���� ����
}

// Ư�� ���� Ŭ����
[System.Serializable]
public class SpecialAttack
{
    public string Name; // Ư�� ���� �̸�
    public int Damage; // Ư�� ���� ���ط�
    public float Cooldown; // Ư�� ���� ���� ���ð� (�� ����)
}
