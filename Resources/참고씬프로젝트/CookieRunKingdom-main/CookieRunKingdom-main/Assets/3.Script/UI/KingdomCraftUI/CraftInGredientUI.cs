using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftInGredientUI : MonoBehaviour
{
    [SerializeField] private Image ingredientImage;
    [SerializeField] private TextMeshProUGUI ingredientAmount;

    private ItemBundle _ingredientItem;

    public void Init(ItemBundle ingredientItem)
    {
        _ingredientItem = ingredientItem;

        int count = DataBaseManager.Instance.MyDataBase.itemDataBase[_ingredientItem.ingredientItem];
        ingredientImage.sprite = _ingredientItem.ingredientItem.ItemImage;
        ingredientAmount.text = count + "/" + _ingredientItem.count.ToString();
    }

    public void SetText()
    {
        int count = DataBaseManager.Instance.MyDataBase.itemDataBase[_ingredientItem.ingredientItem];
        ingredientAmount.text = count + "/" + _ingredientItem.count.ToString();
    }
}
