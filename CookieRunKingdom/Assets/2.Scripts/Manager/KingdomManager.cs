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
    private GameObject _mainUI;
    public GameObject MainUI => _mainUI;
    [SerializeField]
    private StoreUI _storeUI;
    public StoreUI StoreUI => _storeUI;
    [SerializeField]
    private BuildingInfoUI _buildingInfoPanel;
    [SerializeField]
    private CraftUI _craftUI;
    [SerializeField]
    private GameObject _kingdomPlayPanel;

    [Header("InGame Infos")]
    [SerializeField]
    private TextMeshProUGUI[] _coinTexts;
    [SerializeField]
    private TextMeshProUGUI[] _diaTexts;
    [SerializeField]
    private Slider _expBar;
    private float _maxExp = 1000f; //추후 데이터화 예정

    [Header("---------------------------------------------------------")]
    [SerializeField]
    private SkeletonAnimation _selectedBuildingSkeletonAnimation;
    [SerializeField]
    private SpriteRenderer _previewImage;
    [SerializeField]
    private GameObject _editUI;
    [SerializeField]
    private GameObject _mapGrid;
    public GameObject MapGrid => _mapGrid;
    [SerializeField]
    private Tilemap _tilemap;

    private StoreBuildingUI _selectedBuildingUI;
    private BuildingData _selectedBuildingData;
    public BuildingData SelectedBuildingData => _selectedBuildingData;
    public Vector2 SelectedPosition;
    public bool IsBuildingFixed;
    private TextMeshProUGUI _buildingCost;
    private TextMeshProUGUI _buildingPoint;
    private TextMeshProUGUI _buildingCurCount;

    private Building _selectedBuilding;
    public Building SelectedBuilding => _selectedBuilding;

    private GameObject _overlapIndicatorPrefab;
    private List<GameObject> _overlapIndicators = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        _overlapIndicatorPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Map/BuildingOverlap");
    }

    private void Start()
    {
        SetMoneyValue();
        IsBuildingFixed = false;
    }

    private void Update()
    {
        if (IsBuildingFixed || !_selectedBuildingSkeletonAnimation.gameObject.activeSelf) return;

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);

        worldPos.x = Mathf.Round(worldPos.x);
        worldPos.y = Mathf.Round(worldPos.y);

        _selectedBuildingSkeletonAnimation.transform.position = worldPos;

        //겹치는 범위 표시
        //CheckOverlapAndShowIndicators(worldPos);
    }

    public void SelectDTypeBuilding(StoreBuildingUI buildingUI) //선택한 D타입 건물 출현 세팅
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingUI = buildingUI;

        _selectedBuildingSkeletonAnimation.gameObject.SetActive(true);
        _selectedBuildingSkeletonAnimation.skeletonDataAsset = buildingUI.GetBuildingData().SkeletonDataAsset;
        _selectedBuildingSkeletonAnimation.Initialize(true);
        _editUI.SetActive(true);
    }

    public void SelectCTypeBuilding(BuildingData data) //선택한 C타입 건물 출현 세팅
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingData = data;

        _selectedBuildingSkeletonAnimation.gameObject.SetActive(true);
        _selectedBuildingSkeletonAnimation.skeletonDataAsset = _selectedBuildingData.SkeletonDataAsset;
        _selectedBuildingSkeletonAnimation.Initialize(true);
        _editUI.SetActive(true);
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
        Debug.Log("ConstructBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        _storeUI.gameObject.SetActive(true);
        _storeUI.CreateCTypeBuilding();
    }

    public void OnClickPlayBtn() //Main-Play
    {
        Debug.Log("PlayBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        _kingdomPlayPanel.gameObject.SetActive(true);
    }

    public void OnClickGatchaBtn() //Main-뽑기
    {
        Debug.Log("GatchaBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        //_gatchaUI.gameObject.SetActive(true);
        SceneManager.LoadScene("GachaScene");
    }

    public void OnClickCookieKingdomBtn() //Play-쿠키 왕국
    {
        Debug.Log("CookieKingdomBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        _kingdomPlayPanel.gameObject.SetActive(false);
    }

    public void OnClickWorldAdventureBtn() //Play-월드 탐험
    {
        Debug.Log("WorldAdventureBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        SceneManager.LoadScene("ReadyScene");
    }

    public void OnClickPlayExitBtn() //Play-나가기
    {
        Debug.Log("PlayExitBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
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

        Debug.Log("Building Click");

        if (data.Type == "Decorative")
        { 
            //ToastMessage
        }
        else
        {
            _mainUI.SetActive(false);
            _craftUI.gameObject.SetActive(true);
            _craftUI.CreateCraftItem(data);
            _craftUI.SetData(data);
            _craftUI.SetCraftingItem(building);
        }
    }

    public void ClickCraftBtn(CraftItemInfo craftItemInfo) //생산 버튼 클릭 시 호출 -> 생산 시작
    {
        _craftUI.CraftStart(craftItemInfo);
    }

    public void SetSelectedBuilding(Building building)
    {
        _selectedBuilding = building;
    }

    public void SetSnappedPosition()
    {
        Vector2 originalPosition = _selectedBuildingSkeletonAnimation.transform.position;
        float tileSizeX = 2.6f;
        float tileSizeY = 1.33f;

        // 원래 위치를 타일 그리드 위치로 스냅
        float snappedX = Mathf.Round(originalPosition.x / tileSizeX) * tileSizeX;
        float snappedY = Mathf.Round(originalPosition.y / tileSizeY) * tileSizeY;

        SelectedPosition = new Vector2(snappedX, snappedY);
    }

    public void OnClickConstructBuilding()
    {
        if (_selectedBuildingData == null)
        {
            Debug.LogError("선택된 건물 데이터가 없습니다.");
            return;
        }

        //// 선택된 건물에서 필요한 건설 데이터를 가져오기
        int requiredCoin = _selectedBuildingData.RequiredCoin;
        //ItemData requiredMaterial = _selectedBuildingData.RequiredMaterial;
        //int requiredMaterialCount = _selectedBuildingData.RequiredMaterialCount;
        //ItemData requiredEquipment = _selectedBuildingData.RequiredEquipment;
        //int requiredEquipmentCount = _selectedBuildingData.RequiredEquipmentCount;
        //
        //// 플레이어가 충분한 자원을 가지고 있는지 확인
        bool hasEnoughCoin = GameManager.Instance.CurPlayerData.Coin >= requiredCoin;
        //bool hasEnoughMaterial = GameManager.Instance.PlayerInventory.GetItemCount(requiredMaterial.Key) >= requiredMaterialCount;
        //bool hasEnoughEquipment = GameManager.Instance.PlayerInventory.GetItemCount(requiredEquipment.Key) >= requiredEquipmentCount;
        //
        //if (!hasEnoughGold)
        //{
        //    Debug.LogError("건물을 건설하기에 코인이 부족합니다.");
        //    // 플레이어에게 오류 메시지 표시
        //    return;
        //}
        //
        //if (!hasEnoughMaterial)
        //{
        //    Debug.LogError("건물을 건설하기에 재료가 부족합니다.");
        //    // 플레이어에게 오류 메시지 표시
        //    return;
        //}
        //
        //if (!hasEnoughEquipment)
        //{
        //    Debug.LogError("건물을 건설하기에 장비가 부족합니다.");
        //    // 플레이어에게 오류 메시지 표시
        //    return;
        //}
        //
        //// 플레이어의 인벤토리에서 자원 차감
        GameManager.Instance.CurPlayerData.Coin -= requiredCoin;
        //GameManager.Instance.PlayerInventory.RemoveItem(requiredMaterial.Key, requiredMaterialCount);
        //GameManager.Instance.PlayerInventory.RemoveItem(requiredEquipment.Key, requiredEquipmentCount);

        SetMoneyValue();

        // 건물 건설 진행
        if (_tilemap != null)
        {
            _tilemap.ConstructBuilding();
            Debug.Log("건물이 성공적으로 건설되었습니다.");
            _selectedBuildingSkeletonAnimation.gameObject.SetActive(false);
        }
    }

    public void SetMoneyValue()
    {
        foreach (var coinText in _coinTexts) //Coin
        {
            string value = GameManager.Instance.CurPlayerData.Coin.ToString();
            coinText.text = value;
        }
        foreach (var diaText in _diaTexts) //Diamond
        {
            string value = GameManager.Instance.CurPlayerData.Diamond.ToString();
            diaText.text = value;
        }
    }

    public void SetExpValue()
    {
        float value = GameManager.Instance.CurPlayerData.Exp;
        _expBar.value = value/ _maxExp;
    }

    //private void CheckOverlapAndShowIndicators(Vector2 position)
    //{
    //    // 겹치는 사각형 표시 제거
    //    foreach (var indicator in _overlapIndicators)
    //    {
    //        Destroy(indicator);
    //    }
    //    _overlapIndicators.Clear();
    //
    //    // 선택된 빌딩의 위치 및 크기 가져오기
    //    var selectedBuildingBounds = SelectedBuilding.GetBounds();
    //    selectedBuildingBounds.center = position;
    //
    //    // 모든 빌딩과 현재 선택된 빌딩의 위치 및 크기 비교
    //    var buildings = FindObjectsOfType<Building>();
    //
    //    foreach (var building in buildings)
    //    {
    //        if (building == SelectedBuilding) continue;
    //
    //        var buildingBounds = building.GetBounds();
    //
    //        if (selectedBuildingBounds.Intersects(buildingBounds))
    //        {
    //            // 겹치는 부분 표시
    //            var overlapIndicator = Instantiate(_overlapIndicatorPrefab, building.transform.position, Quaternion.identity);
    //            overlapIndicator.transform.localScale = new Vector2(buildingBounds.size.x, buildingBounds.size.y);
    //            _overlapIndicators.Add(overlapIndicator);
    //
    //            building.SetOverlapColor(Color.blue);
    //        }
    //        else
    //        {
    //            building.SetOverlapColor(Color.clear);
    //        }
    //    }
    //}
}
