using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[System.Serializable]
public class WaveInfo
{
    public BaseController[] enemies = new BaseController[15];
    public int distance = 0;
}

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    [Header("스테이지의 정보")]
    [SerializeField] private bool _isBoss;
    [SerializeField] private SkeletonAnimation _bossSkeletonAnimation;


    [SerializeField] private string _stageName;
    [SerializeField] private float _recommendedPower;
    [SerializeField] private int _jelly;
    
    [Header("Wave 정보")]
    [Space(10)]
    [SerializeField] private WaveInfo[] _waveInfo;

    // 순서는 center1 => left1 1~5 => right1 1~5
    //        center2 => left2 1~5 => right2 1~5
    //        center3 => left3 1~5 => right3 1~5
    [Header("맵 정보")]
    [Space(10)]
    [Header("타일")]
    [SerializeField] private Sprite[] _mapSprites;
    [SerializeField] private Transform _stagePos;
    [Header("왼쪽 장애물")]
    [SerializeField] private Sprite[] _closeLeftObjectSprites;
    [SerializeField] private Sprite[] _middleLeftObjectSprites;
    [SerializeField] private Sprite[] _farLeftObjectSprites;
    [Header("오른쪽 장애물")]
    [SerializeField] private Sprite[] _closeRightObjectSprites;
    [SerializeField] private Sprite[] _middleRightObjectSprites;
    [SerializeField] private Sprite[] _farRightObjectSprites;

    [Header("보상 정보")]
    [Space(10)]
    [SerializeField] private ItemBundle[] _victoryRewardItems;
    [SerializeField] private ItemBundle[] _defeatRewardItems;

    public bool IsBoss => _isBoss;
    public SkeletonAnimation BossSkeletonAnimation => _bossSkeletonAnimation;
    public string StageName => _stageName;
    public float RecommendedPower => _recommendedPower;
    public int Jelly => _jelly;
    public WaveInfo[] WaveInfo => _waveInfo;
    public Sprite[] MapSprites => _mapSprites;
    public Transform StagePos => _stagePos;

    public Sprite[] CloseLeftObjectSprites => _closeLeftObjectSprites;
    public Sprite[] MiddleLeftObjectSprites => _middleLeftObjectSprites;
    public Sprite[] FarLeftObjectSprites => _farLeftObjectSprites;

    public Sprite[] CloseRightObjectSprites => _closeRightObjectSprites;
    public Sprite[] MiddleRightObjectSprites => _middleRightObjectSprites;
    public Sprite[] FarRightObjectSprites => _farRightObjectSprites;

    public ItemBundle[] VictoryRewardItems => _victoryRewardItems;
    public ItemBundle[] DefeatRewardItems => _defeatRewardItems;
}
