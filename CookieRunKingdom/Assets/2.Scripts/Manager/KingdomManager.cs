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
    private float _maxExp = 1000f; //���� ������ȭ ����

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

        //��ġ�� ���� ǥ��
        //CheckOverlapAndShowIndicators(worldPos);
    }

    public void SelectDTypeBuilding(StoreBuildingUI buildingUI) //������ DŸ�� �ǹ� ���� ����
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingUI = buildingUI;

        _selectedBuildingSkeletonAnimation.gameObject.SetActive(true);
        _selectedBuildingSkeletonAnimation.skeletonDataAsset = buildingUI.GetBuildingData().SkeletonDataAsset;
        _selectedBuildingSkeletonAnimation.Initialize(true);
        _editUI.SetActive(true);
    }

    public void SelectCTypeBuilding(BuildingData data) //������ CŸ�� �ǹ� ���� ����
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingData = data;

        _selectedBuildingSkeletonAnimation.gameObject.SetActive(true);
        _selectedBuildingSkeletonAnimation.skeletonDataAsset = _selectedBuildingData.SkeletonDataAsset;
        _selectedBuildingSkeletonAnimation.Initialize(true);
        _editUI.SetActive(true);
    }

    public int Building() //������ �ǹ��� ������ Ű �� ��ȯ
    {
        int key = 0;

        if (_selectedBuildingUI != null)
        {
            _selectedBuildingData = _selectedBuildingUI.GetBuildingData();
            key = _selectedBuildingData.Key;
            Debug.Log($"Selected Building Key (from UI): {key}");

            //�ǹ� ��ġ ���� ����
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

            //�ǹ� ��ġ ���� ����
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

    public void OnClickConstructBtn() //Main-�Ǽ��ϱ�
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

    public void OnClickGatchaBtn() //Main-�̱�
    {
        Debug.Log("GatchaBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        SceneManager.LoadScene("GachaScene");
    }

    public void OnClickCookieKingdomBtn() //Play-��Ű �ձ�
    {
        Debug.Log("CookieKingdomBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        _kingdomPlayPanel.gameObject.SetActive(false);
    }

    public void OnClickWorldAdventureBtn() //Play-���� Ž��
    {
        Debug.Log("WorldAdventureBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        SceneManager.LoadScene("ReadyScene");
    }

    public void OnClickPlayExitBtn() //Play-������
    {
        Debug.Log("PlayExitBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        _kingdomPlayPanel.gameObject.SetActive(false);
    }

    public void ClickStoreBuildingBtn(BuildingData data)
    {
        _buildingInfoPanel.SetData(data);
    }

    public void OnClickBuilding(Building building) //��ġ�� �ǹ�
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

    public void ClickCraftBtn(CraftItemInfo craftItemInfo) //���� ��ư Ŭ�� �� ȣ�� -> ���� ����
    {
        _craftUI.CraftStart(craftItemInfo);
    }

    public void SetSelectedBuilding(BuildingData buildingData)
    {
        _selectedBuildingData = buildingData;
    }

    public void SetSnappedPosition()
    {
        Vector2 originalPosition = _selectedBuildingSkeletonAnimation.transform.position;
        float tileSizeX = 2.6f;
        float tileSizeY = 1.33f;

        // ���� ��ġ�� Ÿ�� �׸��� ��ġ�� ����
        float snappedX = Mathf.Round(originalPosition.x / tileSizeX) * tileSizeX;
        float snappedY = Mathf.Round(originalPosition.y / tileSizeY) * tileSizeY;

        SelectedPosition = new Vector2(snappedX, snappedY);
    }

    public void OnClickConstructBuilding()
    {
        if (_selectedBuildingData == null)
        {
            Debug.LogError("���õ� �ǹ� �����Ͱ� �����ϴ�.");
            return;
        }

        // ���õ� �ǹ����� �ʿ��� �Ǽ� �����͸� ��������
        int requiredCoin = _selectedBuildingData.RequiredCoin;
        ItemData requiredMaterial = _selectedBuildingData.RequiredMaterial;
        int requiredMaterialCount = _selectedBuildingData.RequiredMaterialCount;
        ItemData requiredEquipment = _selectedBuildingData.RequiredEquipment;
        int requiredEquipmentCount = _selectedBuildingData.RequiredEquipmentCount;
        
        // �÷��̾ ����� �ڿ��� ������ �ִ��� Ȯ��
        bool hasEnoughCoin = GameManager.Instance.CurPlayerData.Coin >= requiredCoin;
        bool hasEnoughMaterial = GameManager.Instance.PlayerInventory.GetItemCount(requiredMaterial.Key) >= requiredMaterialCount;
        bool hasEnoughEquipment = GameManager.Instance.PlayerInventory.GetItemCount(requiredEquipment.Key) >= requiredEquipmentCount;
        
        if (!hasEnoughCoin)
        {
            Debug.LogError("�ǹ��� �Ǽ��ϱ⿡ ������ �����մϴ�.");
            // �÷��̾�� ���� �޽��� ǥ��
            return;
        }
        
        if (!hasEnoughMaterial)
        {
            Debug.LogError("�ǹ��� �Ǽ��ϱ⿡ ��ᰡ �����մϴ�.");
            // �÷��̾�� ���� �޽��� ǥ��
            return;
        }
        
        if (!hasEnoughEquipment)
        {
            Debug.LogError("�ǹ��� �Ǽ��ϱ⿡ ��� �����մϴ�.");
            // �÷��̾�� ���� �޽��� ǥ��
            return;
        }
        
        // �÷��̾��� �κ��丮���� �ڿ� ����
        GameManager.Instance.CurPlayerData.Coin -= requiredCoin;
        GameManager.Instance.PlayerInventory.RemoveItem(requiredMaterial.Key, requiredMaterialCount);
        GameManager.Instance.PlayerInventory.RemoveItem(requiredEquipment.Key, requiredEquipmentCount);

        SetMoneyValue();

        // �ǹ� �Ǽ� ����
        if (_tilemap != null)
        {
            _tilemap.ConstructBuilding();
            Debug.Log("�ǹ��� ���������� �Ǽ��Ǿ����ϴ�.");
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
    //    // ��ġ�� �簢�� ǥ�� ����
    //    foreach (var indicator in _overlapIndicators)
    //    {
    //        Destroy(indicator);
    //    }
    //    _overlapIndicators.Clear();
    //
    //    // ���õ� ������ ��ġ �� ũ�� ��������
    //    var selectedBuildingBounds = SelectedBuilding.GetBounds();
    //    selectedBuildingBounds.center = position;
    //
    //    // ��� ������ ���� ���õ� ������ ��ġ �� ũ�� ��
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
    //            // ��ġ�� �κ� ǥ��
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
