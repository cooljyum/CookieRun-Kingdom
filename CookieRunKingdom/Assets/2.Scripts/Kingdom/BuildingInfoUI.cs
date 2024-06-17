using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BuildingInfoUI : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic _buildingImage;
    [SerializeField]
    private TextMeshProUGUI _buildingInfo;
    [SerializeField]
    private TextMeshProUGUI _environmentPoint;
    [SerializeField]
    private TextMeshProUGUI _buildingSize;
    [SerializeField]
    private Transform _ingredientContent;
    [SerializeField]
    private TextMeshProUGUI _buildingName;

    private GameObject _itemCellPrefab;
    private GameObject _timeCellPrefab;
    private BuildingData _buildingData;

    private void Awake()
    {
        _itemCellPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Construct/InfoItemCell");
        _timeCellPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Construct/InfoTimeCell");
    }

    public void SetData(BuildingData data)
    {
        _buildingData = data;
        _buildingImage.skeletonDataAsset = data.SkeletonDataAsset;
        _buildingImage.Initialize(true);
        _buildingInfo.text = data.Information.ToString();
        _environmentPoint.text = data.Point.ToString();
        _buildingSize.text = $"{data.Size}X{data.Size}";
        _buildingName.text = data.Name.ToString();
        gameObject.SetActive(true);
        CreateIngredients(data);
    }

    private void CreateIngredients(BuildingData data) //건물 데이터에 맞춰 생산 칸 삭제 & 생성
    {
        for (int i = _ingredientContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_ingredientContent.GetChild(i).gameObject);
        }

        ItemData[] allItemData = Resources.LoadAll<ItemData>("Data/Item");

        //아이템 Cell 생성
        if (data.RequiredMaterial != null)
        {
            GameObject itemObj = Instantiate(_itemCellPrefab, _ingredientContent);
            var ingredientUI = itemObj.GetComponent<InfoIngridientUI>();
            ingredientUI.SetData(data.RequiredMaterial, data.RequiredMaterialCount);
        }
        if (data.RequiredEquipment != null)
        {
            GameObject itemObj = Instantiate(_itemCellPrefab, _ingredientContent);
            var ingredientUI = itemObj.GetComponent<InfoIngridientUI>();
            ingredientUI.SetData(data.RequiredEquipment, data.RequiredEquipmentCount);
        }

        //시간 Cell 생성
        GameObject timeObj = Instantiate(_timeCellPrefab, _ingredientContent);
        var timeUI = timeObj.GetComponent<InfoIngridientUI>();
        timeUI.SetTimeData((int)data.RequiredTime);
    }

    public void OnClickOkayBtn() //Info-확인
    {
        print("OkayBtn Click");
        gameObject.SetActive(false);
        KingdomManager.Instance.SelectCTypeBuilding(_buildingData);
    }

    public void OnClickExitBtn() //Info-나가기
    {
        print("ExitBtn Click");
        gameObject.SetActive(false);
    }
}
