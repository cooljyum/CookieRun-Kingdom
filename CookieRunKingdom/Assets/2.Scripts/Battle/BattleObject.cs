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

    [SerializeField]
    private float _attackRange = 1f;

    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private CharacterData _characterData;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Status _curStatus = Status.Idle;

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
        _characterAni.Initialize(_characterData, _skeletonAni);

        SetStatus(Status.Run);
    }

    //Update
    private void Update()
    {
        if (_target != null )
        {
            float distanceToTarget = Vector3.Distance(transform.position, _target.position);
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
        _curStatus = newStatus;
        _characterAni.PlayAnimation("Battle", _curStatus.ToString());

        switch (_curStatus)
        {
            case Status.Idle:
                break;
            case Status.Run:
                AttackTarget();
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
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void AttackTarget()
    {
        
    }
}
