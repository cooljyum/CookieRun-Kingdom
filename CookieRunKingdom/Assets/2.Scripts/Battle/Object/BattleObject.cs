using Spine.Unity;
using UnityEngine;

public class BattleObject : MonoBehaviour
{
    // ���� ���� ����
    [Header("Status")]
    [SerializeField]
    private bool _isEnemy = false;
    public bool IsEnemy
    {
        get { return _isEnemy; }
        set { _isEnemy = value; }
    }

    // ĳ���� ������
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

    // ü�� ���� ����
    [Header("Health")]
    [SerializeField]
    protected float _hp;
    protected float _maxHp;

    // ĳ���� �̵� ���� ����
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 2f;

    [Header("KnockBack")]
    // �˹� ���� ����
    private bool _isKnockedBack = false;
    private float _knockBackDuration = 0.2f;
    private float _knockBackTimer = 0f;
    private Vector3 _knockBackDirection;

    // Ÿ�� ���� ����
    [Header("Target")]
    [SerializeField]
    protected GameObject _target;
    [SerializeField]
    private float _targetDistance;

    // �ִϸ��̼� ���� ����
    [Header("Animation")]
    private SkeletonAnimation _skeletonAni;
    protected CharacterAnimation _characterAni;

    // HP �� ���� ����
    [Header("HP Bar")]
    private HPBarController _hpBarController;
    [SerializeField]
    private GameObject _hpBarPrefab;

    // ���� ��ٿ� ���� ����
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
    private void Awake()
    {
        _skeletonAni = GetComponent<SkeletonAnimation>();

        // Resources �������� HPBar ������ �ε�
        _hpBarPrefab = Resources.Load<GameObject>("Prefabs/Battle/HPBar");

        // HPBar �������� �����ϰ� HPBarController ����
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
    
    //Target���� 
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

    //Ÿ�ٰ��� �Ÿ��� ����
    private float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _target.transform.position);
    }

    //�˹� �̵�
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

    //Status�� üũ
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

    //Status�� ����
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
    }

/*    private void MeleeAttack()
    {
        Debug.Log("Performing Melee Attack");
        if (_target != null)
        {
            _target.GetComponent<BattleObject>().Damage(CalculateDamage(_characterData.AttackDamage));
        }
        _attackCooldownTimer = _characterData.AttackInterval;
    }*/

/*    private void RangedAttack()
    {
        Debug.Log("Performing Ranged Attack");
        if (_target != null)
        {
            // Implement ranged attack logic here
            _target.GetComponent<BattleObject>().Damage(CalculateDamage(_characterData.AttackDamage));
        }
        _attackCooldownTimer = _characterData.AttackInterval;
    }

    private void MagicalAttack()
    {
        Debug.Log("Performing Magical Attack");
        if (_target != null)
        {
            // Implement magical attack logic here
            _target.GetComponent<BattleObject>().Damage(CalculateDamage(_characterData.AttackDamage));
        }
        _attackCooldownTimer = _characterData.AttackInterval;
    }*/

    private float CalculateDamage(float baseDamage)
    {
        float damage = baseDamage + _characterData.Attack;
        // ġ��Ÿ Ȯ�� ���
        if (Random.value < _characterData.Critical / 100)
        {
            Debug.Log("Critical Hit!");
            damage *= 2;
        }
        return damage;
    }

    private void AttackAniEnd()
    {
        // if (_characterData.Skill.IsPlay) return;

        if (BattleManager.Instance.IsStop) return;

        CheckIsBattle();

        if (_target != null && _curStatus == Status.Attack)
        {
            if (_isEnemy)
            {
                Debug.Log("Enemy Attack End" + _target.ToString());
                _target.GetComponent<BattleCookie>().Damage(CalculateDamage(_characterData.AttackDamage));
            }
            else { 
                _target.GetComponent<BattleObject>().Damage(CalculateDamage(_characterData.AttackDamage));
            }
        }
    }

    protected void CheckIsBattle() 
    {
        if (!BattleManager.Instance.IsOnBattle)
            BattleManager.Instance.IsOnBattle = true;
    }

    public void Damage(float damage)
    {
        float actualDamage = damage - _characterData.Defence;
        actualDamage = Mathf.Max(actualDamage, 0); // �������� 0���� ���� �ʵ��� ����

        _hp -= actualDamage;

        _hpBarController.SetHP(_hp, _maxHp);

        if (_hp <= 0)
        {
            Die();
        }
    }

    private void Die() 
    {
        // ĳ���Ͱ� �׾��� ��
        Debug.Log("Character died");

        _hp = 0;

        BattleManager.Instance.UpdateKillBattleInfo(_isEnemy);

        gameObject.SetActive(false);
    }

    private void KnockBack() // �˹�
    {
        _knockBackDirection = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y * 0.5f, 0) * 50f;
        _isKnockedBack = true;
        _knockBackTimer = _knockBackDuration;
    }
}
