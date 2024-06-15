using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public List<int> DeckKeyLists;
    public List<int> BuildingKeyLists;
    public List<List<int>> PosIndexLists;
    public List<int> MyCardsLists;

    public Dictionary<int, int> InventoryItems;
    public PlayerData()
    {
        InventoryItems = new Dictionary<int, int>();
    }
}
