using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataBase
{
    // 아이템 데이터베이스
    public Dictionary<ItemData, int> itemDataBase = new Dictionary<ItemData, int>();

    public DataBase(Dictionary<ItemData, int> itemDataBase)
    {
        this.itemDataBase = itemDataBase;
    }
}
