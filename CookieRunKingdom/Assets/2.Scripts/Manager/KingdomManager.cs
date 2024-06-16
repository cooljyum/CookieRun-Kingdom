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
    private BuildingInfoUI _buildingInfoPanel;
    [SerializeField]
    private CraftUI _craftUI;
    [SerializeField]
    private GameObject _kingdomPlayPanel;

    [Header("---------------------------------------------------------")]
    [SerializeField]
    private SkeletonAnimation _selectedBuilding;

    private StoreBuildingUI _selectedBuildingUI;
    private BuildingData _selectedBuildingData;

    private TextMeshProUGUI _buildingCost;
    private TextMeshProUGUI _buildingPoint;
    private TextMeshProUGUI _buildingCurCount;

    private void Awake()
    {
        Instance = this;
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
        _selectedBuilding.skeletonDataAsset = buildingUI.GetBuildingData().SkeletonDataAsset;
        _selectedBuilding.Initialize(true);
    }

    public void SelectBuilding(BuildingData data) //선택한 건물 출현 세팅
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingData = data;

        _selectedBuilding.gameObject.SetActive(true);
        _selectedBuilding.skeletonDataAsset = _selectedBuildingData.SkeletonDataAsset;
        _selectedBuilding.Initialize(true);
    }

    public int Building() //선택한 건물의 데이터 키 값 반환
    {
        if (_selectedBuildingUI == null)
            return 0;

        int key = _selectedBuildingUI.GetBuildingData().Key;

        //건물 설치 개수 증가
        GameManager.Instance.AddBuilding(key);
        int currentCount = GameManager.Instance.GetBuildingCount(key);

        if (currentCount >= _selectedBuildingUI.MaxCount)
        {
            _selectedBuildingUI.SetInActive(true);
        }

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

    public void ClickStoreBuildingBtn(BuildingData data)
    {
        _storeUI.gameObject.SetActive(false);
        _buildingInfoPanel.SetData(data);
    }

    public void OnClickBuilding(BuildingData data) //설치된 건물
    {
        print("Building Click");

        if (data.Type == "Decorative")
        { 
            //ToastMessage
        }
        else
        {
            _craftUI.gameObject.SetActive(true);
            _craftUI.CreateCraftItem(data);
            _craftUI.SetData(data);
        }
    }

    public void ClickCraftBtn(CraftItemInfo craftItemInfo)
    {
        _craftUI.CraftStart(craftItemInfo);
    }
}
