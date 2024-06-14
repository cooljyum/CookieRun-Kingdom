using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testststt : MonoBehaviour
{
    [SerializeField] private Transform _outterCircle;
    [SerializeField] private Transform _innerCircle;
    [SerializeField] private Transform _coffeeBean;

    private Coroutine _coRotate;

    private void OnEnable()
    {
        _coRotate = StartCoroutine(CoRotate());
    }

    private void OnDisable()
    {
        if (_coRotate != null)
            StopCoroutine(_coRotate);
    }

    private IEnumerator CoRotate()
    {
        while(true)
        {
            _outterCircle.eulerAngles += Vector3.forward * Time.deltaTime * 10f;
            _innerCircle.eulerAngles += Vector3.forward * Time.deltaTime * -8f;
            _coffeeBean.eulerAngles += Vector3.forward * Time.deltaTime * 16f;

            yield return null;
        }
    }
}
