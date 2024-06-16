using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoIngridientUI : MonoBehaviour
{
    [SerializeField]
    private Image _ingredientImage;
    [SerializeField]
    private TextMeshProUGUI _ingridientAmount;
    [SerializeField]
    private TextMeshProUGUI _requiredTime;

    private BuildingData _buildingData;

    public void SetData(ItemData data, int amount)
    {
        _ingredientImage.sprite = data.Sprite;

        int curCount = GameManager.Instance.PlayerInventory.GetItemCount(data.Key);
        _ingridientAmount.text = $"{curCount}/{amount}";
    }

    public void SetTimeData(int time)
    {
        _requiredTime.text = TimeManager.ConvertTime(time);
    }
}
