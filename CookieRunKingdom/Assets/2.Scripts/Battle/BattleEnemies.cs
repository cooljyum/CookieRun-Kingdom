using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleEnemies : BattleEntitiesBase
{
    private List<List<List<int>>> _battleEnemiesKeys = new List<List<List<int>>>();
    public List<List<List<int>>> BattleEnemiesKeysList => _battleEnemiesKeys;

    protected override CharacterData GetCharacterData(int key)
    {
        return DataManager.Instance.GetMonsterData(key);
    }

    public void CreateBattleEnemiesGroup(int stage)
    {
        StageData stageData = DataManager.Instance.GetStageData(stage);
        Vector2 basePosition = new Vector2(0, 0);
        Vector2 offset = new Vector2(20, 10);

        for (int i = 0; i < stageData.MonsterGroupList.Count; i++)
        {
            MonsterGroupData monsterGroup = DataManager.Instance.GetMonsterGroupData(stageData.MonsterGroupList[i]);
            List<List<int>> monsterGroupList = new List<List<int>>();
            monsterGroupList.Add(new List<int>(monsterGroup.FrontMonsterKeyList));
            monsterGroupList.Add(new List<int>(monsterGroup.MiddleMonsterKeyList));
            monsterGroupList.Add(new List<int>(monsterGroup.BackMonsterKeyList));
            _battleEnemiesKeys.Add(monsterGroupList);

            Vector2 teamPosition = basePosition + (offset * i);
            CreateBattleEnemies(monsterGroupList, teamPosition);
        }
    }

    public void CreateBattleEnemies(List<List<int>> enemyKeys, Vector2 position)
    {
        CreateBattleEntities(enemyKeys, "BattleEnemies", "Prefabs/Battle/BattleEnemy", true, position, true);
    }
}
