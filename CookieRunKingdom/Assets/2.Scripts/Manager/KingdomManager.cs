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
    private Transform _cTypeBuildingContent; //�Ǽ� Ÿ��(constrctType)
    private Transform _dTypeBuildingContent; //�ٹ̱� Ÿ��(decorateType)
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
        //�Ǽ� Ÿ�� �ǹ� ĭ ����
        GameObject cTypeBuildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/StoreCTypeBuildingCell");

        foreach (int myBuilding in GameManager.Instance.MyBuildings)
        {
            BuildingData data = DataManager.Instance.GetBuildingData(myBuilding);

            GameObject buildingObj = Instantiate(cTypeBuildingPrefab, _cTypeBuildingContent);
        }

        //�ٹ̱� Ÿ�� �ǹ� ĭ ����
        GameObject dTypeBuildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/StoreDTypeBuildingCell");

        foreach (int myBuilding in GameManager.Instance.MyBuildings)
        {
            BuildingData data = DataManager.Instance.GetBuildingData(myBuilding);

            GameObject buildingObj = Instantiate(dTypeBuildingPrefab, _dTypeBuildingContent);
        }

        //�ǹ� ���� ĭ ����
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

    public void SelectBuilding(StoreBuildingUI buildingUI) //������ �ǹ� ���� ����
    {
        _storePanel.SetActive(false);
        _selectedBuildingUI = buildingUI;

        _selectedBuilding.gameObject.SetActive(true);
        _selectedBuilding.skeletonDataAsset = buildingUI.BuildingData.SkeletonDataAsset;
        _selectedBuilding.Initialize(true);
    }
    
    public int Building() //������ �ǹ��� ������ Ű �� ��ȯ
    {
        if (_selectedBuildingUI == null)
            return 0;
    
        int key = _selectedBuildingUI.BuildingData.Key;

        //if(_selectedBuildingUI._curCount == _maxCount) //*���� ���� = �ִ� ���� -> ��Ȱ��ȭ*//
        _selectedBuildingUI.SetInActive(true);

        _selectedBuildingUI = null;
        _selectedBuilding.gameObject.SetActive(false);
    
        return key;
    }

    public void OnClickConstructBtn() //Main-�Ǽ��ϱ�
    {
        print("ConstructBtn Click");
        _storePanel.SetActive(true);
    }

    public void OnClickStoreExitBtn() //Store-������
    {
        print("StoreExitBtn Click");
        _storePanel.SetActive(false);
    }

    public void OnClickConstructTypeBtn() //Store-�Ǽ�
    {
        print("ConstructTypeBtn Click");
        _cTypeBuildingContent.gameObject.SetActive(true);
        _dTypeBuildingContent.gameObject.SetActive(false);
    }

    public void OnClickDecorateTypeBtn() //Store-�ٹ̱�
    {
        print("DecorateTypeBtn Click");
        _cTypeBuildingContent.gameObject.SetActive(false);
        _dTypeBuildingContent.gameObject.SetActive(true);
    }
}
