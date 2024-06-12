using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct CraftItemInfo //아이템 생산 정보
{
    public RenderTexture RenderTexture;
    public int RequiredTime;
    public bool IsMaterial;
    public List<Sprite> ItemImages;
    public List<int> ItemCounts;
    public int Cost;
}

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

    [Header("Build And Level Up")] //건설 및 레벨 업 조건
    public int RequiredGold;
    public string RequiredMaterialName;
    public int RequiredMaterialCount;
    public string RequiredEquipmentName;
    public int RequiredEquipmentCount;
    public float RequiredTime;

    [Header("Productive Building")] //생산적 건물
    public List<CraftItemInfo> CraftInfos;
}
