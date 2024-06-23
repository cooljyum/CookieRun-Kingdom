using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f; // �Ѿ� �̵� �ӵ�
    private Vector3 _direction; // �Ѿ� �̵� ����

    [SerializeField] private float _maxDistance = 10f; // �ִ� �̵� �Ÿ�
    private float _currentDistance = 0f; // ���� �̵� �Ÿ�

    private GameObject _target; // Ÿ�� ��ü
    private float _damageAmount = 10f; // ������ ��
    private float _collisionDistance = 1.0f; // �浹 �Ÿ�

    private GameObject _particle;

    // Ÿ�� ���� �޼���
    public void SetTarget(GameObject target, float damage, Vector3 direction)
    {
        _direction = direction.normalized;
        _target = target;
        _damageAmount = damage;

        _particle = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        // Ÿ���� ������ ����
        if (_target == null)
        {
            return;
        }

        // �Ѿ��� �������� �̵�
        transform.position += _direction * _speed * Time.deltaTime;
     //   _particle.transform.position += _direction * _speed * Time.deltaTime;

        // �ִ� �̵� �Ÿ��� ������ �ı�
        /*        if (_currentDistance >= _maxDistance)
                {
                    Destroy(gameObject);
                }*/

        // Ÿ���� ������ ����
        if (_target == null)
        {
            return;
        }

        // Ÿ�ٰ��� �Ÿ� ���
        float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        // ���� �Ÿ� �̳��� ������ �浹 ó��
        if (distanceToTarget <= _collisionDistance)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        BattleObject target = _target.GetComponent<BattleObject>();
        if (target != null)
        {
            target.Damage(_damageAmount);
        }

        Destroy(gameObject); // �Ѿ� �ı�
    }
}
