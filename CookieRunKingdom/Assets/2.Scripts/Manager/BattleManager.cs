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
            return null;
        }
        _cntCurBattleEnemies = _enemiesTeamList[_curEnemyTeamIdx].Count;
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
            {
                UpdateEnemyTeams();
                SetTargetsToEnemies();
            }
        }
        else 
        {
            _killedCookie++;
        }
    }


    void UpdateEnemyTeams()
    {
        // 현재 팀의 모든 게임 오브젝트 비활성화
        foreach (var subList in _enemiesTeamList[_curEnemyTeamIdx])
        {
            foreach (var enemy in subList)
            {
                enemy.SetActive(false);
            }
        }

        // 나머지 팀의 게임 오브젝트들을 부모의 부모 위치로 이동
        for (int teamIdx = 0; teamIdx < _enemiesTeamList.Count; teamIdx++)
        {
            if (teamIdx == _curEnemyTeamIdx) continue;

            List<List<GameObject>> team = _enemiesTeamList[teamIdx];
            for (int subListIdx = 0; subListIdx < team.Count; subListIdx++)
            {
                List<GameObject> subList = team[subListIdx];
                for (int objIdx = 0; objIdx < subList.Count; objIdx++)
                {
                    if (teamIdx - 1 >= 0)
                    {
                        // 부모의 부모의 위치로 이동
                        Transform parentTransform = subList[objIdx].transform.parent;
                        if (parentTransform != null)
                        {
                            Transform grandParentTransform = parentTransform.parent;
                            if (grandParentTransform != null)
                            {
                                subList[objIdx].transform.position = grandParentTransform.position;
                            }
                        }
                    }
                }
            }
        }

        // _curEnemyTeamIdx 증가
        _curEnemyTeamIdx++;
        // 필요에 따라 _curEnemyTeamIdx가 _enemiesTeamList의 범위를 넘지 않도록 처리
        if (_curEnemyTeamIdx >= _enemiesTeamList.Count)
        {
            _curEnemyTeamIdx = 0; // 예를 들어 순환하려면 0으로 설정
        }
    }
}
