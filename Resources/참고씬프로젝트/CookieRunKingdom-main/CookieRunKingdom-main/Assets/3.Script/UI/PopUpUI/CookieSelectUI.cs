using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CookieSelectUI : BaseUI
{
    public System.Action OnChangeBattleCookie = null;

    [SerializeField] private CookieReadyAdventureUI _cookieReadyAdventureUI;


    /// <summary>
    /// 8 5 2
    /// 9 6 3
    /// 7 4 1
    /// </summary>
    /// 

    [SerializeField] private SkeletonGraphic[] _cookiePositions;

    private CookieController[] isPosition;
    private int[,] priority = new int[,] { { 0, 1, 2 }, { 1, 2, 0 }, { 2, 1, 0 } };

    private KingdomManager _manager;
    private StageData _stageData;

    // public List<CookieController> SelectedTempCookies { get; private set; }
    public List<CookieController> SelectedTempCookies;


    public override void Init()
    {
        base.Init();

        _manager = FindObjectOfType<KingdomManager>();
    }


    public void InitStageData(StageData stageData, Vector3 prevCameraPos, float prevOrthoSize)
    {
        _stageData = stageData;
        GameManager.Game.StageData = _stageData;
    }

    public override void Hide()
    {
        GameManager.Sound.PlayBgm(EBGM.lobby);

        _manager.IsMoveCamera = true;
        for (int i = 0; i < _cookiePositions.Length; i++)
            _cookiePositions[i].enabled = false;
        base.Hide();
    }

    public override void Show()
    {
        base.Show();

        GameManager.Sound.PlayBgm(EBGM.readyBattle);

        SelectedTempCookies = new List<CookieController>();
        _manager.IsMoveCamera = false;

        List<CookieController> allCookies = _manager.allCookies;

        for (int i = 0; i < allCookies.Count; i++)
            if (allCookies[i].CookieStat.IsBattleMember)
                AddCookie(allCookies[i]);

        // BattleCookies 다 설정하면 CookieReadyAdventure UI 켜주기
        GameManager.UI.ShowPopUpUI(_cookieReadyAdventureUI);
    }

    #region 쿠키 배치
    public void AddCookie(CookieController cookie)
    {
        if (SelectedTempCookies.Count >= 5)
            return;

        SelectedTempCookies.Add(cookie);
        SelectedTempCookies.Sort(CustomComparison);
        cookie.CookieStat.SetBattle(true, -1);

        ReArrange();
        OnChangeBattleCookie?.Invoke();
    }


    public void RemoveCookie(CookieController cookie)
    {
        if (!cookie.CookieStat.IsBattleMember)
            return;
        SelectedTempCookies.Remove(cookie);
        _cookiePositions[cookie.CookieStat.BattlePosition].enabled = false;
        SelectedTempCookies.Sort(CustomComparison);
        ReArrange();
        cookie.CookieStat.SetBattle(false, -1);
        OnChangeBattleCookie?.Invoke();
    }

    private int CustomComparison(CookieController x, CookieController y)
    {
        // 후방, 중앙, 전방
        int result = ((CookieData)x.Data).CookiePosition.CompareTo(((CookieData)y.Data).CookiePosition);

        // 같으면 index 순
        if (result == 0)
            result = ((CookieData)x.Data).CookieIndex.CompareTo(((CookieData)x.Data).CookieIndex);

        return result;
    }

    private void ReArrange()
    {
        isPosition = new CookieController[_cookiePositions.Length];

        for (int i = 0; i < SelectedTempCookies.Count; i++)
        {
            int cookiePosition = (int)((CookieData)SelectedTempCookies[i].Data).CookiePosition;
            bool isArrange = false;

            for (int j = 0; j < priority.GetLength(1); j++)
            {
                int startIndex = priority[cookiePosition, j] * priority.GetLength(1);
                int endIndex = startIndex + priority.GetLength(1) - 1;

                for (int h = startIndex; h < endIndex; h++)
                {
                    if (isPosition[h] == null)
                    {
                        isPosition[h] = SelectedTempCookies[i];
                        isArrange = true;
                        break;
                    }
                }
                if (isArrange)
                    break;
            }
        }


        for (int i = 0; i < priority.GetLength(0); i++)
        {
            int count = 0;
            int index = 0;

            int startIndex = i * priority.GetLength(1);
            int endIndex = startIndex + priority.GetLength(1) - 1;

            for (int j = startIndex; j < endIndex; j++)
            {
                if (isPosition[j])
                {
                    index = j;
                    count++;
                }
            }

            if (count == 1)
            {
                isPosition[endIndex] = isPosition[index];
                isPosition[index] = null;
            }
        }

        // 정렬이 되고 여기서 설정해줌
        for (int i = 0; i < isPosition.Length; i++)
        {
            if (isPosition[i] != null)
            {
                _cookiePositions[i].enabled = true;
                _cookiePositions[i].skeletonDataAsset = isPosition[i].Data.SkeletonDataAsset;
                _cookiePositions[i].Initialize(true);
                _cookiePositions[i].AnimationState.SetAnimation(0, "battle_idle", true);

                isPosition[i].CookieStat.SetBattle(true, i);
            }
            else
            {
                _cookiePositions[i].enabled = false;
            }
        }
    }
    #endregion


    #region 버튼에 이벤트로 넣어줄 메소드
    public void OnClickBattleStartButton()
    {
        // 고기 젤리 확인하고
        if(GameManager.Game.Jelly >= _stageData.Jelly)
        {
            GameManager.Game.Jelly -= _stageData.Jelly;
        }
        else
        {
            Debug.Log("젤리가 부족합니다.");
            return;
        }

        // 출전할 쿠키가 하나도 없으면 무시
        if(SelectedTempCookies.Count == 0)
            return;

        // 씬전환시 데이터 저장
        _manager.allCookies.ForEach(cookie => cookie.CookieStat.SaveCookie());

        GameManager.Scene.LoadScene(ESceneName.Battle);
    }
    #endregion
}
