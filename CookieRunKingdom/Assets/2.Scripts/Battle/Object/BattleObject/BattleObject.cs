using Spine.Unity;
using UnityEngine;

public class BattleObject : MonoBehaviour
{
    // 상태 관련 변수
    [Header("Status")]
    [SerializeField]
    private bool _isEnemy = false;
    public bool IsEnemy
    {
        get { return _isEnemy; }
        set { _isEnemy = value; }
    }

    // 캐릭터 데이터
    [SerializeField]
    protected CharacterData _characterData;

    [SerializeField]
    private Status _curStatus = Status.None;
    public Status CurStatus
    {
        get { return _curStatus; }
    }
    public enum Status
    {
        None,
        Idle,
        Attack,
        Run,
        Defend,
        Skill
    }

    // 체력 관련 변수
    [Header("Health")]
    [SerializeField]
    protected float _hp;
    protected float _maxHp;
    public float HP
    {
        get { return _hp; }
    }

    // 캐릭터 이동 관련 변수
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 2f;

    [Header("KnockBack")]
    // 넉백 관련 변수
    private bool _isKnockedBack = false;
    private float _knockBackDuration = 0.2f;
    private float _knockBackTimer = 0f;
    private Vector3 _knockBackDirection;

    // 타겟 관련 변수
    [Header("Target")]
    [SerializeField]
    protected GameObject _target;
    [SerializeField]
    private float _targetDistance;

    // 애니메이션 관련 변수
    [Header("Animation")]
    private SkeletonAnimation _skeletonAni;
    protected CharacterAnimation _characterAni;

    // HP 바 관련 변수
    [Header("HP Bar")]
    private HPBarController _hpBarController;
    [SerializeField]
    private GameObject _hpBarPrefab;

    // 공격 쿨다운 관련 변수
    [Header("Attack Cooldown")]
    private float _attackCooldownTimer = 0f;

