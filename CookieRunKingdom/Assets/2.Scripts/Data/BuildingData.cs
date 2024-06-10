using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Object/BuildingData")]
public class BuildingData : ScriptableObject
{
    public int Key;
    public string Name;
    public string Type;
    public int Level;
    public int Size;
    public int Point;
    public SkeletonDataAsset SkeletonDataAsset;

    [Header("Build And Level Up")]
    public int RequiredGold;
    public string RequiredMaterialName;
    public int RequiredMaterialCount;
    public string RequiredEquipmentName;
    public int RequiredEquipmentCount;
    public float RequiredTime;

    [Header("Productive")]
    public string ProductName;
    public int ProductCount;
    public float ProductRequiredTime;
}
