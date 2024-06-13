using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct CraftItemInfo //������ ���� ����
{
    public SkeletonDataAsset SkeletonDataAsset;
    public int RequiredTime;
    public bool IsMaterial;
    public int Cost;
    public ItemData ResultItem;
    public int ResultCount;
}

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Object/BuildingData")]
public class BuildingData : ScriptableObject
{
    public int Key;
    public string Name;
    public string Type;
    //public int Level;
    public int Size;
    public int Point;
    public SkeletonDataAsset SkeletonDataAsset;

    [Header("Build And Level Up")] //�Ǽ� �� ���� �� ����
    public int RequiredGold;
    public ItemData RequiredMaterial;
    public int RequiredMaterialCount;
    public ItemData RequiredEquipment;
    public int RequiredEquipmentCount;
    public float RequiredTime;

    [Header("Productive Building")] //���꼺 �ǹ�
    public List<CraftItemInfo> CraftInfos;
}
