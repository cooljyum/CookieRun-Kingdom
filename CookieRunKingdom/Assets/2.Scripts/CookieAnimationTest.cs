using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieAnimationTest : MonoBehaviour
{
    private SkeletonAnimation _skeletonAnimation;

    private void Start()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            print("KeyDown Enter");
            _skeletonAnimation.AnimationName = "die";
        }
    }

}
