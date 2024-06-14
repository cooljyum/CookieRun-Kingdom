using Spine.Unity;
using UnityEngine;

public class BattleObject : MonoBehaviour
{
    [SerializeField]
    private bool _isEnemy = false;

    public bool IsEnemy
    {
        get { return _isEnemy; }
        set { _isEnemy = value; }
    }

    [SerializeField]
    private float _attackRange = 2f;
    public float AttackRange 
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }

    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private CharacterData _characterData;

    private bool _isKnockedBack = false;
    private float _knockBackDuration = 0.2f;
    private float _knockBackTimer = 0f;
    private Vector3 _knockBackDirection;

    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _targetDistance;

    [SerializeField]
    private Status _curStatus = Status.Idle;

    public Status CurStatus
    {
        get { return _curStatus; }
    }

    public enum Status
    {
        Idle,
        Attack,
        Run,
        Defend
    }

    private SkeletonAnimation _skeletonAni;
    private CharacterAnimation _characterAni;

    private HPBarController _hpBarController;

    [SerializeField]
    private GameObject _hpBarPrefab;

    private float _attackCooldownTimer = 0f;

    private float _maxHp;

    private void Awake()
    {
        _skeletonAni = GetComponent<SkeletonAnimation>();
        _characterAni = new CharacterAnimation();

        // Resources �������� HPBar ������ �ε�
        _hpBarPrefab = Resources.Load<GameObject>("Prefabs/Battle/HPBar");

        // HPBar �������� �����ϰ� HPBarController ����
        GameObject hpBarInstance = Instantiate(_hpBarPrefab, transform.position, Quaternion.identity);
        hpBarInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        
        _hpBarController = hpBarInstance.GetComponent<HPBarController>();
        _hpBarController.SetTarget(this.gameObject);
    }

    // Start
    private void Start()
    {
        Debug.Log("Character Initialized: " + _characterData.Name);
        _characterAni.Init(_characterData, _skeletonAni);
        _characterAni.OnAttackEnd += AttackAniEnd;

        SetStatus(Status.Run);

        _maxHp = _characterData.Hp;
    }

    private void OnEnable()
    {
        _hpBarController.SetHP(_characterData.Hp, _maxHp);
    }

    // Update
    private void Update()
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
        else
        {
            SetStatus(Status.Run);
        }
    }

    //Status�� ����
    private void SetStatus(Status newStatus)
    {
        if (_curStatus == newStatus) return;

        _curStatus = newStatus;

        switch (_curStatus)
        {
            case Status.Idle:
                break;
            case Status.Run:
                break;
            case Status.Attack:
                Attack();
                break;
            case Status.Defend:
                break;
            default:
                break;
        }

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
        if (_target != null)
        {
            Vector3 direction = (_target.transform.position - transform.parent.position).normalized;
            transform.parent.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void Attack()
    {
        //switch (_characterData.AttackType)
        //{
        //    case AttackType.Melee:
        //        MeleeAttack();
        //        break;
        //    case AttackType.Ranged:
        //        RangedAttack();
        //        break;
        //    case AttackType.Magical:
        //        MagicalAttack();
        //        break;
        //}
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
    }

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
        if (_target != null&& _curStatus  == Status.Attack)
        {
            _target.GetComponent<BattleObject>().Damage(CalculateDamage(_characterData.AttackDamage));
        }

       // _attackCooldownTimer = _characterData.AttackInterval;
    }

    public void Damage(float damage)
    {
        float actualDamage = damage - _characterData.Defence;
        actualDamage = Mathf.Max(actualDamage, 0); // �������� 0���� ���� �ʵ��� ����

        _characterData.Hp -= actualDamage;

        _hpBarController.SetHP(_characterData.Hp, _maxHp);

        if (_characterData.Hp <= 0)
        {
            Die();
        }
    }

    private void Die() 
    {
        // ĳ���Ͱ� �׾��� ��
        Debug.Log("Character died");

        _characterData.Hp = 0;

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
