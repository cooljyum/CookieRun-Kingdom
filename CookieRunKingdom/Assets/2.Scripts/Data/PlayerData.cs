using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Level;
    public float Exp;
    public int Coin;
    public int Diamond;
    public int Mileage;
    public int CurStage;
    public List<int> DeckKeyLists;
    public List<int> BattlePosCntLists; // ����,�߹�,�Ĺ濡 �� ��� ������ �Ǿ� �ִ���
    public List<int> BuildingKeyLists;
    public List<Vector2> BuildingPosLists;
    public List<List<int>> PosIndexLists;
    public List<int> MyCardsLists;
    public int InventoryLevel;
    public List<int> InvenItemKeyLists;
    public List<int> InvenItemAmountLists;
}
