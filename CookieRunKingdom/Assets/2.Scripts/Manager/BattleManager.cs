using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    //BattkeCookie
    [Header("BattleCookie")]
    [Tooltip("Settings related to battle cookies.")]

    #if UNITY_EDITOR
    [SerializeField]
    private TeamData _cookieTeam = new TeamData();

#endif
    private BattleCookies _cookiesManager;
    private List<List<int>> _battleCookieKeys = new List<List<int>>();
    private List<List<GameObject>> _battleCookies = new List<List<GameObject>>();

    [Header("-------------------")]
    //Enemy
    [Header("Enemy")]
    [Tooltip("Settings related to battle enemies.")]

    [SerializeField]
    private int _stage = 1;
#if UNITY_EDITOR
    [SerializeField]
    private List<TeamData> _enemiesTeamList = new List<TeamData>();

#endif
    private BattleEnemies _enemiesManager;
    private List<List<List<int>>> _battleEnemiesKeys  = new List<List<List<int>>>();
  

    [System.Serializable]
    public class TeamData
    {
        [SerializeField]
        public PositionData Front = new PositionData();
        [SerializeField]
        public PositionData Middle = new PositionData();
        [SerializeField]
        public PositionData Back = new PositionData();

        public void Clear()
        {
            Front.Clear();
            Middle.Clear();
            Back.Clear();
        }
    }

    [System.Serializable]
    public class PositionData
    {
        public List<int> CookieKey = new List<int>();
        public List<GameObject> CookieObj = new List<GameObject>();

        public void Clear()
        {
            CookieKey.Clear();
            CookieObj.Clear();
        }
    }

    private void Start()
    {
        Init(); // �ʱ�ȭ �Լ� ȣ��
    }

    private void Init()
    {
        TestGame();
        CreateBattle(); // ��Ʋ ������Ʈ ���� �Լ� ȣ��
    }

    private void CreateBattle() 
    {
        SetCookieData();

        _enemiesManager = new BattleEnemies();
        _enemiesManager.CreateBattleEnemiesGroup(_stage);
        _battleEnemiesKeys = _enemiesManager.BattleEnemiesKeysList;
        PopulateEnemiesTeamList();
    }


    //Data����
    private void SetCookieData()
    {
        //Set _cookiesManager
        _cookiesManager = new BattleCookies();
        _cookiesManager.CreateBattleCookies(_battleCookieKeys);
        _battleCookies = _cookiesManager.BattleCookieList;

        // Clear
        _cookieTeam.Clear();

        for (int i = 0; i < _battleCookieKeys.Count; i++)
        {
            PositionData positionData = null;

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
                SetPositionData(positionData, i, _battleCookieKeys[i].Count); // startIndex�� 0���� ����
            }
        }
    }
    private void SetPositionData(PositionData positionData, int index , int count)
    {
        for (int i = 0; i < count; i++)
        {
            positionData.CookieKey.Add(_battleCookieKeys[index][i]);
            positionData.CookieObj.Add(_battleCookies[index][i]);
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
                    teamData.Front.CookieKey = _battleEnemiesKeys[i][j];
                }
                else if (j == 1) // Middle
                {
                    teamData.Middle.CookieKey = _battleEnemiesKeys[i][j];
                }
                else if (j == 2) // Back
                {
                    teamData.Back.CookieKey = _battleEnemiesKeys[i][j];
                }
            }

            _enemiesTeamList.Add(teamData);
        }
    }

    private void TestGame() 
    {
        _battleCookieKeys.Clear();
        // ���� ������ ����
        _battleCookieKeys = new List<List<int>> {
            new List<int> { 1, 2 },   // Front
            new List<int> { 3, 4 },   // Middle
            new List<int> { 5 }       // Back
        };
    }
}
