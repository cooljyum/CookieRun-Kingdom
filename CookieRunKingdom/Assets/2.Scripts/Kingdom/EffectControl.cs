using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour  //* 이펙트 관리 공간 *//
{
    [SerializeField]
    private Animator _installEffectAnimator;

    public void EndInstallEffect()
    {
        Debug.Log("Animation has ended.");
        _installEffectAnimator.gameObject.SetActive(false);
    }
}
