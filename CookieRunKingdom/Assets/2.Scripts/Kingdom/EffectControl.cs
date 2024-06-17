using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour
{
    [SerializeField]
    private Animator _installEffectAnimator;

    public void EndInstallEffect()
    {
        Debug.Log("Animation has ended.");
        _installEffectAnimator.gameObject.SetActive(false);
    }
}
