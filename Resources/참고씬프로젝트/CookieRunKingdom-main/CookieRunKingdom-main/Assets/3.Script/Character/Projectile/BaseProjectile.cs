using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(DamageBox), typeof(Rigidbody2D))]
public class BaseProjectile : MonoBehaviour
{
    public System.Action<BaseProjectile> OnDisableEvent = null;

    [SerializeField] protected bool isSelfRotate = false;  // 스스로 회전하면서 날아가냐?
    [SerializeField] protected bool isRotate = true;  // 타겟을 향하냐?
    [SerializeField] protected bool _isPersistence = false;  // 지속성이 있나?
    [SerializeField] protected bool _isTargetDir = false;    // 타겟을 향해 날아가나?
    [SerializeField] protected float _rotateSpeed = 3f;
    [SerializeField] protected float _moveSpeed = 8f;
    [SerializeField] protected float _duration;

    protected float _currentTime = 0f;
    protected Vector3 _dir;
    protected Vector3 _initPos = Vector3.up * 0.8f;
    protected Transform _parent = null;
    protected Rigidbody2D _rigid;
    protected DamageBox _damageBox;

    private Tweener _tween;

    /// <summary>
    /// 투사체의 정보를 초기화한다.
    /// </summary>
    /// <param name="damage">투사체의 데미지</param>
    /// <param name="targetLayer">투사체의 타겟 레이어</param>
    /// <param name="parent">해당 투사체의 부모</param>
    /// <param name="initPos">해당 투사체가 등장할 위치</param>
    public virtual void Init(int damage, LayerMask targetLayer, Transform parent, Vector3 initPos, CharacterStat characterStat)
    {
        _rigid = GetComponent<Rigidbody2D>();
        _damageBox = GetComponent<DamageBox>();
        _parent = parent;
        _initPos = initPos;

        if (_damageBox != null)
            _damageBox.Init(damage, targetLayer, characterStat, _isPersistence);
    }

    public void ShootProjectile(Vector3 dir)
    {
        _dir = dir;

        if(isSelfRotate)
        {
            _tween = transform.DORotate(new Vector3(0, 0, -360), _rotateSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);
        }

        if(isRotate)
            transform.localEulerAngles = Vector3.forward * (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg - 90);
    }

    private void OnDisable()
    {
        _tween.Pause();
        OnDisableEvent?.Invoke(this);
    }

    protected virtual void OnEnable()
    {
        transform.SetParent(_parent);
        _currentTime = 0;

        transform.localPosition = _initPos;
        transform.SetParent(null);

        _rigid.velocity = _dir * _moveSpeed;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= _duration)
        {
            gameObject.SetActive(false);
        }
    }
}
