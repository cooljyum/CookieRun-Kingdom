using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CraftingItemUI : MonoBehaviour  //* ���� ���� ������ UI *//
{
    [Header("Crafting Info")]
    [SerializeField]
    public Image CraftingItemImage; //������ �̹���
    [SerializeField]
    private Image _checkImage; //üũ ǥ�� �̹��� (�Ϸ�� �� Ȱ��ȭ)
    [SerializeField]
    private Slider _timeProgressBar; //���� �ð� ��
    [SerializeField]
    private Button _fastBtn; //�ð� ���� ��ư
    [SerializeField]
    private TextMeshProUGUI _timeText; //���� �ð�

    private CraftItemInfo? _craftItemInfo; //������ ���� ����
    private int _buildingKey; //�ǹ� Ű
    private bool _isCraftingComplete; //���� �Ϸ� ����

    private void OnEnable()
    {
        StartCoroutine(UpdateCraftingProgress());
    }

    public void CraftStart(CraftItemInfo craftItemInfo) // Craftingĭ ����
    {
        _buildingKey = craftItemInfo.BuildingKey;
        _craftItemInfo = craftItemInfo;
        CraftingItemImage.sprite = _craftItemInfo.Value.ResultItem.Sprite;

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

        CraftingItemImage.gameObject.SetActive(true);
        _timeProgressBar.gameObject.SetActive(true);
        _fastBtn.gameObject.SetActive(true);
        _checkImage.gameObject.SetActive(false);
    }

    private IEnumerator UpdateCraftingProgress() //���� ���� ��Ȳ ������Ʈ �ڷ�ƾ
    {
        while (true)
        {
            //���� ���� ���� �������� �ְ�, ������ �Ϸ���� �ʾ��� ����
            if (_craftItemInfo.HasValue && !_isCraftingComplete)
            {
                //��� �ð� ��� -> �� ����
                float elapsedTime = _craftItemInfo.Value.RequiredTime 
                    - TimeManager.Instance.GetRemainingTime(_buildingKey, _craftItemInfo.Value.ResultItem.Key);
                _timeProgressBar.value = elapsedTime;
                _timeText.text = TimeManager.ConvertTime((int)(_craftItemInfo.Value.RequiredTime - elapsedTime));

                //���� �Ϸ� ���� Ȯ��
                if (_craftItemInfo.Value.RequiredTime - elapsedTime <= 0)
                {
                    OnCraftingComplete(); //���� �Ϸ�
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnClickFastBtn() // Crafting-�ð� ����
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

    private void OnCraftingComplete() //���� �Ϸ�
    {
        if (_isCraftingComplete) return;

        Debug.Log("Crafting Complete");
        _isCraftingComplete = true;
        _checkImage.gameObject.SetActive(true);
        _timeProgressBar.gameObject.SetActive(false);
    }

    public void OnClickCraftedItem() //Crafting-����� ������
    {
        if (!CraftingItemImage.gameObject.activeSelf) return;
        if (!_checkImage.gameObject.activeSelf) return;

        //_craftingItems ����Ʈ���� ������ ����
        Building currentBuilding = KingdomManager.Instance.ClickedBuilding;
        if (currentBuilding != null && _craftItemInfo.HasValue)
        {
            currentBuilding.CraftingItems.Remove(_craftItemInfo.Value);
        }

        CraftingItemImage.gameObject.SetActive(false);
        _checkImage.gameObject.SetActive(false);

        //������ �κ��丮�� �߰�
        GameManager.Instance.PlayerInventory.AddItem(_craftItemInfo.Value.ResultItem.Key,
            _craftItemInfo.Value.ResultCount);
        Debug.Log($"Item {_craftItemInfo.Value.ResultItem.Name} added to inventory.");

        //�÷��̾� �����ͷ� ����
        GameManager.Instance.SavePlayerData();
    }

    public void ClearCraftingItem() //���� ���� ������ �����
    {
        _craftItemInfo = null;
        CraftingItemImage.gameObject.SetActive(false);
        _checkImage.gameObject.SetActive(false);
        _timeProgressBar.gameObject.SetActive(false);
    }
    
    public void SetCraftingItem(CraftItemInfo craftItemInfo) //���� ���� �������� ������ ����
    {
        _buildingKey = craftItemInfo.BuildingKey;
        _craftItemInfo = craftItemInfo;
        CraftingItemImage.sprite = _craftItemInfo.Value.ResultItem.Sprite;

        //���� �ð� ��� �� ����
        float remainingTime = TimeManager.Instance.GetRemainingTime(_buildingKey, craftItemInfo.ResultItem.Key);
        _timeProgressBar.maxValue = _craftItemInfo.Value.RequiredTime;
        _timeProgressBar.value = _craftItemInfo.Value.RequiredTime - remainingTime;
        _timeText.text = TimeManager.ConvertTime((int)remainingTime);

        if (remainingTime == 0) //���� �Ϸ� ?
        {
            _isCraftingComplete = true;
            _timeProgressBar.gameObject.SetActive(false);
            _fastBtn.gameObject.SetActive(false);
            _checkImage.gameObject.SetActive(true);
        }
        else //���� ���� �� ?
        {
            _isCraftingComplete = false;
            _timeProgressBar.gameObject.SetActive(true);
            _fastBtn.gameObject.SetActive(true);
            _checkImage.gameObject.SetActive(false);
        }

        CraftingItemImage.gameObject.SetActive(true);
    }
}
