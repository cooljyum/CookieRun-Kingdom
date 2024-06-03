using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieSkeletonManager : MonoBehaviour
{
    public static CookieSkeletonManager Instance;

    [SerializeField]
    private List<SkeletonDataAsset> _skeletonDataAsset = new List<SkeletonDataAsset>();

    [SerializeField]
    // Key로 CookieSkeletonDataAsset을 받아오는 딕셔너리
    public Dictionary<int, SkeletonDataAsset> _skeletonDataAssets = new Dictionary<int, SkeletonDataAsset>();

    private void Awake()
    {
        Instance = this;

        RegisterSkeletonDataFromList();
    }

    // List를 딕셔너리에 등록하는 메소드
    public void RegisterSkeletonDataFromList()
    {
        _skeletonDataAssets.Clear(); // 기존 딕셔너리 초기화

        for (int i = 0; i < _skeletonDataAsset.Count; i++)
        {
            // 리스트의 인덱스를 키로 사용하여 딕셔너리에 등록
            _skeletonDataAssets[i] = _skeletonDataAsset[i];
        }
    }

    // Set
    public void RegisterSkeletonData(int key, SkeletonDataAsset skeletonData)
    {
        if (!_skeletonDataAssets.ContainsKey(key))
        {
            _skeletonDataAssets[key] = skeletonData;
        }
        else
        {
            Debug.LogWarning($"SkeletonDataAsset with key {key} is already registered.");
        }
    }

    // Get
    public SkeletonDataAsset GetSkeletonData(int key)
    {
        if (_skeletonDataAssets.ContainsKey(key))
        {
            return _skeletonDataAssets[key];
        }
        else
        {
            Debug.LogWarning($"SkeletonDataAsset with key {key} not found.");
            return null;
        }
    }

    // SkeletonAnimation 설정
    public void SetSkeletonDataForAnimation(SkeletonAnimation skeletonAnimation, int key)
    {
        SkeletonDataAsset skeletonData = GetSkeletonData(key);
        if (skeletonData != null)
        {
            skeletonAnimation.skeletonDataAsset = skeletonData;
            skeletonAnimation.Initialize(true); // 새로운 SkeletonDataAsset을 적용하기 위해 초기화
        }
        else
        {
            Debug.LogError($"Failed to set SkeletonDataAsset for key {key}.");
        }
    }
}
