using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<List<GameObject>> _battleCookies;
    private List<List<List<GameObject>>> _enemiesTeamList;

    [SerializeField]
    private int _curEnemyTeamIdx = 0;
    [SerializeField]
    private int _cntCurBattleEnemies = 0;
    [SerializeField]
    private int _killedEnemies = 0;
    [SerializeField]
    private int _killedCurBattleEnemies = 0;
    [SerializeField]
    private int _killedCookie = 0;

    [SerializeField]
    private bool _isOnBattle = true;
    public bool IsOnBattle 
    {
        get { return _isOnBattle; }
        set { _isOnBattle = value; }
    }

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

        _isOnBattle = true;

        StartCoroutine(StartGameInit());
    }

    private IEnumerator StartGameInit()
    {
        yield return new WaitForSeconds(3f);

        SetTargetsToCookies();
        SetTargetsToEnemies();

        BattleUIManager.Instance.SetTimer(180f);

        _isOnBattle = false;
    }


    public void SetTargetsToCookies()
    {
        SetTargets(_battleCookies, GetCurrentEnemyObjLists());
    }

    public void SetTargetsToEnemies()
    {
        SetCurBattleEnemies();
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

        return _enemiesTeamList[_curEnemyTeamIdx];
    }

    private void SetCurBattleEnemies() 
    {
        //받아올때 현재 적 수 받아오기
        for (int i = 0; i < _enemiesTeamList[_curEnemyTeamIdx].Count; i++)
        {
            _cntCurBattleEnemies += _enemiesTeamList[_curEnemyTeamIdx][i].Count;
        }
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
                BattleManager.Instance.IsOnBattle = false;
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
        _enemiesTeamList[_curEnemyTeamIdx][0][0].transform.parent.parent.parent.parent.gameObject.SetActive(false);

        List<List<List<GameObject>>> _enemiesTeamListPos = new List<List<List<GameObject>>>();
        _enemiesTeamListPos =  _enemiesTeamList;

        // 나머지 팀의 게임 오브젝트들을 부모의 부모 위치로 이동
        for (int teamIdx = _enemiesTeamListPos.Count-1; teamIdx >= 2; teamIdx--)
        {
            if (teamIdx == _curEnemyTeamIdx) continue;

            List<List<GameObject>> team = _enemiesTeamList[teamIdx];
            List<List<GameObject>> team_ = _enemiesTeamListPos[teamIdx-1];

            GameObject subList = team[0][0];
            GameObject subList_ = team_[0][0];
            Transform  pos = subList_.transform.parent.parent.parent.parent;
            subList.transform.parent.parent.parent.parent.transform.position = pos.position;
        }

        _curEnemyTeamIdx++;

        // _curEnemyTeamIdx체크 _enemiesTeamList
        if (_curEnemyTeamIdx >= _enemiesTeamList.Count)
        {
            _curEnemyTeamIdx = 0; 
        }
    }
}
