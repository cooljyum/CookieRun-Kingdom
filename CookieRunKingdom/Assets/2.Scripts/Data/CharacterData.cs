using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
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
    public float AttackRange; // 공격 범위
    public SkillBase Skill; // 특수 스킬

    [Header("Appearance")]
    public SkeletonDataAsset SkeletonDataAsset;
    public Sprite ProfileImage;
    public Sprite TypeImage;
    public SkillBtnImg SkillBtnImg;


    [Header("Animation Info")]
    public List<AnimationMapping> AnimationMappings = new List<AnimationMapping>(); // 애니메이션 이름 매핑 리스트

}

// 공격 유형 열거형
public enum AttackType
{
    Melee, // 근거리 공격
    Ranged, // 원거리 공격
    Magical // 마법 공격
}

// 애니메이션 매핑 클래스
[System.Serializable]
public class AnimationMapping
{
    public string Key; // 애니메이션 키
    public string AnimationName; // 애니메이션 이름
}

// 스킬 버튼 이미지
[System.Serializable]
public class SkillBtnImg
{
    public Sprite OnImg;
    public Sprite OffImg;
    public Sprite SkillImg;
}