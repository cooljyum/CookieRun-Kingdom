using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    private StoreUI _storeUI;
    [SerializeField]
    private GameObject _buildingInfoPanel;
    [SerializeField]
    private CraftUI _craftUI;
    [SerializeField]
    private GameObject _kingdomPlayPanel;

    [Header("---------------------------------------------------------")]
    [SerializeField]
    private SkeletonAnimation _selectedBuilding;

    private StoreBuildingUI _selectedBuildingUI;    
    
    private TextMeshProUGUI _buildingCost;
    private TextMeshProUGUI _buildingPoint;
    private TextMeshProUGUI _buildingCurCount;

    private void Awake()
    {
        Instance = this;               
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!_selectedBuilding.gameObject.activeSelf) return;

        Vector2 mousePos = Input.mousePosition;
        _selectedBuilding.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void SelectBuilding(StoreBuildingUI buildingUI) //선택한 건물 출현 세팅
    {
        _storeUI.gameObject.SetActive(false);
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
        _storeUI.gameObject.SetActive(true);
        _storeUI.CreateCTypeBuilding();
    }

    public void OnClickBuilding(BuildingData data) //설치된 건물
    {
        print("Building Click");
        _craftUI.gameObject.SetActive(true);
        _craftUI.CreateCraftItem(data);
    }

    public void OnClickOkayBtn()
    {
        print("OkayBtn Click");
        _buildingInfoPanel.SetActive(true);

    }
}
