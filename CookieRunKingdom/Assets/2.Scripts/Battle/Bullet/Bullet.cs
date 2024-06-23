using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f; // 총알 이동 속도
    private Vector3 _direction; // 총알 이동 방향

    [SerializeField] private float _maxDistance = 10f; // 최대 이동 거리
    private float _currentDistance = 0f; // 현재 이동 거리

    private GameObject _target; // 타겟 객체
    private float _damageAmount = 10f; // 데미지 양
    private float _collisionDistance = 1.0f; // 충돌 거리

    private GameObject _particle;

    // 타겟 설정 메서드
    public void SetTarget(GameObject target, float damage, Vector3 direction)
    {
        _direction = direction.normalized;
        _target = target;
        _damageAmount = damage;

        _particle = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        // 타겟이 없으면 리턴
        if (_target == null)
        {
            return;
        }

        // 총알을 방향으로 이동
        transform.position += _direction * _speed * Time.deltaTime;
     //   _particle.transform.position += _direction * _speed * Time.deltaTime;

        // 최대 이동 거리를 넘으면 파괴
        /*        if (_currentDistance >= _maxDistance)
                {
                    Destroy(gameObject);
                }*/

        // 타겟이 없으면 리턴
        if (_target == null)
        {
            return;
        }

        // 타겟과의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        // 일정 거리 이내에 있으면 충돌 처리
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

        Destroy(gameObject); // 총알 파괴
    }
}
