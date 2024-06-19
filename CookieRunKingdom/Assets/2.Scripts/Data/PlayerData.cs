using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public int Coin;
    public int CurStage;
    public List<int> DeckKeyLists;
    public List<int> BuildingKeyLists;
    public List<Vector2> BuildingPosLists;
    public List<List<int>> PosIndexLists;
    public List<int> MyCardsLists;
    public Dictionary<int, int> InventoryItems;
    public PlayerData()
    {
        InventoryItems = new Dictionary<int, int>();
        BuildingKeyLists = new List<int>();
        BuildingPosLists = new List<Vector2>();
    }
}
