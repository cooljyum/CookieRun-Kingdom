using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    [SerializeField]
    private Transform _itemCellContent;
    [SerializeField]
    private Transform _craftingContent;
    [SerializeField]
    private TextMeshProUGUI _buildingName;
    [SerializeField]
    private SkeletonGraphic _buildingImage;
    [SerializeField]
    private TextMeshProUGUI _coinText;
    [SerializeField]
    private TextMeshProUGUI _diaText;
    [SerializeField]
    private Image _curMaterialImage;
    [SerializeField]
    private TextMeshProUGUI _curMaterialAmount;
    [SerializeField]
    private GameObject _curMaterialObject;

    private GameObject _itemPrefab;
    private BuildingData _buildingData;
    private Image _craftingImage;

    private void Awake()
    {
        _itemPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/CraftItemCell");
    }

    public void SetData(BuildingData buildingData)
    {
        _buildingData = buildingData;
        _buildingName.text = buildingData.Name.ToString();
        _buildingImage.skeletonDataAsset = buildingData.SkeletonDataAsset;
        _buildingImage.Initialize(true);

        if (buildingData.Key / 10 < 10) //1n번대 -> 재료
        {
            _curMaterialImage.sprite = buildingData.CraftInfos[0].ResultItem.Sprite;
            _curMaterialAmount.text = GameManager.Instance.PlayerInventory.GetItemCount(buildingData.CraftInfos[0].ResultItem.Key).ToString();
        }
        else
        {
            _curMaterialObject.SetActive(false);
        }
    }

    public void CreateCraftItem(BuildingData data) //건물 데이터에 맞춰 생산 칸 삭제 & 생성
    {
        for (int i = _itemCellContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_itemCellContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < data.CraftInfos.Count; i++)
        {
            GameObject itemObj = Instantiate(_itemPrefab, _itemCellContent);
            itemObj.GetComponent<CraftItemUI>().SetData(data.CraftInfos[i]);
        }
    }

    public void CraftStart(CraftItemInfo craftItemInfo)
    {
        foreach (Transform craftingCell in _craftingContent)
        {
            var craftingItemUI = craftingCell.GetComponent<CraftingItemUI>();
            if (!craftingItemUI.CraftingItemImage.gameObject.activeSelf)
            {
                craftingItemUI.CraftStart(_buildingData.Key, craftItemInfo);
                break;
            }
        }
    }

    public void OnClickExitBtn() //Craft-나가기
    {
        print("ExitBtn Click");
        gameObject.SetActive(false);
    }
}
