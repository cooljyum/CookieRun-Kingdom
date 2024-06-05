using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Object/DecoBuildingData")]
public class DecoBuildingData : BuildingData
{
    public int Key;
    public string Name;
    public string Type;
    public int Level;
    public string Size;
    public int Point;
    public int RequiredGold;
    public float RequiredTime;
}