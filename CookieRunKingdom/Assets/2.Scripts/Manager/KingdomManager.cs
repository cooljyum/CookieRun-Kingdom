using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class KingdomManager : MonoBehaviour
{
    static public KingdomManager Instance;

    [Header("UI Panel")]
    [SerializeField]
    private StoreUI _storeUI;
    public StoreUI StoreUI => _storeUI;
    [SerializeField]
    private BuildingInfoUI _buildingInfoPanel;
    [SerializeField]
    private CraftUI _craftUI;
    [SerializeField]
    private GameObject _kingdomPlayPanel;
    [SerializeField]
    private GatchaUI _gatchaUI;

    [Header("---------------------------------------------------------")]
    [SerializeField]
    private SkeletonAnimation _selectedBuildingSkeletonAnimation;
    [SerializeField]
    private SpriteRenderer _previewImage;
    [SerializeField]
    private GameObject _editUI;

    private StoreBuildingUI _selectedBuildingUI;
    private BuildingData _selectedBuildingData;    

    private TextMeshProUGUI _buildingCost;
    private TextMeshProUGUI _buildingPoint;
    private TextMeshProUGUI _buildingCurCount;

    private Building _selectedBuilding;
    public Building SelectedBuilding => _selectedBuilding;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!_selectedBuildingSkeletonAnimation.gameObject.activeSelf) return;

        Vector2 mousePos = Input.mousePosition;
        _selectedBuildingSkeletonAnimation.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void SelectDTypeBuilding(StoreBuildingUI buildingUI) //선택한 D타입 건물 출현 세팅
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingUI = buildingUI;

        _selectedBuildingSkeletonAnimation.gameObject.SetActive(true);
        _selectedBuildingSkeletonAnimation.skeletonDataAsset = buildingUI.GetBuildingData().SkeletonDataAsset;
        _selectedBuildingSkeletonAnimation.Initialize(true);
    }

    public void SelectCTypeBuilding(BuildingData data) //선택한 C타입 건물 출현 세팅
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingData = data;

        _selectedBuildingSkeletonAnimation.gameObject.SetActive(true);
        _selectedBuildingSkeletonAnimation.skeletonDataAsset = _selectedBuildingData.SkeletonDataAsset;
        _selectedBuildingSkeletonAnimation.Initialize(true);
    }

    public int Building() //선택한 건물의 데이터 키 값 반환
    {
        int key = 0;

        if (_selectedBuildingUI != null)
        {
            _selectedBuildingData = _selectedBuildingUI.GetBuildingData();
            key = _selectedBuildingData.Key;
            Debug.Log($"Selected Building Key (from UI): {key}");

            //건물 설치 개수 증가
            GameManager.Instance.AddBuilding(key);
            int currentCount = GameManager.Instance.GetBuildingCount(key);

            if (currentCount >= _selectedBuildingUI.MaxCount)
            {
                _selectedBuildingUI.SetInActive(true);
            }

            _selectedBuildingUI = null;
        }
        else if (_selectedBuildingData != null)
        {
            key = _selectedBuildingData.Key;
            Debug.Log($"Selected Building Key (from Data): {key}");

            //건물 설치 개수 증가
            GameManager.Instance.AddBuilding(key);
            int currentCount = GameManager.Instance.GetBuildingCount(key);

            if (_selectedBuildingUI != null && currentCount >= _selectedBuildingUI.MaxCount)
            {
                _selectedBuildingUI.SetInActive(true);
            }

            _selectedBuildingData = null;
        }

        _selectedBuildingSkeletonAnimation.gameObject.SetActive(false);
        return key;
    }

    public void OnClickConstructBtn() //Main-건설하기
    {
        print("ConstructBtn Click");
        _storeUI.gameObject.SetActive(true);
        _storeUI.CreateCTypeBuilding();
    }

    public void OnClickPlayBtn() //Main-Play
    {
        print("PlayBtn Click");
        _kingdomPlayPanel.gameObject.SetActive(true);
    }

    public void OnClickGatchaBtn() //Main-뽑기
    {
        print("GatchaBtn Click");
        _gatchaUI.gameObject.SetActive(true);
    }

    public void OnClickCookieKingdomBtn() //Play-쿠키 왕국
    {
        print("CookieKingdomBtn Click");
        _kingdomPlayPanel.gameObject.SetActive(false);
    }

    public void OnClickWorldAdventureBtn() //Play-월드 탐험
    {
        print("WorldAdventureBtn Click");
        SceneManager.LoadScene("ReadyScene");
    }

    public void OnClickPlayExitBtn() //Play-나가기
    {
        print("PlayExitBtn Click");
        _kingdomPlayPanel.gameObject.SetActive(false);
    }

    public void ClickStoreBuildingBtn(BuildingData data)
    {
        _buildingInfoPanel.SetData(data);
    }

    public void OnClickBuilding(Building building) //설치된 건물
    {
        _selectedBuilding = building;
        BuildingData data = building.BuildingData;

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
            _craftUI.SetCraftingItem(building);
        }
    }

    public void ClickCraftBtn(CraftItemInfo craftItemInfo)
    {
        _craftUI.CraftStart(craftItemInfo);
    }
}
