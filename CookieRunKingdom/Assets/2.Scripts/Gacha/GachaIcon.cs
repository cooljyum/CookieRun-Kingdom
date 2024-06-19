using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaIcon : MonoBehaviour
{ 
    private CharacterData _characterData;
    public CharacterData CharacterData { get { return _characterData; } }
    private SkeletonGraphic _skeletonGraphic;
    private void Awake()
    {
        _skeletonGraphic = GetComponent<SkeletonGraphic>();
    }

    public void SetData(CharacterData characterData)
    {
        _characterData = characterData;
        _skeletonGraphic.skeletonDataAsset = characterData.SkeletonDataAsset;
    }
}
