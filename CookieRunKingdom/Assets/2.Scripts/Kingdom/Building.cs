using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingData _buildingData;
    private SkeletonAnimation _skeletonAnimation;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(_collider.bounds.Contains(worldPos))
            {
                OnClickBtn();
            }
        }
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
        KingdomManager.Instance.OnClickBuilding(_buildingData);
    }
}
