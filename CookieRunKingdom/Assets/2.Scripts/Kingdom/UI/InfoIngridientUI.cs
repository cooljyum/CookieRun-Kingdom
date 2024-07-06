using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoIngridientUI : MonoBehaviour  //* �ǹ� ���� UI *//
{
    [SerializeField]
    private Image _ingredientImage; //�Ǽ� ��� �̹���
    [SerializeField]
    private TextMeshProUGUI _ingridientAmount; //�Ǽ� ��� �ʿ� ����
    [SerializeField]
    private TextMeshProUGUI _requiredTime; //�Ǽ� �ʿ� �ð�

    public void SetIngredientData(ItemData itemData, int amount) //�ǹ� ���� UI Ȱ��ȭ -> �Ǽ� ��� ������ ����
    {
        _ingredientImage.sprite = itemData.Sprite;

        int curCount = GameManager.Instance.PlayerInventory.GetItemAmount(itemData.Key);
        _ingridientAmount.text = $"{curCount}/{amount}";
    }

    public void SetTimeData(int time) //�ǹ� ���� UI Ȱ��ȭ -> �Ǽ� �ð� ������ ����
    {
        _requiredTime.text = TimeManager.ConvertTime(time);
    }
}
