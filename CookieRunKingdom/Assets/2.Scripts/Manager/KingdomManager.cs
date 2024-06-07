using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class KingdomManager : MonoBehaviour
{
    static public KingdomManager Instance;

    [SerializeField]
    private GameObject _storePanel;

    [SerializeField]
    private List<GameObject> _myBuildingList = new List<GameObject>(); //내 빌딩 리스트

    private GameObject _selectedBuilding;
    private BuildingBtn _selectedBuildingBtn;
    private SkeletonAnimation _selectedBuildingSA;
    private Transform _buildingBtnContent;
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

            if (name == "Content")
            {
                _buildingBtnContent = child;
            }

        }
    }

    private void Start()
    {
        _selectedBuilding = GameObject.Find("SelectedBuilding");
        _selectedBuilding.SetActive(false);
        _selectedBuildingSA = _selectedBuilding.GetComponent<SkeletonAnimation>();

    }

    private void OnEnable()
    {
        StartCoroutine(BuildingControl());
    }

    private void Update()
    {

    }

    public void SelectBuilding(BuildingBtn buildingBtn)
    {
        _storePanel.SetActive(false);
        _selectedBuildingBtn = buildingBtn;
        
        string file = "SkeletonData/" + buildingBtn.BuildingData.Name + "_SkeletonData";
        SkeletonAnimation skas = Resources.Load<SkeletonAnimation>(file);
        _selectedBuildingSA = skas;
        _selectedBuilding.SetActive(true);
    }
    
    public int Building()
    {
        if (_selectedBuildingBtn == null)
            return 0;
    
        int key = _selectedBuildingBtn.BuildingData.Key;
    
        _selectedBuildingBtn.transform.parent.GetChild(4).gameObject.SetActive(true);  //이렇게 접근하는 게 맞나
        _selectedBuildingBtn = null;
        _selectedBuilding.SetActive(false);
    
        return key;
    }

    public void OnClickConstructBtn()
    {
        print("Btn Click");
        _storePanel.SetActive(true);
    }

    private IEnumerator BuildingControl()
    {
        while (_selectedBuilding != null) 
        {
            //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _selectedBuilding.transform.position = Input.mousePosition;

            yield return null;
        }
    }
}
