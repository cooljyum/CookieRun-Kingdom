using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<List<GameObject>> _battleCookies;
    private List<List<List<GameObject>>> _enemiesTeamList;

    [SerializeField]
    private int _currentEnemyTeamIndex = 0;

    public static BattleManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BattleObjectSpawnManager.Instance.Init();
        _battleCookies = BattleObjectSpawnManager.Instance.GetBattleCookies();
        _enemiesTeamList = BattleObjectSpawnManager.Instance.GetEnemiesObjList();

        SetTargetsToCookies();
        SetTargetsToEnemies();
    }

    public void SetTargetsToCookies()
    {
        SetTargets(_battleCookies, GetCurrentEnemyObjectLists());
    }

    public void SetTargetsToEnemies()
    {
        if (_enemiesTeamList.Count == 0)
        {
            Debug.LogWarning("No enemies team list found!");
            return;
        }

        SetTargets(GetCurrentEnemyObjectLists(), _battleCookies);
    }

    private void SetTargets(List<List<GameObject>> objectLists, List<List<GameObject>> targetLists)
    {
        foreach (var objectList in objectLists)
        {
            foreach (var obj in objectList)
            {
                var battleObj = obj.GetComponent<BattleObject>();
                var closestObj = FindClosestObject(battleObj.transform.parent, targetLists);
                if (closestObj != null)
                {
                    battleObj.SetTarget(closestObj);
                }
            }
        }
    }

    public void SetTargetObj(GameObject gameObj, bool isEnemy = false)
    {
        var targetLists = isEnemy ? _battleCookies : GetCurrentEnemyObjectLists();
        var closestObj = FindClosestObject(gameObj.transform.parent, targetLists);
        if (closestObj != null)
        {
            gameObj.GetComponent<BattleObject>().SetTarget(closestObj);
        }
    }

    private List<List<GameObject>> GetCurrentEnemyObjectLists()
    {
        if (_currentEnemyTeamIndex >= _enemiesTeamList.Count)
        {
            Debug.LogError("Invalid enemy team index!");
            return new List<List<GameObject>>();
        }

        return _enemiesTeamList[_currentEnemyTeamIndex];
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
