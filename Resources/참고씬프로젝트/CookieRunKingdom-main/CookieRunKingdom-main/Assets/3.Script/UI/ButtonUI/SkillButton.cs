using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SkillButton : MonoBehaviour
{
    private BaseController _cookie;
    private Sprite _idleSprite;
    private Sprite _skillSprite;

    private bool isSkillUse = false;

    [SerializeField] private Image _buttonImage;
    [SerializeField] private Image _coolTimeImage;
    [SerializeField] private TextMeshProUGUI _coolTimeText; 
    [SerializeField] private Slider _hpBar;

    private float _coolTime = 5f;

    private float currentTime;
    public float CurrentTime
    {
        get { return currentTime; }
        set
        {
            currentTime = value;
            currentTime = Mathf.Clamp(currentTime, 0, 1000);

            if (currentTime != 0)
            {
                _coolTimeText.text = ((int)currentTime).ToString();
                _coolTimeImage.fillAmount = currentTime / _coolTime;
            }
            else
            {
                isSkillUse = false;
                _coolTimeText.gameObject.SetActive(false);
                _coolTimeImage.gameObject.SetActive(false);
            }
        }
    }

    private Sequence buttonClickSeq;
    private Sequence skillUseSeq;

    private CookieData _data;

    private void OnDestroy()
    {
        buttonClickSeq.Kill();
        skillUseSeq.Kill();
    }

    public void Init(BaseController cookie)
    {
        _cookie = cookie;
        _cookie.CharacterBattleController.OnDeadEvent += DisabledButton;
        _cookie.CharacterBattleController.OnHitEvent += UpdateHpBar;

        _data = (CookieData)cookie.Data;

        _coolTime = _data.SkillCoolTime;

        _idleSprite = _data.IdleSprite;
        _skillSprite = _data.SKillSprite;

        buttonClickSeq = DOTween.Sequence();
        buttonClickSeq.Pause().SetAutoKill(false).Append(transform.DOScale(1.07f, 0.08f))
            .Append(transform.DOScale(1f, 0.08f));

        skillUseSeq = DOTween.Sequence();
        skillUseSeq.Pause().SetAutoKill(false).Append(_buttonImage.transform.DOScaleX(-1, 0.05f))
            .AppendCallback(() =>
            {
                _buttonImage.sprite = _skillSprite;
            })
            .AppendInterval(1f)
            .Append(_buttonImage.transform.DOScaleX(1, 0.05f))
            .AppendCallback(() =>
            {
                _coolTimeText.gameObject.SetActive(true);
                _coolTimeImage.gameObject.SetActive(true);

                _buttonImage.sprite = _idleSprite;
                CurrentTime = _coolTime;
            });


        _buttonImage.sprite = _idleSprite;
        CurrentTime = _coolTime / 10f;
    }

    public void OnClickButton()
    {
        buttonClickSeq.Restart();

        // 쿨타임이 다 찼고, 스킬을 쓸 수 있는 경우 사용
        if(IsReadyToUse())
        {
            isSkillUse = true;
            skillUseSeq.Restart();
            _cookie.CharacterBattleController.ChangeState(EBattleState.BattleSkillState);
        }
    }

    public bool IsReadyToUse()
    {
        // 쿨타임이 다 찼고, 스킬을 쓸 수 있고, 그 쿠키가 죽지 않았을 경우 true 반환
        return !isSkillUse && _cookie.BaseSkill.IsReadyToUseSkill() && !_cookie.CharacterBattleController.IsDead;
    }

    private void Update()
    {
        if(CurrentTime != 0)
        {
            CurrentTime -= Time.deltaTime;
        }
    }

    private void DisabledButton()
    {
        _buttonImage.color = new Color(0.5f, 0.5f, 0.5f);
    }

    private void UpdateHpBar()
    {
        _hpBar.value = (float)_cookie.CharacterBattleController.CurrentHp / _cookie.CharacterBattleController.MaxHp;
    }
}
