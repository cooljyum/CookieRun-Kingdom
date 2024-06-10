using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private CharacterData _characterData;

    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _attackCooldownTimer = 0f;

    public void Init(CharacterData characterData, GameObject target)
    {
        _characterData = characterData;
        _target = target;
    }

    public void UpdateAttack()
    {
        if (_target != null)
        {
            HandleAttackCooldown();
        }
    }

    private void HandleAttackCooldown()
    {
        if (_attackCooldownTimer > 0)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }
    }

    public void PerformAttack()
    {
        if (_attackCooldownTimer <= 0)
        {
            switch (_characterData.AttackType)
            {
                case AttackType.Melee:
                    MeleeAttack();
                    break;
                case AttackType.Ranged:
                    RangedAttack();
                    break;
                case AttackType.Magical:
                    MagicalAttack();
                    break;
            }
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

    public float CalculateDamage(float baseDamage)
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
}
