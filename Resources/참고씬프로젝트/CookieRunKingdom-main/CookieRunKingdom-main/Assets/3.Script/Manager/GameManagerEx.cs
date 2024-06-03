using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class GameManagerEx
{
    #region 유저정보
    private int _kingdomIndex;
    public int KingdomIndex
    {
        get { return _kingdomIndex; }
        set { _kingdomIndex = value; }
    }

    private string _kingdomName;
    public string KingdomName
    {
        get { return _kingdomName; }
        set { _kingdomName = value; }
    }
    private bool _isFirst;
    public bool IsFirst
    {
        get { return _isFirst; }
        set { _isFirst = value; }
    }

    public int EnvironmentScore { get; set; }
    #endregion

    #region 재화
    public DateTime prevJellyTime = System.DateTime.Now;
    public int jellyTime = Utils.JellyTime;
    public Action OnChangeDia, OnChangeMoney, OnChangeJelly;
    private int _dia, _money, _jelly, _maxJelly;

    public int Dia
    {
        get { return _dia; }
        set { _dia = value; OnChangeDia?.Invoke(); }
    }

    public int Money
    {
        get { return _money; }
        set { _money = value; OnChangeMoney?.Invoke(); }
    }

    public int Jelly
    {
        get { return _jelly; }
        set 
        { 
            _jelly = value;
            OnChangeJelly?.Invoke();
        }
    }

    public int MaxJelly
    {
        get { return _maxJelly; }
        set { _maxJelly = value; Jelly += _maxJelly; }
    }


    public void UpdateGoods()
    {
        OnChangeDia?.Invoke();
        OnChangeMoney?.Invoke();
        OnChangeJelly?.Invoke();
    }

    #endregion

    #region 쿠키정보
    public Vector3 battlePosition;

    public List<CookieInfo> allCookies;
    #endregion

    #region 건물정보
    public List<BuildingInfo> OwnedCraftableBuildings;
    #endregion

    #region 아이템 정보
    public DataBase MyDataBase { get; set; }
    #endregion

    public DateTime PrevCraftTime { get; set; } // kingdomManageState를 나간 시간

    // 저장할 데이터
    // ==============================================
    // 저장안하는 데이터

    public StageData StageData { get; set; }

    public EKingdomState StartKingdomState { get; set; }

    public void Init()
    {
        
    }

    /// <summary>
    /// 로그인 되면 호출
    /// </summary>
    public void LoadData()
    {
        UserInfo userInfo = GameManager.SQL.UserInfo;

        _kingdomIndex = userInfo.KingdomIndex;
        _kingdomName = userInfo.KingdomName;
        _dia = userInfo.Dia;
        _isFirst = userInfo.IsFirst == 0;
        _money = userInfo.Money;
        _dia = userInfo.Dia;
        _jelly = userInfo.Jelly;
        _maxJelly = userInfo.MaxJelly;

        PrevCraftTime = DateTime.ParseExact(userInfo.LastTime, "yyyyMMddHHmmss",
            System.Globalization.CultureInfo.InvariantCulture);

        prevJellyTime = DateTime.ParseExact(userInfo.LastJellyTime, "yyyyMMddHHmmss",
            System.Globalization.CultureInfo.InvariantCulture);
        jellyTime = userInfo.JellyTime;

        allCookies = userInfo.cookieJson.allCookies;
        OwnedCraftableBuildings = userInfo.buildingJson.allBuildings;
        battlePosition = userInfo.cookieJson.battlePosition;
    }

    public async UniTask SaveData()
    {
        UserInfo userInfo = GameManager.SQL.UserInfo;

        UpdateJellyTime();

        int isFirst = 0;
        if (!_isFirst)
            isFirst = 1;

        userInfo.SaveData(_kingdomIndex, _kingdomName, isFirst, _money, _dia, _jelly, _maxJelly,
            PrevCraftTime, ReturnItemDataInfo(), prevJellyTime, jellyTime);
    }

    private string ReturnItemDataInfo()
    {
        string result = "";

        foreach(var itemInfo in MyDataBase.itemDataBase)
        {
            result += itemInfo.Value + ",";
        }
        result = result.TrimEnd(',');

        return result;
    }

    public void UpdateJellyTime()
    {
        int diffTime = (int)((DateTime.Now - prevJellyTime).TotalSeconds);
        prevJellyTime = DateTime.Now;
        if (_jelly >= _maxJelly)
            return;

        if (diffTime >= jellyTime)
        {
            diffTime -= jellyTime;
            int count = diffTime / Utils.JellyTime;
            _jelly += 1 + count;
            jellyTime = diffTime % Utils.JellyTime;

            if (_jelly >= _maxJelly)
                _jelly = _maxJelly;
        }
        else
        {
            jellyTime -= diffTime;
        }
    }
}
