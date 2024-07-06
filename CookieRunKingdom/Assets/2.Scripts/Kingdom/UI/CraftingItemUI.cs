using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CraftingItemUI : MonoBehaviour  //* 생산 중인 아이템 UI *//
{
    [Header("Crafting Info")]
    [SerializeField]
    public Image CraftingItemImage; //아이템 이미지
    [SerializeField]
    private Image _checkImage; //체크 표시 이미지 (완료될 때 활성화)
    [SerializeField]
    private Slider _timeProgressBar; //생산 시간 바
    [SerializeField]
    private Button _fastBtn; //시간 단축 버튼
    [SerializeField]
    private TextMeshProUGUI _timeText; //생산 시간

    private CraftItemInfo? _craftItemInfo; //아이템 생산 정보
    private int _buildingKey; //건물 키
    private bool _isCraftingComplete; //생산 완료 여부

    private void OnEnable()
    {
        StartCoroutine(UpdateCraftingProgress());
    }

    public void CraftStart(CraftItemInfo craftItemInfo) // Crafting칸 세팅
    {
        _buildingKey = craftItemInfo.BuildingKey;
        _craftItemInfo = craftItemInfo;
        CraftingItemImage.sprite = _craftItemInfo.Value.ResultItem.Sprite;

        //남은 시간 세팅
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

    private IEnumerator UpdateCraftingProgress() //생산 진행 상황 업데이트 코루틴
    {
        while (true)
        {
            //현재 제작 중인 아이템이 있고, 제작이 완료되지 않았을 때만
            if (_craftItemInfo.HasValue && !_isCraftingComplete)
            {
                //경과 시간 계산 -> 값 세팅
                float elapsedTime = _craftItemInfo.Value.RequiredTime 
                    - TimeManager.Instance.GetRemainingTime(_buildingKey, _craftItemInfo.Value.ResultItem.Key);
                _timeProgressBar.value = elapsedTime;
                _timeText.text = TimeManager.ConvertTime((int)(_craftItemInfo.Value.RequiredTime - elapsedTime));

                //제작 완료 여부 확인
                if (_craftItemInfo.Value.RequiredTime - elapsedTime <= 0)
                {
                    OnCraftingComplete(); //제작 완료
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnClickFastBtn() // Crafting-시간 단축
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

    private void OnCraftingComplete() //생산 완료
    {
        if (_isCraftingComplete) return;

        Debug.Log("Crafting Complete");
        _isCraftingComplete = true;
        _checkImage.gameObject.SetActive(true);
        _timeProgressBar.gameObject.SetActive(false);
    }

    public void OnClickCraftedItem() //Crafting-생산된 아이템
    {
        if (!CraftingItemImage.gameObject.activeSelf) return;
        if (!_checkImage.gameObject.activeSelf) return;

        //_craftingItems 리스트에서 아이템 제거
        Building currentBuilding = KingdomManager.Instance.ClickedBuilding;
        if (currentBuilding != null && _craftItemInfo.HasValue)
        {
            currentBuilding.CraftingItems.Remove(_craftItemInfo.Value);
        }

        CraftingItemImage.gameObject.SetActive(false);
        _checkImage.gameObject.SetActive(false);

        //아이템 인벤토리에 추가
        GameManager.Instance.PlayerInventory.AddItem(_craftItemInfo.Value.ResultItem.Key,
            _craftItemInfo.Value.ResultCount);
        Debug.Log($"Item {_craftItemInfo.Value.ResultItem.Name} added to inventory.");

        //플레이어 데이터로 저장
        GameManager.Instance.SavePlayerData();
    }

    public void ClearCraftingItem() //생산 중인 아이템 지우기
    {
        _craftItemInfo = null;
        CraftingItemImage.gameObject.SetActive(false);
        _checkImage.gameObject.SetActive(false);
        _timeProgressBar.gameObject.SetActive(false);
    }
    
    public void SetCraftingItem(CraftItemInfo craftItemInfo) //생산 중인 아이템의 데이터 세팅
    {
        _buildingKey = craftItemInfo.BuildingKey;
        _craftItemInfo = craftItemInfo;
        CraftingItemImage.sprite = _craftItemInfo.Value.ResultItem.Sprite;

        //생산 시간 계산 및 세팅
        float remainingTime = TimeManager.Instance.GetRemainingTime(_buildingKey, craftItemInfo.ResultItem.Key);
        _timeProgressBar.maxValue = _craftItemInfo.Value.RequiredTime;
        _timeProgressBar.value = _craftItemInfo.Value.RequiredTime - remainingTime;
        _timeText.text = TimeManager.ConvertTime((int)remainingTime);

        if (remainingTime == 0) //생산 완료 ?
        {
            _isCraftingComplete = true;
            _timeProgressBar.gameObject.SetActive(false);
            _fastBtn.gameObject.SetActive(false);
            _checkImage.gameObject.SetActive(true);
        }
        else //아직 생산 중 ?
        {
            _isCraftingComplete = false;
            _timeProgressBar.gameObject.SetActive(true);
            _fastBtn.gameObject.SetActive(true);
            _checkImage.gameObject.SetActive(false);
        }

        CraftingItemImage.gameObject.SetActive(true);
    }
}
