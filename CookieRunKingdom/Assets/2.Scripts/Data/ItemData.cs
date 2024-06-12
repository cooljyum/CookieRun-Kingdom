using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Material & Equipment")]
    public int Key;
    public string Name;
    public Sprite Image;

    [Header("Equipment : Required Information")]
    public List<int> MateiralKeys;
    public List<int> MateiralAmounts;
}
