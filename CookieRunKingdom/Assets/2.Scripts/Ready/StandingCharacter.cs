using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StandingCharacter : MonoBehaviour
{
    private CharacterData _characterData;
    public CharacterData CharacterData
    { get { return _characterData; } }



    private SkeletonAnimation _skeletonAnimation;

    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void SetData(CharacterData characterData)
    {
        _characterData = characterData;        
        _skeletonAnimation.skeletonDataAsset = characterData.SkeletonDataAsset;
        _skeletonAnimation.AnimationName = "idle";
        _skeletonAnimation.Initialize(true);        
    }


}
