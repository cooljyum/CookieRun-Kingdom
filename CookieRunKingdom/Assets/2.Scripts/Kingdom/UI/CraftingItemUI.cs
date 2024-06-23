using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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

    public void CraftStart(CraftItemInfo craftItemInfo) // Craftingĭ ����
    {
        _buildingKey = craftItemInfo.BuildingKey;
        _craftItemInfo = craftItemInfo;
        _craftingImage.sprite = _craftItemInfo.Value.ResultItem.Sprite;

        //���� �ð� ����
        float remainingTime = TimeManager.Instance.GetRemainingTime(_buildingKey, craftItemInfo.ResultItem.Key);
        if (remainingTime <= 0)
        {
            remainingTime = _craftItemInfo.Value.RequiredTime;
            TimeManager.Instance.AddTime(_buildingKey, craftItemInfo);
        }

        _timeProgressBar.maxValue = _craftItemInfo.Value.RequiredTime;
        _timeProgressBar.value = _craftItemInfo.Value.RequiredTime - remainingTime;
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
                //��� �ð� ��� -> �� ����
                float elapsedTime = _craftItemInfo.Value.RequiredTime - TimeManager.Instance.GetRemainingTime(_buildingKey, _craftItemInfo.Value.ResultItem.Key);
                _timeProgressBar.value = elapsedTime;
                _timeText.text = TimeManager.ConvertTime((int)(_craftItemInfo.Value.RequiredTime - elapsedTime));

                //���� �Ϸ� ���� Ȯ��
                if (_craftItemInfo.Value.RequiredTime - elapsedTime <= 0)
                {
                    OnCraftingComplete();
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnClickFastBtn() // Crafting-�ð� ���
    {
        if (!_craftItemInfo.HasValue) return;

        Debug.Log("FastBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        float remainingTime = TimeManager.Instance.GetRemainingTime(_buildingKey, _craftItemInfo.Value.ResultItem.Key);
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

        Debug.Log("Crafting Complete");
        _isCraftingComplete = true;
        _checkImage.gameObject.SetActive(true);
        _timeProgressBar.gameObject.SetActive(false);
    }

    public void OnClickCraftedItem() //Crafting-����� ������
    {
        if (!_craftingImage.gameObject.activeSelf) return;
        if (!_checkImage.gameObject.activeSelf) return;

        //_craftingItems ����Ʈ���� ������ ����
        Building currentBuilding = KingdomManager.Instance.SelectedBuilding;
        if (currentBuilding != null && _craftItemInfo.HasValue)
        {
            currentBuilding.CraftingItems.Remove(_craftItemInfo.Value);
        }

        _craftingImage.gameObject.SetActive(false);
        _checkImage.gameObject.SetActive(false);

        //������ �κ��丮�� �߰�
        GameManager.Instance.PlayerInventory.AddItem(_craftItemInfo.Value.ResultItem.Key, _craftItemInfo.Value.ResultCount);
        Debug.Log($"Item {_craftItemInfo.Value.ResultItem.Name} added to inventory.");

        //�÷��̾� ������ ����
        GameManager.Instance.SavePlayerData();
    }

    public void ResetData()
    {
        _craftItemInfo = null;
        _craftingImage.gameObject.SetActive(false);
        _checkImage.gameObject.SetActive(false);
        _timeProgressBar.gameObject.SetActive(false);
    }
    
    public void SetCraftingItem(CraftItemInfo craftItemInfo) //���� ���̾��� �������� ������ ����
    {
        _buildingKey = craftItemInfo.BuildingKey;
        _craftItemInfo = craftItemInfo;
        _craftingImage.sprite = _craftItemInfo.Value.ResultItem.Sprite;

        //���� �ð� ��� �� ����
        float remainingTime = TimeManager.Instance.GetRemainingTime(_buildingKey, craftItemInfo.ResultItem.Key);
        _timeProgressBar.maxValue = _craftItemInfo.Value.RequiredTime;
        _timeProgressBar.value = _craftItemInfo.Value.RequiredTime - remainingTime;
        _timeText.text = TimeManager.ConvertTime((int)remainingTime);

        if (remainingTime == 0)
        {
            _isCraftingComplete = true;
            _timeProgressBar.gameObject.SetActive(false);
            _fastBtn.gameObject.SetActive(false);
            _checkImage.gameObject.SetActive(true);
        }
        else
        {
            _isCraftingComplete = false;
            _timeProgressBar.gameObject.SetActive(true);
            _fastBtn.gameObject.SetActive(true);
            _checkImage.gameObject.SetActive(false);
        }

        _craftingImage.gameObject.SetActive(true);
    }
}
