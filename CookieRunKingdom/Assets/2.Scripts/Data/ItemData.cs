using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite GetItemSprite(int key)
    {
        if (key == Key)
            return Sprite;
        else 
            return null;
    }

    [Header("Material & Equipment")]
    public int Key;
    public string Name;
    public Sprite Sprite;

    [Header("Equipment : Required Information")]
    public List<int> MaterialKeys;
    public List<int> MateiralAmounts;
}
