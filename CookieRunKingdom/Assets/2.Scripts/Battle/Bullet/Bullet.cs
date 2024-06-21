using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f; // �Ѿ� �̵� �ӵ�
    private Vector3 _direction; // �Ѿ� �̵� ����

    [SerializeField] private float _maxDistance = 10f; // �ִ� �̵� �Ÿ�
    private float _curDistance = 0f; // ���� �̵� �Ÿ�

    private GameObject _target; // Ÿ��

    private float _Damage;

    // ���� ���� �޼���
    public void SetDirection(Vector3 dir)
    {
        _direction = dir.normalized;
    }

    private void Update()
    {
        // �Ѿ��� �������� �̵�
        transform.position += _direction * _speed * Time.deltaTime;

        // �̵� �Ÿ� ���
        _curDistance += _speed * Time.deltaTime;

        // �ִ� �̵� �Ÿ��� ������ �ı�
        if (_curDistance >= _maxDistance)
        {
            Destroy(gameObject);
        }

        // Ÿ�� ���� ���� (���� �Ÿ� ���� �ִ��� üũ)
        DetectTarget();
    }

    private void DetectTarget()
    {
        if (_target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

            if (distanceToTarget <= 1.0f) 
            {
                HitTarget();
            }
        }
    }

    private void HitTarget()
    {
        BattleObject target = _target.GetComponent<BattleObject>();
        if (target != null)
        {
            target.Damage(10f); // ���÷� �������� 10���� ����
        }

        Destroy(gameObject); // �Ѿ� �ı�
    }
}
