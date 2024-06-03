using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] private BattleMapGenerator _mapGenerator;
    [SerializeField] private MapScrolling _mapScrolling;
    [SerializeField] private BattleUI _battleUI;

    private List<CookieController> _battleCookies;

    protected override void Init()
    {
        // 스테이지를 읽어와
        StageData stageData = GameManager.Game.StageData;

        // 맵 만들기도 여기서 만들자
        _mapGenerator.GenerateBattleMap(stageData);
        _mapScrolling.Init();

        // 쿠키 만들자
        CreateCookie();

        // 쿠키 순서 조정


        // 배틀 시작
        StartBattle(stageData);

        // 로딩 UI 풀어주자!
        GameManager.Scene.EndLoading();
        GameManager.Sound.PlayBgm(EBGM.battle);
    }

    private void CreateCookie()
    {
        CookieController[] allCookiesPrefab = DataBaseManager.Instance.AllCookies;
        List<CookieInfo> allCookieInfo = GameManager.Game.allCookies;

        _battleCookies = new List<CookieController>();

        for(int i = 0; i < allCookieInfo.Count; i++)
        {
            if(allCookieInfo[i].isBattleMember)
            {
                CookieInfo cookieInfo = allCookieInfo[i];
                CookieController cookie = Instantiate(allCookiesPrefab[cookieInfo.cookieIndex]);

                // 정보넣기
                cookie.CookieStat.LoadCookie();
                _battleCookies.Add(cookie);
            }
        }

        _battleCookies.Sort(CustomComparison);
    }
    private int CustomComparison(CookieController x, CookieController y)
    {
        // 쿠키의 자리 순서대로 정렬
        int result = (x.CookieStat.BattlePosition).CompareTo(y.CookieStat.BattlePosition);

        // 같으면 index 순
        if (result == 0)
            result = ((CookieData)x.Data).CookieIndex.CompareTo(((CookieData)x.Data).CookieIndex);

        return result;
    }

    private void StartBattle(StageData stageData)
    {
        // battleManager 초기화 작업
        BattleManager.instance.SetStage(_battleCookies, stageData);
        BattleManager.instance.Init();

        // UI 작업
        GameManager.UI.PushUI(_battleUI);
        _battleUI.StartBattle(stageData, BattleManager.instance.StartBattle);
    }
}
