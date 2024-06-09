using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; //�����ĺ���
    public string Name; //�̸�

    [Header("Stats")]
    public float Hp; //�ִ�ü��
    public float Attack; //���ݷ�
    public float Defence; //����
    public float Critical; //ġ��Ÿ Ȯ��
    public int Type; //����
    public int Rarity; //��ͼ�

    [Header("Attack Info")]
    public AttackType AttackType; // ���� ���� (����, ����, ���Ÿ�, �ٰŸ�)
    public float AttackDamage; // ���� ������
    public float AttackInterval; // ���� ���� (�� ����)
    public List<SpecialAttack> SpecialAttacks; // Ư�� ���� ���

    [Header("Appearance")]
    public SkeletonDataAsset SkeletonDataAsset;
    public Sprite profileImage;
    public Sprite typeImage;

    [Header("Animation Info")]
    public List<AnimationMapping> AnimationMappings = new List<AnimationMapping>(); // �ִϸ��̼� �̸� ���� ����Ʈ
}


// Ư�� ���� Ŭ����
[System.Serializable]
public class SpecialAttack
{
    public string Name; // Ư�� ���� �̸�
    public int Damage; // Ư�� ���� ���ط�
    public float Cooldown; // Ư�� ���� ���� ���ð� (�� ����)
}

// ���� ���� ������
public enum AttackType
{
    Melee, // �ٰŸ� ����
    Ranged, // ���Ÿ� ����
    Magical // ���� ����
}

// �ִϸ��̼� ���� Ŭ����
[System.Serializable]
public class AnimationMapping
{
    public string Key; // �ִϸ��̼� Ű
    public string AnimationName; // �ִϸ��̼� �̸�
}