using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemBundle
{
    public ItemData ingredientItem;
    public int count;
}

[CreateAssetMenu]
public class CraftData : ScriptableObject
{
    [SerializeField] private Sprite craftImage;
    [SerializeField] private string craftName;

    [SerializeField] private bool isResource;

    [SerializeField] private ItemBundle craftResult;
    [SerializeField] private ItemBundle[] ingredients;
    [SerializeField] private float craftTime;
    [SerializeField] private float craftCost;

    public Sprite CraftImage => craftImage;
    public string CraftName => craftName;
    public bool IsResource => isResource;
    public ItemBundle CraftResult => craftResult;
    public ItemBundle[] Ingredients => ingredients;
    public int CraftTime => (int)craftTime;
    public int CraftCost => (int)craftCost;
}
