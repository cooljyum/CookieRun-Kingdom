using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;  // Assuming you are using Spine for the SkeletonDataAsset

public class BattleObject : MonoBehaviour
{
    [SerializeField]
    private CharacterData characterData;

    private Status status;

    [SerializeField]
    private Transform target;  // The target to attack
    [SerializeField]
    private float attackRange = 5f;  // The range within which the attack will start

    private struct Status
    {
        private float _attack;
        private float _hp;
        private float _defence;
        private float _critical;

        //Set
        public Status(CharacterData characterData)
        {
            _attack = characterData.Attack;
            _hp = characterData.Hp;
            _defence = characterData.Defence;
            _critical = characterData.Critical;
        }

        //Get
        public float Attack => _attack;
        public float Hp => _hp;
        public float Defence => _defence;
        public float Critical => _critical;
    }

    public void SetCharacterData(CharacterData newCharacterData)
    {
        characterData = newCharacterData;
        status = new Status(characterData);
    }

    private void Start()
    {
        Debug.Log("Character Initialized: " + characterData.Name);
        Debug.Log("Attack: " + status.Attack);
        Debug.Log("HP: " + status.Hp);
        Debug.Log("Defence: " + status.Defence);
        Debug.Log("Critical: " + status.Critical);
    }

    private void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                AttackTarget();
            }
        }
    }

    private void AttackTarget()
    {
        // Implement your attack logic here
        Debug.Log("Attacking target: " + target.name);
        // Example: Reduce the target's HP, play attack animation, etc.
    }
}
