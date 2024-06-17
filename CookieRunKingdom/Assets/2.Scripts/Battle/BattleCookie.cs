using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCookie : BattleObject
{
    [Header("Battle Cookie Settings")]
    [SerializeField]
    private SkillBtn _skillBtn;

    // 공격 쿨다운 관련 변수
    [Header("Skill Cooldown")]
    [SerializeField]
    private float _skillCooldownTimer;
    [SerializeField]
    private float _skillMaxCooldownTimer;

    private void Start()
    {
        base.Start();

        GameObject skillBtn = BattleUIManager.Instance.AddSkillBtn();

        if (skillBtn != null)
        {
            _skillBtn = skillBtn.GetComponent<SkillBtn>();
            _skillBtn.SetImg(_characterData.SkillBtnImg);
        }

        _skillMaxCooldownTimer = _characterData.Skill.Cooldown;
        _skillCooldownTimer = _skillMaxCooldownTimer;

        GameObject skillEffectsParent = GameObject.Find("SkillEffects");
        if (skillEffectsParent != null && _characterData.Skill.EffectPrefab != null)
        {
            _characterData.Skill.EffectObj = Instantiate(_characterData.Skill.EffectPrefab, skillEffectsParent.transform);
        }
        else
        {
            Debug.LogWarning("SkillEffects parent object or Effect is missing!");
        }
    }

    private void Update()
    {
        base.Update();

        SkillTimer();
    }

    public void Damage(float damage)
    {
        base.Damage(damage);

        
        _skillBtn.SetHPBarUI(_hp/_maxHp);
    }

    private void SkillTimer() 
    {
        if (_skillCooldownTimer > 0)
        {
            _skillCooldownTimer -= Time.deltaTime;
            _skillCooldownTimer = Mathf.Max(0, _skillCooldownTimer); // 타이머가 음수가 되지 않도록
        }

        // 쿨다운 상태를 업데이트
        _skillBtn.SetWaitBgFillAmount(_skillCooldownTimer / _skillMaxCooldownTimer);

        // 쿨다운이 끝났다면 스킬을 사용할 수 있는 상태로 변경
        if (_skillCooldownTimer <= 0)
        {
            _skillBtn.SetState(SkillBtn.SkillBtnState.Ready);
        }
        else 
        {
            _skillBtn.SetState(SkillBtn.SkillBtnState.Wait);
        }
    }
}
