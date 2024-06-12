using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class KingdomManager : MonoBehaviour
{
    static public KingdomManager Instance;

    [Header("UI Panel")]
    [SerializeField]
    private GameObject _storePanel;

    [SerializeField]
    private GameObject _buildingInfoPanel;

    [SerializeField]
    public GameObject CraftPanel;

    [SerializeField]
    private GameObject _kingdomPlayPanel;

    [Header("---------------------------------------------------------")]
    [SerializeField]
    private SkeletonAnimation _selectedBuilding;

    private StoreBuildingUI _selectedBuildingUI;    
    private Transform _cTypeBuildingContent; //건설 타입(constrctType)
    private Transform _dTypeBuildingContent; //꾸미기 타입(decorateType)
    private Transform _itemCellContent;
    private TextMeshProUGUI _buildingCost;
    private TextMeshProUGUI _buildingPoint;
    private TextMeshProUGUI _buildingCurCount;

    private void Awake()
    {
        Instance = this;

        Transform[] childen = GetComponentsInChildren<Transform>();

        foreach (Transform child in childen)
        {
            string name = child.name;

            if (name == "ConstructContent")
            {
                _cTypeBuildingContent = child;
            }
            else if (name == "DecorateContent")
            {
                _dTypeBuildingContent = child;
            }
            else if (name == "ProduceContent")
            {
                _itemCellContent = child;
            }
        }
    }

    private void Start()
    {
        //건설 타입 건물 칸 생성
        GameObject cTypeBuildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/StoreCTypeBuildingCell");

        foreach (int myBuilding in GameManager.Instance.MyBuildings)
        {
            BuildingData data = DataManager.Instance.GetBuildingData(myBuilding);

            GameObject buildingObj = Instantiate(cTypeBuildingPrefab, _cTypeBuildingContent);
        }

        //꾸미기 타입 건물 칸 생성
        GameObject dTypeBuildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/StoreDTypeBuildingCell");

        foreach (int myBuilding in GameManager.Instance.MyBuildings)
        {
            BuildingData data = DataManager.Instance.GetBuildingData(myBuilding);

            GameObject buildingObj = Instantiate(dTypeBuildingPrefab, _dTypeBuildingContent);
        }

        //건물 생산 칸 생성
        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/CraftItemCell");
        
        for(int i = 0; i < 3; i++)
        {
            GameObject itemObj = Instantiate(itemPrefab, _itemCellContent);
        }
    }

    private void Update()
    {
        if (!_selectedBuilding.gameObject.activeSelf) return;

        Vector2 mousePos = Input.mousePosition;
        _selectedBuilding.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void SelectBuilding(StoreBuildingUI buildingUI) //선택한 건물 출현 세팅
    {
        _storePanel.SetActive(false);
        _selectedBuildingUI = buildingUI;

        _selectedBuilding.gameObject.SetActive(true);
        _selectedBuilding.skeletonDataAsset = buildingUI.BuildingData.SkeletonDataAsset;
        _selectedBuilding.Initialize(true);
    }
    
    public int Building() //선택한 건물의 데이터 키 값 반환
    {
        if (_selectedBuildingUI == null)
            return 0;
    
        int key = _selectedBuildingUI.BuildingData.Key;

        //if(_selectedBuildingUI._curCount == _maxCount) //*현재 개수 = 최대 개수 -> 비활성화*//
        _selectedBuildingUI.SetInActive(true);

        _selectedBuildingUI = null;
        _selectedBuilding.gameObject.SetActive(false);
    
        return key;
    }

    public void OnClickConstructBtn() //Main-건설하기
    {
        print("ConstructBtn Click");
        _storePanel.SetActive(true);
    }

    public void OnClickStoreExitBtn() //Store-나가기
    {
        print("StoreExitBtn Click");
        _storePanel.SetActive(false);
    }

    public void OnClickConstructTypeBtn() //Store-건설
    {
        print("ConstructTypeBtn Click");
        _cTypeBuildingContent.gameObject.SetActive(true);
        _dTypeBuildingContent.gameObject.SetActive(false);
    }

    public void OnClickDecorateTypeBtn() //Store-꾸미기
    {
        print("DecorateTypeBtn Click");
        _cTypeBuildingContent.gameObject.SetActive(false);
        _dTypeBuildingContent.gameObject.SetActive(true);
    }
}
