using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

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

    private float _attackCooldownTimer = 0f;

    // Start
    private void Start()
    {
        Debug.Log("Character Initialized: " + _characterData.Name);
        _skeletonAni = GetComponent<SkeletonAnimation>();
        _characterAni = new CharacterAnimation();
        _characterAni.Init(_characterData, _skeletonAni);
        _characterAni.OnAttackComplete += AttackEnd;
        SetStatus(Status.Run);
    }

    // Update
    private void Update()
    {
        if (_target != null)
        {
            if (_isKnockedBack && _isEnemy)
            {
                HandleKnockBack();
            }
            else
            {
                HandleMovementAndAttack();
            }
            BattleManager.Instance.SetTargetObj(gameObject, _isEnemy);
        }

    }

    private void HandleKnockBack()
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

    private void HandleMovementAndAttack()
    {
        _targetDistance = Vector3.Distance(transform.position, _target.transform.parent.position);
       // if (_targetDistance <= _attackRange && _attackCooldownTimer <= 0)
        if (_targetDistance <= _attackRange )
        {
            SetStatus(Status.Attack);
        }
        else if (_curStatus != Status.Run)
        {
            SetStatus(Status.Run);
            _attackCooldownTimer = 0;
        }

        if (_isEnemy && _curStatus == Status.Run)
        {
            MoveTowardsTarget();
        }

        if (_curStatus == Status.Attack)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }
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

    private void SetStatus(Status newStatus)
    {
        if (_curStatus == newStatus) return;
        _curStatus = newStatus;
        _characterAni.PlayAnimation("Battle", _curStatus.ToString());

        switch (_curStatus)
        {
            case Status.Idle:
                break;
            case Status.Run:
                break;
            case Status.Attack:
                PerformAttack();
                break;
            case Status.Defend:
                break;
            default:
                break;
        }
    }

    private void MoveTowardsTarget()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.transform.position - transform.parent.position).normalized;
            transform.parent.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void PerformAttack()
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
        // 치명타 확률 계산
        if (Random.value < _characterData.Critical / 100)
        {
            Debug.Log("Critical Hit!");
            damage *= 2;
        }
        return damage;
    }

    private void AttackEnd()
    {
        Debug.Log("Damage dealt to the target");
        if (_target != null&& _curStatus  == Status.Attack)
        {
            _target.GetComponent<BattleObject>().Damage(CalculateDamage(_characterData.AttackDamage));
        }

        // Set attack cooldown timer here
        _attackCooldownTimer = _characterData.AttackInterval;
    }

    public void Damage(float damage)
    {
        float actualDamage = damage - _characterData.Defence;
        actualDamage = Mathf.Max(actualDamage, 0); // 데미지가 0보다 작지 않도록 설정

        _characterData.Hp -= actualDamage;

        if (_characterData.Hp <= 0)
        {
            // 캐릭터가 죽었을 때
            Debug.Log("Character died");
        }

        KnockBack();

    }

    private void KnockBack()
    {
        _knockBackDirection = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y * 0.5f, 0) * 50f;
        _isKnockedBack = true;
        _knockBackTimer = _knockBackDuration;
    }
}
