using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BattleObject : MonoBehaviour
{
    [SerializeField]
    private CharacterData characterData;
    private SkeletonAnimation skeletonAnimation;
    private Status currentStatus = Status.Idle;
    private CharacterAnimation characterAnimation;

    public enum Status
    {
        Idle,
        Attack,
        Defend
    }

    [SerializeField]
    private Transform target;  // The target to attack
    [SerializeField]
    private float attackRange = 5f;  // The range within which the attack will start

    public void SetCharacterData(CharacterData newCharacterData)
    {
        characterData = newCharacterData;
    }

    private void Start()
    {
        Debug.Log("Character Initialized: " + characterData.Name);
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        characterAnimation = new CharacterAnimation();
        characterAnimation.Initialize(characterData, skeletonAnimation);

        SetStatus(Status.Attack);
    }

    private void Update()
    {
        if (target != null && currentStatus == Status.Idle)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                SetStatus(Status.Attack);
            }
        }
        // Handle other status-based behaviors here
    }

    private void SetStatus(Status newStatus)
    {
        currentStatus = newStatus;
        characterAnimation.PlayAnimation("Battle", currentStatus.ToString());

        switch (currentStatus)
        {
            case Status.Idle:
                // Handle idle behavior
                break;
            case Status.Attack:
                AttackTarget();
                break;
            case Status.Defend:
                // Handle defending behavior
                break;
            // Add cases for other statuses as needed
            default:
                break;
        }
    }

    private void AttackTarget()
    {
        // Implement your attack logic here
        // Debug.Log("Attacking target: " + target.name);
        // Example: Reduce the target's HP, play attack animation, etc.
    }
}
