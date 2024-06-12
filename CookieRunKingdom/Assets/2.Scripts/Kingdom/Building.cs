using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingData _buildingData;
    private SkeletonAnimation _skeletonAnimation;

    private void Awake()
    {
        _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    public void Build(BuildingData buildingData, Vector2 pos) //�ǹ� ������ ���� & ��ġ
    {
        _buildingData = buildingData;
        _skeletonAnimation.skeletonDataAsset = buildingData.SkeletonDataAsset;
        _skeletonAnimation.Initialize(true);
        transform.position = pos;
        gameObject.SetActive(true);
    }

    public void OnClickBtn() //�ǹ� Ŭ�� -> ���� â
    {
        print("Btn Click");
        KingdomManager.Instance.CraftPanel.SetActive(true);
    }
}
