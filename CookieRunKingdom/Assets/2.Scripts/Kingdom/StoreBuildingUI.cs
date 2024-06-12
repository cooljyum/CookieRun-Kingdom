using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreBuildingUI : MonoBehaviour
{
    public BuildingData BuildingData;
    private RawImage _buildingImage;
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _buildingName;
    private TextMeshProUGUI _environmentPoint;
    private TextMeshProUGUI _curCount;
    private GameObject _inactiveImage;

    private void Awake()
    {
        _buildingImage = transform.GetChild(1).GetComponent<RawImage>();
        _costText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        _buildingName = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        _environmentPoint = transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>();
        _curCount = transform.GetChild(3).GetChild(5).GetComponent<TextMeshProUGUI>();
        _inactiveImage = transform.parent.GetChild(4).gameObject;
    }

    private void Start()
    {
        BuildingData buildingData = BuildingData;
        SetData(buildingData);
    }

    private void SetData(BuildingData buildingData)
    {
        RenderTexture[] _renderTextures = Resources.LoadAll<RenderTexture>("RenderTextures");
        _buildingImage.texture = _renderTextures[buildingData.Key/100];
        _costText.text = buildingData.RequiredGold.ToString();
        _buildingName.text = buildingData.Name;
        _environmentPoint.text = buildingData.Point.ToString();
        //_curCount.text = GameManager.Instance.CurPlayerData.  //*설치완료 -> _curCount++;
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
