using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Object/ProductiveBuildingData")]
public class ProductiveBuildingData : BuildingData
{
    public int Key;
    public string Name;
    public string Type;
    public int Level;
    public string Size;
    public int Point;
    public int RequiredGold;
    public string RequiredMaterialName;
    public int RequiredMaterialCount;
    public string RequiredEquipmentName;
    public int RequiredEquipmentCount;
    public float RequiredTime;
    public string ProductName;
    public int ProductCount;
    public float ProductRequiredTime;
    public SkeletonDataAsset SkeletonDataAsset;
}