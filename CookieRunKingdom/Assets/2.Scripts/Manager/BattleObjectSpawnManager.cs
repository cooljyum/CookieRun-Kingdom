using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleObjectSpawnManager : Singleton<BattleObjectSpawnManager>
{
    //BattkeCookie
    [Header("BattleCookie")]
    [Tooltip("Settings related to battle cookies.")]
    private BattleCookies _cookiesManager;
    [SerializeField]
    private TeamData _cookieTeam = new TeamData();
    private List<List<int>> _battleCookieKeys = new List<List<int>>();
    public List<List<int>> BattleCookieKeys
    {
        get { return _battleCookieKeys; }
    }
    private List<List<GameObject>> _battleCookies = new List<List<GameObject>>();
    public List<List<GameObject>> GetBattleCookies()
    {
        return _battleCookies;
    }
    [Header("-------------------")]
    //Enemy
    [Header("Enemy")]
    [Tooltip("Settings related to battle enemies.")]
    private BattleEnemies _enemiesManager;
    [SerializeField]
    private List<TeamData> _enemiesTeamList = new List<TeamData>();
    private List<List<List<int>>> _battleEnemiesKeys = new List<List<List<int>>>();
    private List<List<List<GameObject>>> _battleEnemiesObjectsList = new List<List<List<GameObject>>>();
    public List<List<List<GameObject>>> GetEnemiesObjList()
    {
        return _battleEnemiesObjectsList;

    }
    public List<TeamData> GetEnemiesTeamList()
    {
        return _enemiesTeamList;
    }

    //ClassData
    [System.Serializable]
    public class TeamData
    {
        [SerializeField]
        public BattleObjPosData Front = new BattleObjPosData();
        [SerializeField]
        public BattleObjPosData Middle = new BattleObjPosData();
        [SerializeField]
        public BattleObjPosData Back = new BattleObjPosData();

        public void Clear()
        {
            Front.Clear();
            Middle.Clear();
            Back.Clear();
        }
    }
    [System.Serializable]
    public class BattleObjPosData
    {
        public List<int> BattleKey = new List<int>();
        public List<GameObject> BattleObj = new List<GameObject>();

        public void Clear()
        {
            BattleKey.Clear();
            BattleObj.Clear();
        }
    }

    public void Init() //초기화
    {
        SetBattleCookieKeys();
        CreateBattle();
    }

    private void CreateBattle() //Battle스폰
    {
        SetCookieData();
        SetEnemiesData();
        PopulateEnemiesTeamList();
    }

    //Data설정
    private void SetCookieData()
    {
        //Set _cookiesManager
        _cookiesManager = new BattleCookies();
        _cookiesManager.CreateBattleCookies(_battleCookieKeys);
        _battleCookies = _cookiesManager.BattleEntitiesList;

        BattleManager.Instance.CntCurCookies = _cookiesManager.CntSpawnEntities;

        // Clear
        _cookieTeam.Clear();

        for (int i = 0; i < _battleCookieKeys.Count; i++)
        {
            BattleObjPosData positionData = null;

            switch (i)
            {
                case 0:
                    positionData = _cookieTeam.Front;
                    break;
                case 1:
                    positionData = _cookieTeam.Middle;
                    break;
                case 2:
                    positionData = _cookieTeam.Back;
                    break;
                default:
                    Debug.LogWarning($"More _battleCookieKeys entries than expected. Ignoring entry at index {i}.");
                    continue;
            }

            // Setup position data
            if (positionData != null)
            {
                SetPositionData(positionData, i, _battleCookieKeys[i].Count); // startIndex는 0부터 시작
            }
        }
    }

    private void SetEnemiesData()
    {
        _enemiesManager = new BattleEnemies();
        _enemiesManager.CreateBattleEnemiesGroup(BattleManager.Instance.Stage);
        _battleEnemiesKeys = _enemiesManager.BattleEnemiesKeysList;
        _battleEnemiesObjectsList = _enemiesManager.BattleEnemiesObjectsList;
    }

    private void SetPositionData(BattleObjPosData positionData, int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            positionData.BattleKey.Add(_battleCookieKeys[index][i]);
            positionData.BattleObj.Add(_battleCookies[index][i]);
        }
    }

    private void PopulateEnemiesTeamList()
    {
        _enemiesTeamList.Clear();

        for (int i = 0; i < _battleEnemiesKeys.Count; i++)
        {
            TeamData teamData = new TeamData();

            for (int j = 0; j < _battleEnemiesKeys[i].Count; j++)
            {
                if (j == 0) // Front
                {
                    teamData.Front.BattleKey = _battleEnemiesKeys[i][j];
                    teamData.Front.BattleObj = _battleEnemiesObjectsList[i][j];
                }
                else if (j == 1) // Middle
                {
                    teamData.Middle.BattleKey = _battleEnemiesKeys[i][j];
                    teamData.Middle.BattleObj = _battleEnemiesObjectsList[i][j];
                }
                else if (j == 2) // Back
                {
                    teamData.Back.BattleKey = _battleEnemiesKeys[i][j];
                    teamData.Back.BattleObj = _battleEnemiesObjectsList[i][j];
                }
            }

            _enemiesTeamList.Add(teamData);
        }
    }

    private void SetBattleCookieKeys()
    {
        _battleCookieKeys.Clear();

        //테스트코드
        if(GameManager.Instance.CurPlayerData.BattlePosCntLists.Count <= 0)
            TestGame();

        int curIdx = 0;
        foreach (int posCount in GameManager.Instance.CurPlayerData.BattlePosCntLists)
        {
            List<int> posList = new List<int>();

            for (int i = 0; i < posCount; i++)
            {
                if (curIdx < GameManager.Instance.CurPlayerData.DeckKeyLists.Count)
                {
                    posList.Add(GameManager.Instance.CurPlayerData.DeckKeyLists[curIdx]);
                    curIdx++;
                }
                else
                {
                    Debug.LogWarning("DeckKeyLists에 값이 부족합니다.");
                    break;
                }
            }

            _battleCookieKeys.Add(posList);
        }
    }
    private void TestGame()
    {
        _battleCookieKeys.Clear();

        // 예제 데이터 설정
        _battleCookieKeys = new List<List<int>> {
                    new List<int> { 1, 2 },   // Front
                    new List<int> { 3, 4 },   // Middle
                    new List<int> { 5 }       // Back
                };
    }
}
