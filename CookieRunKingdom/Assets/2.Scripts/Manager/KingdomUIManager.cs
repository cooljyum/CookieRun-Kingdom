using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingdomUIManager : MonoBehaviour
{
    static public KingdomUIManager Instance;

    [SerializeField]
    private GameObject _storePanel;    

    private void Awake()
    {
        Instance = this;

        
    }

    //private void Start()
    //{
    //    for (int i = 0; i < _buildingList.Count; i++)
    //    {
    //        GameObject btn = Instantiate(_buildingBtnPrefab, )
    //        btn.GetComponent<BuildingButton>().SetData(key);
    //
    //    }
    //
    //    _selectBuilding.transform.position = Input.mousePosition;
    //
    //}
    //
    //private void Update()
    //{
    //    
    //}
    //
    //public void SelectBuilding(BuildingButton buildingBtn)
    //{
    //    _selectBuildingBtn = buildingBtn;
    //
    //    string file = "Textures/Characters/" + buildingBtn.CharacterData.SpriteImage;
    //    Sprite sprite = Resources.Load<Sprite>(file);
    //    _selectBuilding.sprite = sprite;
    //    _selectBuilding.gameObject.SetActive(true);
    //}
    //
    //public int Build()
    //{
    //    if (_selectBuildingBtn == null)
    //        return 0;
    //
    //    int key = _selectBuildingBtn.Data.Key;
    //
    //    Destroy(_selectBuildingBtn.gameObject);
    //    _selectBuildingBtn = null;
    //    _selectBuilding.gameObject.SetActive(false);
    //
    //    return key;
    //}

    public void OnClickBuildingBtn()
    {
        _storePanel.SetActive(true);
    }
}
