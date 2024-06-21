using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f; // 총알 이동 속도
    private Vector3 _direction; // 총알 이동 방향

    [SerializeField] private float _maxDistance = 10f; // 최대 이동 거리
    private float _curDistance = 0f; // 현재 이동 거리

    private GameObject _target; // 타겟

    private float _Damage;

    // 방향 설정 메서드
    public void SetDirection(Vector3 dir)
    {
        _direction = dir.normalized;
    }

    private void Update()
    {
        // 총알을 방향으로 이동
        transform.position += _direction * _speed * Time.deltaTime;

        // 이동 거리 계산
        _curDistance += _speed * Time.deltaTime;

        // 최대 이동 거리를 넘으면 파괴
        if (_curDistance >= _maxDistance)
        {
            Destroy(gameObject);
        }

        // 타겟 감지 로직 (일정 거리 내에 있는지 체크)
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
            target.Damage(10f); // 예시로 데미지를 10으로 설정
        }

        Destroy(gameObject); // 총알 파괴
    }
}
