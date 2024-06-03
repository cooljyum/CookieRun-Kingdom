using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string _characterName;
    [SerializeField] private AnimationData _animationData;

    [SerializeField] private SkeletonDataAsset _skeletonDataAsset;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;

    [Header("Stat")]
    [SerializeField] private int _hpStat = 10000;
    [SerializeField] private int _attackStat = 100;
    [SerializeField] private int _defenseStat = 20;
    [SerializeField] private int _criticalStat = 1;

    public string CharacterName => _characterName;
    public AnimationData AnimationData => _animationData;
    public SkeletonDataAsset SkeletonDataAsset => _skeletonDataAsset;
    public float MoveSpeed => moveSpeed;
    public float AttackRange => attackRange;

    public int Power => AttackStat + DefenseStat;
    public int HpStat => _hpStat;
    public int AttackStat => _attackStat;
    public int DefenseStat => _defenseStat;
    public int CriticalStat => _criticalStat;
}
