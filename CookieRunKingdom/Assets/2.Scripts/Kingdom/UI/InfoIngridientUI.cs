using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoIngridientUI : MonoBehaviour  //* 건물 정보 UI *//
{
    [SerializeField]
    private Image _ingredientImage; //건설 재료 이미지
    [SerializeField]
    private TextMeshProUGUI _ingridientAmount; //건설 재료 필요 개수
    [SerializeField]
    private TextMeshProUGUI _requiredTime; //건설 필요 시간

    public void SetIngredientData(ItemData itemData, int amount) //건물 정보 UI 활성화 -> 건설 재료 데이터 세팅
    {
        _ingredientImage.sprite = itemData.Sprite;

        int curCount = GameManager.Instance.PlayerInventory.GetItemAmount(itemData.Key);
        _ingridientAmount.text = $"{curCount}/{amount}";
    }

    public void SetTimeData(int time) //건물 정보 UI 활성화 -> 건설 시간 데이터 세팅
    {
        _requiredTime.text = TimeManager.ConvertTime(time);
    }
}
