using System.Collections;
using System.Collections.Generic;
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

    private Transform _buildingBtnContent;
    private GameObject _selectedBuilding;
    private BuildingBtn _selectedBuildingBtn;
    private SkeletonAsset _selectedBuildingSA;

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
        _selectedBuilding.gameObject.SetActive(false);

    }

    private void Update()
    {
        _selectedBuilding.transform.position = Input.mousePosition;
    }

    public void SelectBuilding(BuildingBtn buildingBtn)
    {
        _selectedBuildingSA = _selectedBuilding.GetComponent<SkeletonAsset>();

        _storePanel.SetActive(false);
        _selectedBuildingBtn = buildingBtn;
    
        string file = buildingBtn.BuildingData.Name + "_SkeletonData";
        SkeletonAsset skas = Resources.Load<SkeletonAsset>(file);
        _selectedBuildingSA = skas;
        _selectedBuilding.SetActive(true);
    }
    
    public int Building()
    {
        if (_selectedBuildingBtn == null)
            return 0;
    
        int key = _selectedBuildingBtn.BuildingData.Key;
    
        //_selectedBuildingBtn.SetActive(true);  //?//Sibling..을 어케 접근하지
        _selectedBuildingBtn = null;
        _selectedBuilding.gameObject.SetActive(false);
    
        return key;
    }

    public void OnClickConstructBtn()
    {
        _storePanel.SetActive(true);
    }
}
