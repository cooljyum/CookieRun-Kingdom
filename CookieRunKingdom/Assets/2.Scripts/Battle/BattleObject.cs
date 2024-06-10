using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static UnityEngine.GraphicsBuffer;

public class BattleObject : MonoBehaviour
{
    [SerializeField]
    private bool _isEnemy = false;

    public bool IsEnemy 
    {
        get { return _isEnemy; }
        set { _isEnemy = value; }
    }

    private float _attackRange = 2f;

    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private CharacterData _characterData;

    [SerializeField]
    private Stats _stats;

    [System.Serializable]
    private struct Stats
    {
        [SerializeField]
        private float _hp;
        [SerializeField]
        private float _attack;
        [SerializeField]
        private float _defence;
        [SerializeField]
        private float _critical;

        public float Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }

        public float Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        public float Defence
        {
            get { return _defence; }
            set { _defence = value; }
        }

        public float Critical
        {
            get { return _critical; }
            set { _critical = value; }
        }
    }


    [SerializeField]
    private Transform _target;

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

    //Start
    private void Start()
    {
        Debug.Log("Character Initialized: " + _characterData.Name);
        _skeletonAni = GetComponent<SkeletonAnimation>();
        _characterAni = new CharacterAnimation();
        _characterAni.Init(_characterData, _skeletonAni);

        //SetStats
        _stats.Hp = _characterData.Hp;


       _characterAni.OnAttackComplete += AttackEnd;

        SetStatus(Status.Run);
    }

    //Update
    private void Update()
    {
        if (_target != null )
        {
            float distanceToTarget = Vector3.Distance(transform.position, _target.parent.position);
            if (distanceToTarget <= _attackRange)
            {
                SetStatus(Status.Attack);
            }
            else if (_curStatus != Status.Run)
            {
                SetStatus(Status.Run);
            }

            if (_isEnemy && _curStatus == Status.Run)
            {
                MoveTowardsTarget();
            }
        }
    }

    //Set
    public void SetTarget(Transform newTarget)
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
            Vector3 direction = (_target.position - transform.parent.position).normalized;
            transform.parent.position += direction * moveSpeed * Time.deltaTime;
        }
    }
    private void AttackEnd()
    {
        Debug.Log("Damage dealt to the target");
        if (_target != null)
        {
            _target.GetComponent<BattleObject>().Damege(_characterData.AttackDamage);
        }

    }

    public void Damege(float damege)
    {
        _characterData.Hp -= damege;
    }
    private void AttackTarget()
    {
        
    }
}
