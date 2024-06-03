using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HousingItemData : ScriptableObject
{
    [SerializeField] private bool isTile = false;
    [SerializeField] private string housingItemName;
    [SerializeField] private Sprite housingItemImage;

    public bool IsTile => isTile;
    public string HousingItemName => housingItemName;
    public Sprite HousingItemImage => housingItemImage;
}
