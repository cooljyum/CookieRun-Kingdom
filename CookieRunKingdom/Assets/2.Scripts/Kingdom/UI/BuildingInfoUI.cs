using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BuildingInfoUI : MonoBehaviour  //* �ǹ� ���� UI *//
{
    [SerializeField]
    private GameObject _parentObject; //��Ȱ��ȭ�� �θ� ������Ʈ
    [SerializeField]
    private SkeletonGraphic _buildingImage; //�ǹ� �̹���
    [SerializeField]
    private TextMeshProUGUI _buildingInfo; //�ǹ� �Ұ���
    [SerializeField]
    private TextMeshProUGUI _environmentPoint; //�ǹ� ȯ�� ����
    [SerializeField]
    private TextMeshProUGUI _buildingSize; //�ǹ� (�ٴ�) ũ�� (: NXN)
    [SerializeField]
    private Transform _ingredientContent; //�Ǽ� ���
    [SerializeField]
    private TextMeshProUGUI _buildingName; //�ǹ� �̸�

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

    private void CreateIngredients(BuildingData data) //�ǹ� �����Ϳ� ���� ���� ĭ ���� & ����
    {
        for (int i = _ingredientContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_ingredientContent.GetChild(i).gameObject);
        }

        ItemData[] allItemData = Resources.LoadAll<ItemData>("Data/Item");

        //������ Cell ����
        if (data.RequiredMaterial != null)
        {
            GameObject itemObj = Instantiate(_itemCellPrefab, _ingredientContent);
            var ingredientUI = itemObj.GetComponent<InfoIngridientUI>();
            ingredientUI.SetIngredientData(data.RequiredMaterial, data.RequiredMaterialCount);
        }
        if (data.RequiredEquipment != null)
        {
            GameObject itemObj = Instantiate(_itemCellPrefab, _ingredientContent);
            var ingredientUI = itemObj.GetComponent<InfoIngridientUI>();
            ingredientUI.SetIngredientData(data.RequiredEquipment, data.RequiredEquipmentCount);
        }

        //�ð� Cell ����
        GameObject timeObj = Instantiate(_timeCellPrefab, _ingredientContent);
        var timeUI = timeObj.GetComponent<InfoIngridientUI>();
        timeUI.SetTimeData((int)data.RequiredTime);
    }

    public void OnClickOkayBtn() //Info-Ȯ��
    {
        Debug.Log("OkayBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        gameObject.SetActive(false);
        KingdomManager.Instance.SelectCTypeBuilding(_buildingData);
    }

    public void OnClickExitBtn() //Info-������
    {
        Debug.Log("ExitBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        _parentObject.SetActive(false);
    }
}
