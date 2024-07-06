using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreBuildingUI : MonoBehaviour  //건물 상점 칸
{
    [SerializeField]
    private SkeletonGraphic _buildingImage; //건물 이미지
    [SerializeField]
    private TextMeshProUGUI _costText; //건물 가격
    [SerializeField]
    private TextMeshProUGUI _buildingName; //건물 이름
    [SerializeField]
    private TextMeshProUGUI _environmentPoint; //건물 환경 점수
    [SerializeField]
    private TextMeshProUGUI _curCount; //건물의 현재 설치 수
    [SerializeField]
    private GameObject _inactiveImage; //비활성화 이미지 (설치 가능한 만큼 설치 시 활성화)

    private BuildingData _buildingData; //건물 데이터
    public BuildingData GetBuildingData() { return _buildingData; }

    private bool _isCType; //건설 타입 여부
    public int MaxCount { get; private set; } //설치 가능 최대 개수
    private int _buildingLevel = 1; //건물 레벨

    public void SetData(BuildingData buildingData) //건물 상점 UI 활성화 -> 데이터 세팅
    {
        _buildingData = buildingData;

        if (_buildingData.Type == "Decorative")
            _isCType = false;
        else
            _isCType = true;

        _buildingImage.skeletonDataAsset = buildingData.SkeletonDataAsset;
        _buildingImage.startingAnimation = "off";
        _buildingImage.Initialize(true);
        _costText.text = buildingData.RequiredCoin.ToString();
        _buildingName.text = buildingData.Name;
        _environmentPoint.text = buildingData.Point.ToString();
        MaxCount = _buildingLevel;

        if (_isCType)
        {
            int currentCount = GameManager.Instance.GetBuildingCount(buildingData.Key);
            _curCount.text = currentCount.ToString();

            // 최대 개수에 도달하면 버튼 비활성화
            if (currentCount >= MaxCount)
                SetInActive(true);
            else
                SetInActive(false);
        }
    }

    public void OnClickBuildingBtn() //Store-건물
    {   
        Debug.Log("BuildingBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");

        if (_isCType)
            KingdomManager.Instance.ClickStoreBuildingBtn(_buildingData);
        else
            KingdomManager.Instance.SelectDTypeBuilding(this);

        KingdomManager.Instance.MapGrid.SetActive(true);
    }

    public void SetInActive(bool isActive) //상점 내 건물 버튼 비활성화
    {
        _inactiveImage.SetActive(isActive);
    }
}
