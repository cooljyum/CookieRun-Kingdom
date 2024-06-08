using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; //고유식별값
    public string Name; //이름

    [Header("Stats")]
    public float Hp; //최대체력
    public float Attack; //공격력
    public float Defence; //방어력
    public float Critical; //치명타 확룔
    public int Type; //유형
    public int Rarity; //희귀성

    [Header("Attack Info")]
    public AttackType AttackType; // 공격 유형 (물리, 마법, 원거리, 근거리)
    public float AttackDamage; // 공격 데미지
    public float AttackInterval; // 공격 간격 (초 단위)
    public List<SpecialAttack> SpecialAttacks; // 특수 공격 목록

    [Header("Appearance")]
    public SkeletonDataAsset SkeletonDataAsset;
    public Sprite profileImage;
    public Sprite typeImage;
}


// 특수 공격 클래스
[System.Serializable]
public class SpecialAttack
{
    public string Name; // 특수 공격 이름
    public int Damage; // 특수 공격 피해량
    public float Cooldown; // 특수 공격 재사용 대기시간 (초 단위)
}

// 공격 유형 열거형
public enum AttackType
{
    Melee, // 근거리 공격
    Ranged, // 원거리 공격
    Magical // 마법 공격
}