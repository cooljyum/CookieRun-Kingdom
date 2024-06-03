using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    money,
    dia,
    expendables,
    product,
    Empty
}

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [SerializeField] private EItemType itemType;
    [SerializeField] private Sprite itemImage;
    [SerializeField] private string itemName;
    [Multiline(5)]
    [SerializeField] private string itemExplain;

    [SerializeField] private float itemValue;

    public EItemType ItemType => itemType;
    public Sprite ItemImage => itemImage;
    public string ItemName => itemName;
    public string ItemExplain => itemExplain;
    public float ItemValue => itemValue;
    public string ItemTypeString
    {
        get
        {
            switch (itemType)
            {
                case EItemType.expendables:
                    return "소모품";
                case EItemType.product:
                    return "생산품";
            }
            return "나도 몰라";
        }
    }
}
