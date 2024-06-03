using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public enum ECookieGrade { Common, Rare, Epic, SuperEpic, Ancient, Legendary, Dragon, Enemy }
public enum ECookieType { Assault, Defense, Magic, Infiltration, Ranged, Headling, Explosion, Support, Enemy }
public enum ECookiePosition { Front, Center, Rear }

[CreateAssetMenu]
public class CookieData : CharacterData
{
    [SerializeField] private int _cookieIndex;

    [SerializeField] private float _skillCoolTime;

    [SerializeField] private string _appearSpeech;
    [SerializeField] private string _bubbleSpeech;

    [SerializeField] private SkeletonDataAsset _cookiePortraitSkeleton;

    [SerializeField] private Sprite _cookieHeadSprite;
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _idleBlackAndWhiteSprite;
    [SerializeField] private Sprite _skillSprite;
    [SerializeField] private string _skillName;

    [SerializeField] private Sprite _typeSprite;
    [SerializeField] private Sprite _evolutionSprite;

    [SerializeField] private Sprite _cookieGradeSprite;

    [SerializeField] private ECookieGrade _cookieGrade;
    [SerializeField] private ECookieType _cookieType;
    [SerializeField] private ECookiePosition _cookiePosition;

    [Header("아이템")]
    [SerializeField] private ItemData _expCandyItemData;

    public int CookieIndex => _cookieIndex;

    public float SkillCoolTime => _skillCoolTime;

    public string AppearSpeech => _appearSpeech;
    public string BubbleSpeech => _bubbleSpeech;

    public SkeletonDataAsset CookiePortrait => _cookiePortraitSkeleton;
    public Sprite CookieHeadSprite => _cookieHeadSprite;
    public Sprite IdleSprite => _idleSprite;
    public Sprite IdleBlackAndWhiteSprite => _idleBlackAndWhiteSprite;
    public Sprite SKillSprite => _skillSprite;
    public string SKillName => _skillName;
    public Sprite TypeSprite => _typeSprite;
    public Sprite EvolutionSprite => _evolutionSprite;
    public Sprite CookieGradeSprite => _cookieGradeSprite;

    public ECookieGrade CookieGrade => _cookieGrade;
    public ECookieType CookieType => _cookieType;
    public ECookiePosition CookiePosition => _cookiePosition;

    public ItemData ExpCandyItemData => _expCandyItemData;

    public string CookieGradeName
    {
        get
        {
            switch (CookieGrade)
            {
                case ECookieGrade.Common:
                    return "COMMON";
                case ECookieGrade.Rare:
                    return "RARE";
                case ECookieGrade.Epic:
                    return "EPIC";
            }
            return "";
        }
    }

    public string CookieTypeName
    {
        get
        {
            switch (CookieType)
            {
                case ECookieType.Assault:
                    return "돌격형"; 
                case ECookieType.Defense:
                    return "방어형";
                case ECookieType.Magic:
                    return "마법형";
                case ECookieType.Infiltration:
                    return "침투형";
                case ECookieType.Ranged:
                    return "사격형";
                case ECookieType.Headling:
                    return "회복형";
                case ECookieType.Explosion:
                    return "폭발형";
                case ECookieType.Support:
                    return "지원형";
                case ECookieType.Enemy:
                    return "적";
            }
            return "";
        }
    }

    public string CookiePositionName
    {
        get
        {
            switch (CookiePosition)
            {
                case ECookiePosition.Front:
                    return "전방";
                case ECookiePosition.Center:
                    return "중앙";
                case ECookiePosition.Rear:
                    return "후방";
            }
            return "";
        }
    }
}
