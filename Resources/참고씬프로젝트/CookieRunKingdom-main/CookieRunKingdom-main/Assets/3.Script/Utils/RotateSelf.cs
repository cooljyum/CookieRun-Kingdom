using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateSelf : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

    private Tweener tweener;

    private void OnDisable()
    {
        tweener.Kill();
    }

    private void OnEnable()
    {
        tweener = transform.DOLocalRotate(new Vector3(0, 0, -360), _rotateSpeed * Time.deltaTime, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }
}
