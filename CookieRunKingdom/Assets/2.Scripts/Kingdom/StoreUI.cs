using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    [SerializeField]
    private Transform _cTypeBuildingContent; //�Ǽ� Ÿ��(constrctType)
    [SerializeField]
    private Transform _dTypeBuildingContent; //�ٹ̱� Ÿ��(decorateType)    

    private GameObject _cTypeBuildingPrefab;
    private GameObject _dTypeBuildingPrefab;

    private BuildingData[] _buildingDataArray;

    private void Awake()
    {
        _cTypeBuildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/StoreCTypeBuildingCell");
        _dTypeBuildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/StoreDTypeBuildingCell");

        _buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");
    }

    public void CreateCTypeBuilding() //�ǹ� �����Ϳ� ���� �Ǽ� Ÿ�� �ǹ� ĭ ����
    {
        //cTypeBuildingContent�� ���� �ڽ� ������Ʈ ����
        for (int i = _cTypeBuildingContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_cTypeBuildingContent.GetChild(i).gameObject);
        }

        //buildingDataArray�߿��� CŸ�� �ǹ� ����
        foreach (BuildingData buildingData in _buildingDataArray)
        {
            if (buildingData.Type != "Decorative")
            {
                GameObject buildingObj = Instantiate(_cTypeBuildingPrefab, _cTypeBuildingContent);
                buildingObj.GetComponent<StoreBuildingUI>().SetData(buildingData);
            }
        }
    }

    public void CreateDTypeBuilding() //�ǹ� �����Ϳ� ���� �ٹ̱� Ÿ�� �ǹ� ĭ ����
    {
        //dTypeBuildingContent�� ���� �ڽ� ������Ʈ ����
        for (int i = _dTypeBuildingContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_dTypeBuildingContent.GetChild(i).gameObject);
        }

        //buildingDataArray�߿��� DŸ�� �ǹ� ����
        foreach (BuildingData buildingData in _buildingDataArray)
        {
            if (buildingData.Type == "Decorative")
            {
                GameObject buildingObj = Instantiate(_dTypeBuildingPrefab, _dTypeBuildingContent);
                buildingObj.GetComponent<StoreBuildingUI>().SetData(buildingData);
            }
        }
    }

    public void OnClickConstructTypeBtn() //Store-�Ǽ�
    {
        print("ConstructTypeBtn Click");
        CreateCTypeBuilding();
        _cTypeBuildingContent.gameObject.SetActive(true);
        _dTypeBuildingContent.gameObject.SetActive(false);
    }

    public void OnClickDecorateTypeBtn() //Store-�ٹ̱�
    {
        print("DecorateTypeBtn Click");
        CreateDTypeBuilding();
        _cTypeBuildingContent.gameObject.SetActive(false);
        _dTypeBuildingContent.gameObject.SetActive(true);
    }

    public void OnClickExitBtn() //Store-������
    {
        print("ExitBtn Click");
        gameObject.SetActive(false);
    }
}