    [Header("AttackRange")]
    [SerializeField]
    private float _attackRange = 2f;
    public float AttackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }

    [Header("Heal")]
    private GameObject _healEffect;

    private void Awake()
    {
        _skeletonAni = GetComponent<SkeletonAnimation>();

        //HealEffect
        _healEffect = Resources.Load<GameObject>("Prefabs/Battle/Skill/HealEffect");
        _healEffect = Instantiate(_healEffect, transform.position, Quaternion.identity);
        _healEffect.SetActive(false);

        // HPBar
        _hpBarPrefab = Resources.Load<GameObject>("Prefabs/Battle/HPBar");
        GameObject hpBarInstance = Instantiate(_hpBarPrefab, transform.position, Quaternion.identity);
        hpBarInstance.transform.SetParent(GameObject.Find("HPBars").transform, false);
        _hpBarController = hpBarInstance.GetComponent<HPBarController>();
    }

    // Start
    protected void Start()
    {
        Debug.Log("Character Initialized: " + _characterData.Name);
        _characterAni = gameObject.AddComponent<CharacterAnimation>();
        _characterAni.Init(_characterData, _skeletonAni);
        _characterAni.OnAttackEnd += AttackAniEnd;

        _hpBarController.SetTarget(this.gameObject, _isEnemy);

        _maxHp = _characterData.Hp;
        _hp = _maxHp;

        SetStatus(Status.Idle);
    }

    // Update
    protected void Update()
    {
        if (_target != null)
        {
            if (_isEnemy)
            {
                if (_isKnockedBack)
                    MoveKnockBackPos();

                if (_curStatus == Status.Run)
                    MoveToTarget();
            }
            SetTarget();
        }
        UpdateStatus();
    }

    //Target세팅 
    private void SetTarget()
    {
        if (_target && _target.activeSelf == true)
        {
            float distance = GetDistanceToTarget();

            if (distance <= _attackRange)
                return;
        }

        BattleManager.Instance.SetTargetObj(gameObject, _isEnemy);
    }

    //타겟과의 거리를 얻음
    private float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _target.transform.position);
    }

    //넉백 이동
    private void MoveKnockBackPos()
    {
        _knockBackTimer -= Time.deltaTime;

        if (_knockBackTimer > 0)
        {
            transform.parent.Translate(_knockBackDirection * Time.deltaTime);
        }
        else
        {
            _isKnockedBack = false;
        }
    }

    //Status을 체크
    private void UpdateStatus()
    {
        if (_curStatus == Status.Skill) return;

        if (_target && _target.activeSelf == true)
        {
            _targetDistance = GetDistanceToTarget();

            if (_targetDistance <= _attackRange)
            {
                SetStatus(Status.Attack);
            }
            else if (_curStatus != Status.Run)
            {
                SetStatus(Status.Run);
            }
        }
    }

    //Status를 세팅
    protected void SetStatus(Status newStatus)
    {
        if (_curStatus == newStatus) return;

        _curStatus = newStatus;

        //Status
        _characterAni.PlayAni("Battle", _curStatus.ToString());
    }

    // Set
    public void SetTarget(GameObject newTarget)
    {
        _target = newTarget;
    }

    public void SetCharacterData(CharacterData newCharacterData)
    {
        _characterData = newCharacterData;
    }

    private void MoveToTarget()
    {
        if (BattleManager.Instance.IsStop) return;

        if (_target != null)
        {
            Vector3 direction = (_target.transform.position - transform.parent.position).normalized;
            transform.parent.position += direction * moveSpeed * Time.deltaTime * BattleUIManager.Instance.StageSpeed;
        }
    }

    private void Attack()
    {
        switch (_characterData.AttackType)
        {
            case AttackType.Melee:
                MeleeAttack();
                break;
            case AttackType.Ranged:
                RangedAttack();
                break;
            case AttackType.Heal:
                HealAttack();
                break;
        }
    }

    private void MeleeAttack()
    {
        Debug.Log("Performing Melee Attack");
        if (_target != null)
        {
            _target.GetComponent<BattleObject>().Damage(CalculateDamage(_characterData.AttackDamage));
        }
        _attackCooldownTimer = _characterData.AttackInterval;
    }

    private void RangedAttack()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            GameObject bulletInstance = Instantiate(_characterData.BulletObject, transform.position, Quaternion.identity);
            Bullet bullet = bulletInstance.GetComponent<Bullet>();
            bullet.SetTarget(_target, CalculateDamage(_characterData.AttackDamage), direction);
        }
    }

    private void HealAttack()
    {
        BattleObject _healTarget = BattleManager.Instance.FindLowestHpCookie();
        _healTarget.Heal(CalculateDamage(_characterData.AttackDamage));
    }

    private float CalculateDamage(float baseDamage)
    {
        float damage = baseDamage + _characterData.Attack;
        // 치명타 확률 계산
        if (Random.value < _characterData.Critical / 100)
        {
            Debug.Log("Critical Hit!");
            damage *= 2;
        }
        return damage;
    }

    private void AttackAniEnd()
    {
        if (BattleManager.Instance.IsStop) return;

        if (_target != null && _target.activeSelf && _curStatus == Status.Attack)
        {
            CheckIsBattle();
            Attack();
        }
    }

    protected void CheckIsBattle()
    {
        if (!BattleManager.Instance.IsOnBattle )
            BattleManager.Instance.IsOnBattle = true;
    }

    public void Heal(float value)
    {
        _hp += value;

        if (_hp > _maxHp)
            _hp = _maxHp;

        _healEffect.SetActive(true);
    }

    public void Damage(float damage)
    {
        float actualDamage = damage - _characterData.Defence;
        actualDamage = Mathf.Max(actualDamage, 0); // 데미지가 0보다 작지 않도록 설정

        _hp -= actualDamage;

        _hpBarController.SetHP(_hp, _maxHp);

        if (_hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 캐릭터가 죽었을 때
        Debug.Log("Character died");

        _hp = 0;

        BattleManager.Instance.UpdateKillBattleInfo(_isEnemy);

        gameObject.SetActive(false);
    }

    private void KnockBack() // 넉백
    {
        _knockBackDirection = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y * 0.5f, 0) * 50f;
        _isKnockedBack = true;
        _knockBackTimer = _knockBackDuration;
    }
}