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
    
    public int Building() //������ �ǹ��� ������ Ű �� ��ȯ
    {
        if (_selectedBuildingUI == null)
            return 0;
    
        int key = _selectedBuildingUI.GetBuildingData().Key;

        //if(_selectedBuildingUI._curCount == _maxCount) //*���� ���� = �ִ� ���� -> ��Ȱ��ȭ*//
        _selectedBuildingUI.SetInActive(true);

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

    public void OnClickBuilding(BuildingData data) //��ġ�� �ǹ�
    {
        print("Building Click");
        _craftUI.gameObject.SetActive(true);
        _craftUI.SetData(data);
        _craftUI.CreateCraftItem(data);
        //_buildingInfoPanel.SetActive(true);
    }

    public void OnClickOkayBtn()
    {
        print("OkayBtn Click");
        _buildingInfoPanel.SetActive(false);
        //_craftUI.SetData(data);
        //_craftUI.gameObject.SetActive(true);
    }   //_craftUI.CreateCraftItem(data);
}
