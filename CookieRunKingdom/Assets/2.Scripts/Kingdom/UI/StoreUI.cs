using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    [SerializeField]
    private Transform _cTypeBuildingContent; //건설 타입(Constrct Type) 건물들의 부모 객체
    [SerializeField]
    private Transform _dTypeBuildingContent; //꾸미기 타입(Decoration Type) 건물들의 부모 객체

    private GameObject _cTypeBuildingPrefab;
    private GameObject _dTypeBuildingPrefab;

    private BuildingData[] _buildingDataArray; //모든 건물 데이터 배열 (필터링하여 상점에 노출)

    private void Awake()
    {
        _cTypeBuildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Construct/StoreCTypeBuildingCell");
        _dTypeBuildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Construct/StoreDTypeBuildingCell");

        _buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");
    }

    public void CreateCTypeBuilding() //건물 데이터에 맞춰 건설 타입 건물 칸 생성
    {
        //cTypeBuildingContent의 기존 자식 오브젝트 삭제
        for (int i = _cTypeBuildingContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_cTypeBuildingContent.GetChild(i).gameObject);
        }

        //buildingDataArray중에서 C타입 건물 생성
        foreach (BuildingData buildingData in _buildingDataArray)
        {
            if (buildingData.Type != "Decorative")
            {
                GameObject buildingObj = Instantiate(_cTypeBuildingPrefab, _cTypeBuildingContent);
                buildingObj.GetComponent<StoreBuildingUI>().SetData(buildingData);
            }
        }
    }

    public void CreateDTypeBuilding() //건물 데이터에 맞춰 꾸미기 타입 건물 칸 생성
    {
        //dTypeBuildingContent의 기존 자식 오브젝트 삭제
        for (int i = _dTypeBuildingContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_dTypeBuildingContent.GetChild(i).gameObject);
        }

        //buildingDataArray중에서 D타입 건물 생성
        foreach (BuildingData buildingData in _buildingDataArray)
        {
            if (buildingData.Type == "Decorative")
            {
                GameObject buildingObj = Instantiate(_dTypeBuildingPrefab, _dTypeBuildingContent);
                buildingObj.GetComponent<StoreBuildingUI>().SetData(buildingData);
            }
        }
    }

    public void OnClickConstructTypeBtn() //Store-건설
    {
        Debug.Log("ConstructTypeBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateCTypeBuilding();
        _cTypeBuildingContent.gameObject.SetActive(true);
        _dTypeBuildingContent.gameObject.SetActive(false);
    }

    public void OnClickDecorateTypeBtn() //Store-꾸미기
    {
        Debug.Log("DecorateTypeBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateDTypeBuilding();
        _cTypeBuildingContent.gameObject.SetActive(false);
        _dTypeBuildingContent.gameObject.SetActive(true);
    }

    public void OnClickExitBtn() //Store-나가기
    {
        Debug.Log("ExitBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        gameObject.SetActive(false);
    }
}
