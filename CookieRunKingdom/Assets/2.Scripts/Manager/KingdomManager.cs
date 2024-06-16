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

    public void SelectBuilding(StoreBuildingUI buildingUI) //������ �ǹ� ���� ����
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingUI = buildingUI;

        _selectedBuilding.gameObject.SetActive(true);
        _selectedBuilding.skeletonDataAsset = buildingUI.GetBuildingData().SkeletonDataAsset;
        _selectedBuilding.Initialize(true);
    }

    public void SelectBuilding(BuildingData data) //������ �ǹ� ���� ����
    {
        _storeUI.gameObject.SetActive(false);
        _selectedBuildingData = data;

        _selectedBuilding.gameObject.SetActive(true);
        _selectedBuilding.skeletonDataAsset = _selectedBuildingData.SkeletonDataAsset;
        _selectedBuilding.Initialize(true);
    }

    public int Building() //������ �ǹ��� ������ Ű �� ��ȯ
    {
        if (_selectedBuildingUI == null)
            return 0;

        int key = _selectedBuildingUI.GetBuildingData().Key;

        //�ǹ� ��ġ ���� ����
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

    public void OnClickConstructBtn() //Main-�Ǽ��ϱ�
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

    public void OnClickBuilding(BuildingData data) //��ġ�� �ǹ�
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
