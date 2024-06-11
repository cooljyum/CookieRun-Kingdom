using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public List<int> DeckKeyLists;
    public List<List<int>> PosIndexLists;
}
