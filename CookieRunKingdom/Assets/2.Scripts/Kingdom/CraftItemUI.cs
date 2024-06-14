using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItemUI : MonoBehaviour
{
    [SerializeField]
    private Transform _craftingContent;
    [SerializeField]
    private GameObject _materialSetting;
    [SerializeField]
    private GameObject _equipmentSetting;
    [Header("Information")]
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemNameText;
    [SerializeField]
    private TextMeshProUGUI _requiredTimeText;
    [Header("Material")]
    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private Image _resultImage;
    [SerializeField]
    private TextMeshProUGUI _resultAmountText;
    [Header("Equipment")]
    [SerializeField]
    private Image _requiredMaterial1Image;
    [SerializeField]
    private TextMeshProUGUI _requiredMaterial1AmountText;
    [SerializeField]
    private Image _requiredMaterial2Image;
    [SerializeField]
    private TextMeshProUGUI _requiredMaterial2AmountText;
    [SerializeField]
    private TextMeshProUGUI _curAmount;
    [Header("Crafting")]
    [SerializeField]
    private Image _craftingImage;
    [SerializeField]
    private Image _checkImage;
    [SerializeField]
    private Slider _timeProgressBar;
    [SerializeField]
    private Button _fastBtn;
    [SerializeField]
    private TextMeshProUGUI _timeText;

    private BuildingData _buildingData;
    private CraftItemInfo _craftItemInfo;
    private int _craftIndex;
    private int _itemKey;

    public Dictionary<ItemData, int> myItems; //?//

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    public void SetData(CraftItemInfo craftItemInfo) //생성한 Craft 칸 데이터 세팅
    {
        _craftItemInfo = craftItemInfo;

        _itemImage.sprite = craftItemInfo.ResultItem.Sprite;

        if (craftItemInfo.IsMaterial) //재료 세팅
        {
            _materialSetting.SetActive(true);
            _equipmentSetting.SetActive(false);
            _costText.text = craftItemInfo.Cost.ToString();
            _resultImage.sprite = craftItemInfo.ResultItem.Sprite;
            _resultAmountText.text = craftItemInfo.ResultCount.ToString();            
        }
        else //도구 세팅
        {
            _materialSetting.SetActive(false);
            _equipmentSetting.SetActive(true);
            ItemData itemData = new ItemData();
            _requiredMaterial1Image.sprite = itemData.GetItemSprite(craftItemInfo.ResultItem.MaterialKeys[0]);
            _requiredMaterial1AmountText.text = craftItemInfo.ResultItem.MateiralAmounts[0].ToString();
            _requiredMaterial2Image.sprite = itemData.GetItemSprite(craftItemInfo.ResultItem.MaterialKeys[1]);
            _requiredMaterial2AmountText.text = craftItemInfo.ResultItem.MateiralAmounts[1].ToString();
            //_curAmount.text;
        }


    }

    private void Update()
    {
        
    }

    public void CraftStart() //Crafting칸 세팅
    {
        _craftingImage.sprite = _craftItemInfo.ResultItem.Sprite;
        _timeProgressBar.value = _craftItemInfo.RequiredTime;
        
        _timeProgressBar.gameObject.SetActive(true);
    }

    public void OnClickCraftBtn() //Craft-제작
    {
        print("CraftBtn Click");
        CraftStart();
    }

    public void OnClickFastBtn() //Crafting-시간 축소
    {
        print("FastBtn Click");
        _timeProgressBar.value -= 10000f;
    }
}
