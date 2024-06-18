using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemUI : MonoBehaviour
{
    [Header("Crafting Info")]
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
    public Image CraftingItemImage => _craftingImage;

    private CraftItemInfo? _craftItemInfo;
    private int _buildingKey;
    private bool _isCraftingComplete;

    private void OnEnable()
    {
        StartCoroutine(UpdateCraftingProgress());
    }

    public void CraftStart(CraftItemInfo craftItemInfo) // Crafting칸 세팅
    {
        _buildingKey = craftItemInfo.BuildingKey;
        _craftItemInfo = craftItemInfo;
        _craftingImage.sprite = _craftItemInfo.Value.ResultItem.Sprite;

        float remainingTime = TimeManager.Instance.GetRemainTime(_buildingKey, craftItemInfo.ResultItem.Key);
        if (remainingTime <= 0)
        {
            remainingTime = _craftItemInfo.Value.RequiredTime;
            TimeManager.Instance.AddTime(_buildingKey, craftItemInfo);
        }

        _timeProgressBar.maxValue = _craftItemInfo.Value.RequiredTime;
        _timeProgressBar.value = 0.0f;
        _timeText.text = TimeManager.ConvertTime((int)remainingTime);

        _craftingImage.gameObject.SetActive(true);
        _timeProgressBar.gameObject.SetActive(true);
        _fastBtn.gameObject.SetActive(true);
        _checkImage.gameObject.SetActive(false);
    }

    private IEnumerator UpdateCraftingProgress()
    {
        while (true)
        {
            if (_craftItemInfo.HasValue && !_isCraftingComplete)
            {
                float elapsedTime = _craftItemInfo.Value.RequiredTime - TimeManager.Instance.GetRemainTime(_buildingKey, _craftItemInfo.Value.ResultItem.Key);
                _timeProgressBar.value = elapsedTime;
                _timeText.text = TimeManager.ConvertTime((int)(_craftItemInfo.Value.RequiredTime - elapsedTime));

                if (_craftItemInfo.Value.RequiredTime - elapsedTime <= 0)
                {
                    OnCraftingComplete();
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnClickFastBtn() // Crafting-시간 축소
    {
        if (!_craftItemInfo.HasValue) return;

        print("FastBtn Click");
        float remainingTime = TimeManager.Instance.GetRemainTime(_buildingKey, _craftItemInfo.Value.ResultItem.Key);
        remainingTime -= 10000;

        if (remainingTime < 0)
        {
            remainingTime = 0;
        }

        TimeManager.Instance.AddTime(_buildingKey, new CraftItemInfo { ResultItem = _craftItemInfo.Value.ResultItem, RequiredTime = (int)remainingTime });
        float elapsedTime = _craftItemInfo.Value.RequiredTime - remainingTime;
        _timeProgressBar.value = elapsedTime;
        _timeText.text = TimeManager.ConvertTime((int)(_craftItemInfo.Value.RequiredTime - elapsedTime));
    }

    private void OnCraftingComplete()
    {
        if (_isCraftingComplete) return;

        print("Crafting Complete");
        _isCraftingComplete = true;
        _checkImage.gameObject.SetActive(true);
        _timeProgressBar.gameObject.SetActive(false);
    }

    public void OnClickCraftedItem()
    {
        if (!_craftingImage.gameObject.activeSelf) return;

        _craftingImage.gameObject.SetActive(false);
        _checkImage.gameObject.SetActive(false);
        GameManager.Instance.PlayerInventory.AddItem(_craftItemInfo.Value.ResultItem.Key, _craftItemInfo.Value.ResultCount);
        print($"Item {_craftItemInfo.Value.ResultItem.Name} added to inventory.");
    }
}
