using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BattleCookie : BattleObject
{
    [Header("Battle Cookie Settings")]
    [SerializeField]
    private SkillBtn _skillBtn;

    // ���� ��ٿ� ���� ����
    [Header("Skill Cooldown")]
    [SerializeField]
    private float _skillCooldownTimer;
    [SerializeField]
    private float _skillMaxCooldownTimer;

    private void Start()
    {
        base.Start();

        _characterAni.OnSkillEnd += SkillEnd;


        GameObject skillBtn = BattleUIManager.Instance.AddSkillBtn();

        if (skillBtn != null)
        {
            _skillBtn = skillBtn.GetComponent<SkillBtn>();
            _skillBtn.SetImg(_characterData.SkillBtnImg);
            _skillBtn.SetOwner(gameObject);
            _skillBtn.SetState(SkillBtn.SkillBtnState.Wait);
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
        if (BattleManager.Instance.IsStop) return;

        base.Update();

        SkillTimer();
    }

    public void Damage(float damage)
    {
        base.Damage(damage);

        _skillBtn.SetHPBarUI(_hp / _maxHp);
    }

    public void Skill()
    {
        SetStatus(Status.Skill);
        _characterData.Skill.UseSkill(gameObject, _target);
        BattleUIManager.Instance.SetSkillEffectUI(_characterData.ProfileImage.texture);
    }

    public void SkillEnd()
    {
        Debug.Log("Skill End!");

        CheckIsBattle();
        SetStatus(Status.Run);
        _skillBtn.SetState(SkillBtn.SkillBtnState.On);
        _skillCooldownTimer = _skillMaxCooldownTimer;
    }
    private void SkillTimer()
    {
        if (!BattleManager.Instance.IsOnBattle)
        {
            _skillBtn.SetState(SkillBtn.SkillBtnState.On);
            return;
        }

        if (_skillBtn.CurState == SkillBtn.SkillBtnState.Skill && _skillBtn.CurState == SkillBtn.SkillBtnState.Off) return;

        if (_skillCooldownTimer > 0)
        {
            _skillCooldownTimer -= Time.deltaTime;
            _skillCooldownTimer = Mathf.Max(0, _skillCooldownTimer); // Ÿ�̸Ӱ� ������ ���� �ʵ���
        }

        // ��ٿ� ���¸� ������Ʈ
        _skillBtn.SetWaitBgFillAmount(_skillCooldownTimer / _skillMaxCooldownTimer);

        // ��ٿ��� �����ٸ� ��ų�� ����� �� �ִ� ���·� ����
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
