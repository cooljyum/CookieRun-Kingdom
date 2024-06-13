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

    public BuildingData BuildingData;
    private bool _isCType;
    
    private void Awake()
    {

    }

    private void Start()
    {

    }

    public void SetData(BuildingData buildingData, bool isCType)
    {
        _isCType = isCType;
        _buildingImage.skeletonDataAsset = buildingData.SkeletonDataAsset;
        _buildingImage.startingAnimation = "off";
        _buildingImage.Initialize(true);
        _costText.text = buildingData.RequiredGold.ToString();
        _buildingName.text = buildingData.Name;
        _environmentPoint.text = buildingData.Point.ToString();
        if (_isCType)
        {
            //_curCount.text = GameManager.Instance.CurPlayerData.  //*설치완료 -> _curCount++;
        }
    }

    public void OnClickBuildingBtn() //Store-건물
    {   
        print("BuildingBtn Click");
        KingdomManager.Instance.SelectBuilding(this);
    }

    public void SetInActive(bool isActive) //상점 내 건물 버튼 비활성화
    {
        _inactiveImage.SetActive(isActive);
    }
}
