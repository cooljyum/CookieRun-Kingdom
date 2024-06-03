using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [Header("데이터")]
    [SerializeField] private CharacterData _data;

    // 컴포넌트
    protected CharacterAnimator _characterAnimator;
    protected CharacterBattleController _characterBattleController;
    protected BaseSkill _baseSkill;
    protected CharacterStat _characterStat;

    public CharacterData Data => _data;
    public CharacterAnimator CharacterAnimator => _characterAnimator;
    public CharacterBattleController CharacterBattleController => _characterBattleController;
    public BaseSkill BaseSkill => _baseSkill;
    public CharacterStat CharacterStat => _characterStat;

    protected virtual void Awake()
    {
        _characterStat = GetComponent<CharacterStat>();
        _characterAnimator = GetComponent<CharacterAnimator>();
        _characterBattleController = GetComponent<CharacterBattleController>();
        _baseSkill = GetComponent<BaseSkill>();

        _characterStat.Init(this);
        _characterAnimator.Init(this);
        _characterBattleController.Init(this);
        _baseSkill.Init(this);
    }
}
