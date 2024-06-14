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
    
    private void Awake()
    {

    }

    private void Start()
    {

    }

    public void SetData(BuildingData buildingData, bool isCType)
    {
        _buildingData = buildingData;

        _isCType = isCType;
        _buildingImage.skeletonDataAsset = buildingData.SkeletonDataAsset;
        _buildingImage.startingAnimation = "off";
        _buildingImage.Initialize(true);
        _costText.text = buildingData.RequiredGold.ToString();
        _buildingName.text = buildingData.Name;
        _environmentPoint.text = buildingData.Point.ToString();
        if (_isCType)
        {
            //_curCount.text = GameManager.Instance.CurPlayerData.  //*��ġ�Ϸ� -> _curCount++;
        }
    }

    public void OnClickBuildingBtn() //Store-�ǹ�
    {   
        print("BuildingBtn Click");
        KingdomManager.Instance.SelectBuilding(this);
    }

    public void SetInActive(bool isActive) //���� �� �ǹ� ��ư ��Ȱ��ȭ
    {
        _inactiveImage.SetActive(isActive);
    }
}
