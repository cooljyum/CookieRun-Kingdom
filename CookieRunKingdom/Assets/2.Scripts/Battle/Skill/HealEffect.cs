using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _healCircle;

    private Sequence _seq = null;

    private void OnEnable()
    {
        _healCircle.transform.localScale = Vector3.zero;
        _healCircle.color = new Color(_healCircle.color.r, _healCircle.color.g, _healCircle.color.b, 1f);
        if(_seq == null)
        {
            _seq = DOTween.Sequence();
            _seq.Pause().SetAutoKill(false).Append(_healCircle.transform.DOScale(Vector3.one * 2, 0.5f))
                .Join(_healCircle.DOFade(0, 1.5f))
                .OnComplete(() => gameObject.SetActive(false));
        }

        _seq.Pause();
        _seq.Restart();
    }

    private void OnDisable()
    {
        _seq.Pause();
    }
}
