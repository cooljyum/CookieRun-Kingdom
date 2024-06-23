using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCookies : BattleEntitiesBase
{
    protected override CharacterData GetCharacterData(int key)
    {
        return DataManager.Instance.GetCharacterData(key);
    }

    public void CreateBattleCookies(List<List<int>> cookieKeys)
    {
        CreateBattleEntities(cookieKeys, "BattleCookies", "Prefabs/Battle/BattleCookie");
    }
}

