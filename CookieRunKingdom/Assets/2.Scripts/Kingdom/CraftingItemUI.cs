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

    private CraftItemInfo _craftItemInfo;
    private Stopwatch _stopwatch;
    private float _requiredTime;

    private void OnEnable()
    {
        _stopwatch = new Stopwatch();
        StartCoroutine(Crafting());
    }

    public void CraftStart(CraftItemInfo craftItemInfo) //Crafting칸 세팅
    {
        _craftItemInfo = craftItemInfo;
        _craftingImage.sprite = _craftItemInfo.ResultItem.Sprite;
        _requiredTime = _craftItemInfo.RequiredTime;
        _timeText.text = TimeManager.ConvertTime(_craftItemInfo.RequiredTime);
        _timeProgressBar.maxValue = _requiredTime;
        _timeProgressBar.value = _requiredTime;
        TimeManager.Instance.AddTime(0, craftItemInfo);
        _craftingImage.gameObject.SetActive(true);
        _timeProgressBar.gameObject.SetActive(true);

        _stopwatch.Start();
    }

    public IEnumerator Crafting() //Crafting 시간 세팅
    {
        while (_stopwatch.IsRunning)
        {
            float elapsedSeconds = (float)_stopwatch.Elapsed.TotalSeconds;
            _timeProgressBar.value = Mathf.Max(_requiredTime - elapsedSeconds, 0);
            _timeText.text = TimeManager.ConvertTime((int)_timeProgressBar.value);

            if (_timeProgressBar.value <= 0)
            {
                _stopwatch.Stop();
                OnCraftingComplete();
            }

            yield return null;
        }
    }

    public void OnClickFastBtn() //Crafting-시간 축소
    {
        print("FastBtn Click");
        float newRequiredTime = _requiredTime - 10000;
        if (newRequiredTime < 0)
        {
            newRequiredTime = 0;
        }

        _requiredTime = newRequiredTime;
        _timeProgressBar.value = _requiredTime;
        _timeText.text = TimeManager.ConvertTime((int)_requiredTime);

        if (_requiredTime > 0)
        {
            _stopwatch.Restart();
        }
        else
        {
            OnCraftingComplete();
        }
    }

    private void OnCraftingComplete()
    {
        print("Crafting Complete");
        _checkImage.gameObject.SetActive(true);
        _timeProgressBar.gameObject.SetActive(false);
        _fastBtn.gameObject.SetActive(false);
    }

    public void OnClickCraftedItem()
    {
        if (!_craftingImage.gameObject.activeSelf) return;

        _craftingImage.gameObject.SetActive(false);
        _checkImage.gameObject.SetActive(false);
        GameManager.Instance.PlayerInventory.AddItem(_craftItemInfo.ResultItem.Key, _craftItemInfo.ResultCount);
        print($"Item {_craftItemInfo.ResultItem.Name} added to inventory.");
    }
}
