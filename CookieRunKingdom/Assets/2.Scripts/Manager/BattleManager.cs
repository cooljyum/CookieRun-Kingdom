using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<List<GameObject>> _battleCookies;
    private List<List<List<GameObject>>> _enemiesTeamList;

    [SerializeField]
    private int _curEnemyTeamIdx = 0;

    [SerializeField]
    private int _killedEnemies = 0;
    [SerializeField]
    private int _cntCurBattleEnemies = 0;
    [SerializeField]
    private int _killedCurBattleEnemies = 0;
    [SerializeField]
    private int _killedCookie = 0;

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
        SetTargets(_battleCookies, GetCurrentEnemyObjLists());
    }

    public void SetTargetsToEnemies()
    {
        SetTargets(GetCurrentEnemyObjLists(), _battleCookies);
    }

    private void SetTargets(List<List<GameObject>> objectLists, List<List<GameObject>> targetLists)
    {
        foreach (var objectList in objectLists)
        {
            foreach (var obj in objectList)
            {
                var battleObj = obj.GetComponent<BattleObject>();
                var closestObj = FindClosestObj(battleObj.transform.parent, targetLists);
                if (closestObj != null)
                {
                    battleObj.SetTarget(closestObj);
                }
            }
        }
    }

    public void SetTargetObj(GameObject gameObj, bool isEnemy = false)
    {
        var targetLists = isEnemy ? _battleCookies : GetCurrentEnemyObjLists();
        var closestObj = FindClosestObj(gameObj.transform.parent, targetLists);
        gameObj.GetComponent<BattleObject>().SetTarget(closestObj);
    }

    private List<List<GameObject>> GetCurrentEnemyObjLists()
    {
        if (_curEnemyTeamIdx >= _enemiesTeamList.Count)
        {
            Debug.LogError("Invalid enemy team index!");
            return new List<List<GameObject>>();
        }

        return _enemiesTeamList[_curEnemyTeamIdx];
    }

    private GameObject FindClosestObj(Transform originTransform, List<List<GameObject>> objectLists)
    {
        GameObject closestObj = null;
        float minDistance = float.MaxValue;

        foreach (var objectList in objectLists)
        {
            foreach (var obj in objectList)
            {
                float distance = Vector3.Distance(originTransform.position, obj.transform.position);
                if (distance < minDistance && obj.activeSelf == true)
                {
                    minDistance = distance;
                    closestObj = obj;
                }
            }
        }

        return closestObj;
    }

    //업데이트 죽은 배틀 정보 관리
    public void UpdateKillBattleInfo(bool isEnemy)
    {
        if (isEnemy)
        {
            _killedEnemies++;
            _killedCurBattleEnemies++;
            if (_cntCurBattleEnemies == _killedCurBattleEnemies)
                _curEnemyTeamIdx++;
        }
        else 
        {
            _killedCookie++;
        }
    }
}
