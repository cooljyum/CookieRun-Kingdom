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
    // Key�� CookieSkeletonDataAsset�� �޾ƿ��� ��ųʸ�
    public Dictionary<int, SkeletonDataAsset> _skeletonDataAssets = new Dictionary<int, SkeletonDataAsset>();

    private void Awake()
    {
        Instance = this;

        RegisterSkeletonDataFromList();
    }

    // List�� ��ųʸ��� ����ϴ� �޼ҵ�
    public void RegisterSkeletonDataFromList()
    {
        _skeletonDataAssets.Clear(); // ���� ��ųʸ� �ʱ�ȭ

        for (int i = 0; i < _skeletonDataAsset.Count; i++)
        {
            // ����Ʈ�� �ε����� Ű�� ����Ͽ� ��ųʸ��� ���
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

    // SkeletonAnimation ����
    public void SetSkeletonDataForAnimation(SkeletonAnimation skeletonAnimation, int key)
    {
        SkeletonDataAsset skeletonData = GetSkeletonData(key);
        if (skeletonData != null)
        {
            skeletonAnimation.skeletonDataAsset = skeletonData;
            skeletonAnimation.Initialize(true); // ���ο� SkeletonDataAsset�� �����ϱ� ���� �ʱ�ȭ
        }
        else
        {
            Debug.LogError($"Failed to set SkeletonDataAsset for key {key}.");
        }
    }
}
