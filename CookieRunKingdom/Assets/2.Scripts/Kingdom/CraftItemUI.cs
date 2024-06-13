using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItemUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _materialSetting;

    [SerializeField]
    private GameObject _equipmentSetting;

    //공통
    private Image _itemImage;
    private TextMeshProUGUI _itemNameText;
    private TextMeshProUGUI _requiredTimeText;
    //재료
    private TextMeshProUGUI _costText;
    private Image _resultImage;
    private TextMeshProUGUI _resultAmountText;
    //장비
    private Image _requiredMaterial1Image;
    private TextMeshProUGUI _requiredMaterial1AmountText;
    private Image _requiredMaterial2Image;
    private TextMeshProUGUI _requiredMaterial2AmountText;
    private TextMeshProUGUI _curAmount;

    private BuildingData _buildingData;
    private ItemData _itemData;
    private int _craftIndex;
    private int _itemKey;

    private void Awake()
    {
        Image[] itemImages = transform.GetChild(0).GetComponentsInChildren<Image>();

        if (itemImages.Length > 1)
        {
            _itemImage = itemImages[1];
        }
        _itemNameText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        _requiredTimeText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();

        _costText = _materialSetting.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        _resultImage = _materialSetting.transform.GetChild(1).GetComponentInChildren<Image>();
        _resultAmountText = _materialSetting.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        Image[] materialImages = _equipmentSetting.transform.GetChild(1).GetComponentsInChildren<Image>();

        if (materialImages.Length > 1)
        {
            _requiredMaterial1Image = materialImages[0];
            _requiredMaterial2Image = materialImages[1];
        }

        TextMeshProUGUI[] materialTexts = _equipmentSetting.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

        if (materialTexts.Length > 1)
        {
            _requiredMaterial1AmountText = materialTexts[0];
            _requiredMaterial2AmountText = materialTexts[1];
        }
        _curAmount = _equipmentSetting.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
    }

    public void SetData(CraftItemInfo craftItemInfo)
    {
        if (craftItemInfo.IsMaterial)
        {
            _materialSetting.SetActive(true);
            _equipmentSetting.SetActive(false);

        }
        else
        {
            _materialSetting.SetActive(false);
            _equipmentSetting.SetActive(true);


        }
    }

    public void OnClickCraftBtn() //Craft-제작
    {
        print("CraftBtn Click");
        //CraftStart()
    }

}
