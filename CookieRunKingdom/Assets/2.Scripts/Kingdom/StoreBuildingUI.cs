using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreBuildingUI : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic _buildingImage;
    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private TextMeshProUGUI _buildingName;
    [SerializeField]
    private TextMeshProUGUI _environmentPoint;
    [SerializeField]
    private TextMeshProUGUI _curCount;
    [SerializeField]
    private GameObject _inactiveImage;

    private BuildingData _buildingData;
    public BuildingData GetBuildingData() { return _buildingData; }

    private bool _isCType;
    public int MaxCount { get; private set; }
    private int _buildingLevel = 1;

    public void SetData(BuildingData buildingData)
    {
        _buildingData = buildingData;
        if (_buildingData.Type == "Decorative")
        {
            _isCType = false;
        }
        else
        {
            _isCType = true;
        }
        _buildingImage.skeletonDataAsset = buildingData.SkeletonDataAsset;
        _buildingImage.startingAnimation = "off";
        _buildingImage.Initialize(true);
        _costText.text = buildingData.RequiredGold.ToString();
        _buildingName.text = buildingData.Name;
        _environmentPoint.text = buildingData.Point.ToString();
        MaxCount = _buildingLevel;

        if (_isCType)
        {
            int currentCount = GameManager.Instance.GetBuildingCount(buildingData.Key);
            _curCount.text = currentCount.ToString();

            // 최대 개수에 도달하면 버튼 비활성화
            if (currentCount >= MaxCount)
            {
                SetInActive(true);
            }
            else
            {
                SetInActive(false);
            }
        }
    }

    public void OnClickBuildingBtn() //Store-건물
    {   
        print("BuildingBtn Click");

        if (_isCType)
        {
            KingdomManager.Instance.ClickStoreBuildingBtn(_buildingData);
        }
        else
        {
            KingdomManager.Instance.SelectDTypeBuilding(this);
        }

        KingdomManager.Instance.MapGrid.SetActive(true);
    }

    public void SetInActive(bool isActive) //상점 내 건물 버튼 비활성화
    {
        _inactiveImage.SetActive(isActive);
    }
}
