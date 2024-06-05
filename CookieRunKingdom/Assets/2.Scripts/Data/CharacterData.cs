using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Create CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public int Key;
    public string Name;
    public int Level;
    public float Hp;
    public float Attack;
    public float Defence;
    public float Critical;
    public int Type;
    public int Position;
    public int Rarity;
    public SkeletonDataAsset SkeletonDataAsset;
    public Sprite profileImage;
}
