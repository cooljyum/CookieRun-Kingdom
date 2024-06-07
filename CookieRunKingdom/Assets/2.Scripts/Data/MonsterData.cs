using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; // 고유 식별자
    public string Name; // 이름
    public MonsterType Type; // 몬스터 유형 (근거리, 원거리, 보스 등)

    [Header("Stats")]
    public float Hp; // 몬스터의 체력
    public float Attack; // 몬스터의 공격력
    public float Defence; // 몬스터의 방어력
    public float Critical; // 몬스터의 치명타 확률
    public float Speed; // 몬스터의 이동 속도

    [Header("Attack Info")]
    public AttackType AttackType; // 공격 유형 (물리, 마법, 원거리, 근거리)
    public float AttackDamage; // 공격 데미지
    public float AttackInterval; // 공격 간격 (초 단위)
    public List<SpecialAttack> SpecialAttacks; // 특수 공격 목록

    [Header("Rewards")]
    public int ExperiencePoints; // 처치 시 얻는 경험치
    public List<string> LootTable; // 드랍 아이템 목록 및 확률

    [Header("Appearance")]
    public SkeletonDataAsset SkeletonDataAsset; // Spine 애니메이션 데이터
}

// 몬스터 유형 열거형
public enum MonsterType
{
    Melee, // 근거리
    Ranged, // 원거리
    Boss // 보스
}

// 공격 유형 열거형
public enum AttackType
{
    Melee, // 근거리 공격
    Ranged, // 원거리 공격
    Magical // 마법 공격
}

// 특수 공격 클래스
[System.Serializable]
public class SpecialAttack
{
    public string Name; // 특수 공격 이름
    public int Damage; // 특수 공격 피해량
    public float Cooldown; // 특수 공격 재사용 대기시간 (초 단위)
}
