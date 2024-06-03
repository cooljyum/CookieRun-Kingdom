using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CharacterBattleController : MonoBehaviour
{
    public System.Action OnDeadEvent = null;
    public System.Action OnHitEvent = null;

    [SerializeField] private HealEffect _healEffect;

    private Slider _hpBar = null;
    private bool _isDead = false;
    public bool IsDead => _isDead;

    public int MaxHp => _controller.CharacterStat.hpStat.ResultStat;

    private int _currentHp;
    public int CurrentHp
    {
        get { return _currentHp; }
        set
        {
            _currentHp = value;
            _currentHp = Mathf.Clamp(_currentHp, 0, MaxHp);

            UpdateHpbar();
            OnHitEvent?.Invoke();

            if (_currentHp <= 0)
                if(!_isDead)
                    Dead();
        }
    }

    // 쿠키들 원래 위치 (전투중이 아닐 때는 이 위치를 고수해야 함)
    public CookieBundle CookieBundle { get; private set; }
    public Transform OffsetPosition { get; private set; }
    public LayerMask myLayer { get; private set; }
    public LayerMask enemyLayer { get; private set; }

    public ECCType CurrentCCType { get; set; }

    protected BaseController _controller;
    protected BattleStateFactory _factory;
    private Coroutine _coUpdate = null;

    private HpBarController _hpBarController;
    private DamageTextController _damageTextController;
    private Camera _camera;

    public bool IsForward { get; private set; }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Init(BaseController controller)
    {
        _isDead = false;

        _hpBarController = FindObjectOfType<HpBarController>();
        _damageTextController = FindObjectOfType<DamageTextController>();
        _camera = Camera.main;

        _controller = controller;
    }

    public void SetEnemy(LayerMask layer)
    {
        if(layer == LayerMask.NameToLayer("Enemy"))
        {
            myLayer = LayerMask.NameToLayer("Cookie");
            enemyLayer = LayerMask.NameToLayer("Enemy");
        }
        else if(layer == LayerMask.NameToLayer("Cookie"))
        {
            myLayer = LayerMask.NameToLayer("Enemy");
            enemyLayer = LayerMask.NameToLayer("Cookie");
        }

        _controller.BaseSkill.SetLayer(layer);
    }

    public void SetPosition(CookieBundle cookieBundle, Transform pos)
    {
        CookieBundle = cookieBundle;
        OffsetPosition = pos;
    }

    /// <summary>
    /// 전투 시작시 실행하는 메소드
    /// </summary>
    /// <param name="isForward">앞을 향하고 있나</param>
    public void StartBattle(bool isForward = true)
    {
        CurrentHp = MaxHp;

        IsForward = isForward;
        _controller.CharacterAnimator.AdjustmentAnimationName(isForward);

        _factory = new BattleStateFactory(_controller);

        if (_coUpdate != null)
            StopCoroutine(_coUpdate);
        _coUpdate = StartCoroutine(CoUpdate());
    }

    public bool CheckState(EBattleState state)
    {
        return _factory.CheckState(state);
    }

    public void ChangeState(EBattleState state)
    {
        _factory.ChangeState(state);
    }

    public void SetCC(ECCType ccType, Vector3 dir)
    {
        if (_isDead)
            return;

        _factory.BattleCC.SetCC(ccType, dir);
        _factory.ChangeState(EBattleState.BattleCrowdControlState);
    }


    private void UpdateHpbar()
    {
        if (_hpBar == null && _currentHp < MaxHp)
        {
            _hpBar = _hpBarController.GetHpBar(IsForward);
        }
        else if (_hpBar != null && _currentHp == MaxHp)
        {
            _hpBar.gameObject.SetActive(false);
            _hpBar = null;
        }

        if (_hpBar != null)
            _hpBar.value = (float)CurrentHp / MaxHp;
    }

    private IEnumerator CoUpdate()
    {
        while (true)
        {
            yield return null;

            _factory.CurrentState.Update();
            float zPos = (transform.position.x + transform.position.y) / 1000f;
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos);

            if (_hpBar != null)
            {
                _hpBar.transform.position = _camera.WorldToScreenPoint(transform.position + Vector3.up * 2);
            }
        }
    }

    protected virtual void Dead()
    {
        OnDeadEvent?.Invoke();

        _factory.ChangeState(EBattleState.BattleDeadState);
        _hpBar.gameObject.SetActive(false);
        _hpBar = null;
        _isDead = true;
    }


    public void ChangeCurrentHp(int value, CharacterStat _character)
    {
        if (_isDead)
            return;

        int critical = _character.criticalStat.ResultStat;
        int random = Random.Range(0, 100);
        if (random <= critical)
            value *= 2;


        if (value < 0)
            value += _character.defenseStat.ResultStat;


        TextMeshProUGUI damageText = _damageTextController.GetDamageText(value, random <= critical, value > 0);
        if(damageText != null)
        {
            if (random <= critical || value > 0)
                damageText.transform.position = _camera.WorldToScreenPoint(transform.position + Vector3.up * 2.2f);
            else
                damageText.transform.position = _camera.WorldToScreenPoint(transform.position + Vector3.up * 2);

            damageText.transform.SetAsFirstSibling();
            Sequence seq = DOTween.Sequence();
            seq.Append(damageText.transform.DOMoveY(damageText.transform.position.y + 10f, 0.5f))
                .SetEase(Ease.OutQuart)
                .Join(damageText.DOFade(0.5f, 0.5f))
                .OnComplete(() =>
                {
                    damageText.gameObject.SetActive(false);
                });
        }

        if (value > 0)
        {
            _healEffect.gameObject.SetActive(false);
            _healEffect.gameObject.SetActive(true);
        }

        CurrentHp += value;
    }

    public virtual void Disappear()
    {

    }
}
