using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public int Coin;
    public int Mileage;
    public int CurStage;
    public List<int> DeckKeyLists;
    public List<int> BattlePosCntLists; // 전방,중방,후방에 각 몇명씩 포지션 되어 있는지
    public List<int> BuildingKeyLists;
    public List<Vector2> BuildingPosLists;
    public List<List<int>> PosIndexLists;
    public List<int> MyCardsLists;
    public List<int> InvenItemKeyLists;
    public List<int> InvenItemAmountLists;
}
