using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingCharacter : MonoBehaviour
{
    private CharacterData _characterData;
    private SkeletonAnimation _skeletonAnimation;

    [SerializeField]
    private Transform _transform;

    private void Start()
    {
        transform.position = _transform.position;
    }

    public void SetTransform(Transform transform)
    {
        _transform = transform;
    }

    public void SetData(CharacterData characterData)
    {
        _characterData = characterData;
        _skeletonAnimation.SkeletonDataAsset.FillStateData
        
    }
}
