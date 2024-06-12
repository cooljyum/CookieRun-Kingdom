using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Material & Equipment")]
    public int Key;
    public string Name;
    public int Amount;

    [Header("Equipment : Required Information")]
    public string RequiredMaterial1Name;
    public int RequiredMaterial1Amount;
    public string RequiredMaterial2Name;
    public int RequiredMaterial2Amount;
}
