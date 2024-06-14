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
    private BoxCollider2D _collider;

    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(_collider.bounds.Contains(worldPos))
            {
                //해제 이벤트 호출
                DeckSettingManager.Instance.RemoveCharacter(_characterData.Key);
            }
        }
    }

    public void SetData(CharacterData characterData)
    {
        _characterData = characterData;        
        _skeletonAnimation.skeletonDataAsset = characterData.SkeletonDataAsset;
        _skeletonAnimation.AnimationName = "idle";
        _skeletonAnimation.Initialize(true);        
    }


}
