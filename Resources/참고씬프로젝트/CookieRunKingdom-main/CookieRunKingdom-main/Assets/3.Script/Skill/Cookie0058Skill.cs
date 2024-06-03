using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 양파맛 쿠키
public class Cookie0058Skill : BaseMeleeSkill
{
    [SerializeField] private DetectRange _detectedSkillRange;
    [SerializeField] private DamageBox _cryingEffect;

    private DetectRange _cryingDetectRange;
    private SpriteRenderer _cryingRenderer;
    private float CurrentAnimationTime => _controller.CharacterAnimator.GetIntervalAnimation();

    public int _onionPower = 0;

    private int _skillIndex = 0;

    Sequence seq;

    public override void Init(BaseController controller)
    {
        base.Init(controller);
    }

    public override bool IsReadyToUseSkill()
    {
        return _onionPower >= 1 && _detectedSkillRange.enemies.Count != 0;
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    public override void NormalAttackEvent()
    {
        _onionPower++;

        float time = CurrentAnimationTime / 2;
        _cryingEffect.gameObject.SetActive(false);
        _cryingEffect.SetPower(0);

        // 투명하게 하기
        Color color = _cryingRenderer.color;
        color.a = 0;
        _cryingRenderer.color = color;

        // 효과
        seq.Kill();
        seq = DOTween.Sequence();
        seq.OnStart(() => _cryingEffect.gameObject.SetActive(true))
            .Append(_cryingRenderer.DOFade(1, time))
            .Append(_cryingRenderer.DOFade(0, time))
            .OnComplete(() => _cryingEffect.gameObject.SetActive(false));
    }

    public override void OnSkillEvent(int index)
    {
        base.OnSkillEvent(index);

        if(index == 0)
        {
            float time = CurrentAnimationTime;
            _cryingEffect.gameObject.SetActive(false);
            _cryingEffect.transform.localScale = Vector3.zero;
            _cryingEffect.SetPower(AttackPower * _onionPower);

            // 투명하게 하기
            Color color = _cryingRenderer.color;
            color.a = 0;
            _cryingRenderer.color = color;


            // 효과
            seq.Kill();
            seq = DOTween.Sequence();
            seq.OnStart(() => _cryingEffect.gameObject.SetActive(true))
                .Append(_cryingRenderer.DOFade(1, time / 3))
                .Join(_cryingEffect.transform.DOScale(25, time))
                .OnComplete(() =>
                {
                    _cryingEffect.gameObject.SetActive(false);
                    _cryingEffect.transform.localScale = Vector3.one * 1.5f;
                });


            _onionPower = 0;
        }
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);

        _cryingRenderer = _cryingEffect.GetComponent<SpriteRenderer>();
        _cryingDetectRange = _cryingEffect.GetComponent<DetectRange>();

        _cryingEffect.Init(0, layer,_controller.CharacterStat, true);
        _detectedSkillRange.Init(layer);
        _cryingDetectRange.Init(layer);
    }

    public override bool UseSkill()
    {
        if(_skillIndex == 0)
        {
            PlayAnimation(animationName[_skillIndex++], false);
        }
        else if(_skillIndex == 1)
        {
            if (!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                _skillIndex = 0;
                return false;
            }
        }

        return true;
    }
}
