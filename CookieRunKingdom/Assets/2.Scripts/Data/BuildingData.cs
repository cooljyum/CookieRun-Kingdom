using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct CraftItemInfo //������ ���� ����
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

    [Header("Build And Level Up")] //�Ǽ� �� ���� �� ����
    public int RequiredGold;
    public string RequiredMaterialName;
    public int RequiredMaterialCount;
    public string RequiredEquipmentName;
    public int RequiredEquipmentCount;
    public float RequiredTime;

    [Header("Productive Building")] //������ �ǹ�
    public List<CraftItemInfo> CraftInfos;
}
