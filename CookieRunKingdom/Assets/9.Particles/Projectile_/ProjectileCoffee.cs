using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class ProjectileCoffee : BaseProjectile
{
  //  private DetectRange _detectedRange;

    [SerializeField] private float timePerAttack = 0.3f;

    [SerializeField] private Transform _outterCircle;
    [SerializeField] private Transform _innerCircle;
    [SerializeField] private Transform _coffeeBean;

    private Coroutine _coRotate = null;
    private int _damage;
 //   private CharacterStat _character;

    public override void Init(int damage, LayerMask targetLayer, Transform parent, Vector3 initPos)//, CharacterStat characterStat)
    {
        base.Init(damage, targetLayer, parent, initPos);//, characterStat);

        _damage = damage;
/*        _character = characterStat;
        _detectedRange = GetComponent<DetectRange>();
        _detectedRange.Init(targetLayer);*/
    }

    private void OnDisable()
    {
        if (_coRotate != null)
            StopCoroutine(_coRotate);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _coRotate = StartCoroutine(CoRotate());
    }

    private IEnumerator CoRotate()
    {
        float currentTime = 0f;

        int count = (int)(_duration / timePerAttack) - 1;
        int currentCount = 0;
        while (true)
        {
/*            currentTime += Time.deltaTime;

            if(currentTime >= timePerAttack)
            {
                currentCount++;
                if (currentCount == count)
                    break;

                for (int i = 0; i < _detectedRange.enemies.Count; i++)
                {
                    Vector3 dir = transform.position - _detectedRange.enemies[i].transform.position;
                    dir = new Vector3(dir.x, dir.y, 0).normalized;
                    _detectedRange.enemies[i].SetCC(ECCType.dragged, dir * 0.3f);
                    _detectedRange.enemies[i].ChangeCurrentHp(-_damage, _character);
                }
                currentTime = 0;
            }

            _outterCircle.eulerAngles += Vector3.forward * Time.deltaTime * 10f;
            _innerCircle.eulerAngles += Vector3.forward * Time.deltaTime * -8f;
            _coffeeBean.eulerAngles += Vector3.forward * Time.deltaTime * 16f;*/

            yield return null;
        }

/*        for (int i = 0; i < _detectedRange.enemies.Count; i++)
            _detectedRange.enemies[i].SetCC(ECCType.airborne, Vector3.up * 3f);*/
    }
}
