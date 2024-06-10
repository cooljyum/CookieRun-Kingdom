using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    private List<List<GameObject>> _battleCookies;
    private List<List<List<GameObject>>> _enemiesTeamList;

    [SerializeField]
    private int _enemiesTeamCount = 0;

    private void Start()
    {
        BattleObjectSpawnManager.Instance.Init();
        _battleCookies = BattleObjectSpawnManager.Instance.GetBattleCookies();
        _enemiesTeamList = BattleObjectSpawnManager.Instance.GetEnemiesObjList();

        SetTargetsToCookies();
        SetTargetsToEnemies();
    }

    private void SetTargetsToCookies()
    {
        SetTargets(_battleCookies, GetEnemyObjectLists());
    }

    private void SetTargetsToEnemies()
    {
        if (_enemiesTeamList.Count == 0)
            return;

        SetTargets(GetEnemyObjectLists(), _battleCookies);
    }

    private void SetTargets(List<List<GameObject>> objLists, List<List<GameObject>> targetList)
    {
        foreach (var objList in objLists)
        {
            foreach (var obj in objList)
            {
                BattleObject battleObj = obj.GetComponent<BattleObject>();
                GameObject closestObj = FindClosestObject(battleObj.transform, targetList);
                if (closestObj != null)
                {
                    battleObj.SetTarget(closestObj.transform);
                }
            }
        }
    }

    private List<List<GameObject>> GetEnemyObjectLists()
    {
        return _enemiesTeamList[_enemiesTeamCount];
    }
    private GameObject FindClosestObject(Transform originTransform, List<List<GameObject>> objectLists)
    {
        GameObject closestObject = null;
        float minDistance = float.MaxValue;

        foreach (var objectList in objectLists)
        {
            foreach (var obj in objectList)
            {
                float distance = Vector3.Distance(originTransform.position, obj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestObject = obj;
                }
            }
        }

        return closestObject;
    }
}
