using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieStat : MonoBehaviour
{
    private int _cookieLevel;
    private int _evolutionCount;
    private int _battlePosition;
    private bool _isBattleMember;

    private bool _isHave;
    private int _evolutionGauge;
    private int _evolutionMaxGague;

    private int _maxExpValue = 100;

    private CookieController _controller;
    private CookieData _data;

    public int CookieLevel => _cookieLevel;
    public int EvolutionCount => _evolutionCount;
    public int BattlePosition => _battlePosition;
    public bool IsBattleMember => _isBattleMember;

    public bool IsHave { get { return _isHave; } set { _isHave = value; } }
    public int EvolutionGauge { get { return _evolutionGauge; } set { _evolutionGauge = value; } }
    public int EvolutionMaxGauge { get { return _evolutionMaxGague; } set { _evolutionMaxGague = value; } }

    public void Init(CookieController controller)
    {
        _controller = controller;
        _data = ((CookieData)_controller.Data);
    }

    public void SetBattle(bool isBattle, int position)
    {
        _isBattleMember = isBattle;
        _battlePosition = position;
    }

    public void SaveCookie()
    {
        CookieInfo cookieInfo = GameManager.Game.allCookies[_data.CookieIndex];
        cookieInfo.cookieLevel = _cookieLevel;
        cookieInfo.evolutionCount = _evolutionCount;
        cookieInfo.battlePosition = _battlePosition;
        cookieInfo.isHave = _isHave;
        cookieInfo.isBattleMember = _isBattleMember;
        cookieInfo.evolutionGauge = _evolutionGauge;
        cookieInfo.evolutionMaxGauge = _evolutionMaxGague;
    }

    public void LoadCookie()
    {
        CookieInfo cookieInfo = GameManager.Game.allCookies[_data.CookieIndex];
        _cookieLevel = cookieInfo.cookieLevel;
        _evolutionCount = cookieInfo.evolutionCount;
        _battlePosition = cookieInfo.battlePosition;
        _isHave = cookieInfo.isHave;
        _isBattleMember = cookieInfo.isBattleMember;

        _evolutionGauge = cookieInfo.evolutionGauge;
        _evolutionMaxGague = cookieInfo.evolutionMaxGauge;

        for (int i = 1; i < _cookieLevel; i++)
            _controller.CharacterStat.LevelUP();

        for (int i = 0; i < _evolutionCount; i++)
            _controller.CharacterStat.Evolution();
    }

    public void LevelUp()
    {
        if(!_isHave)
        {
            GuideDisplayer.Instance.ShowGuide("해당 쿠키를 소유하고 있지 않습니다!");
            return;
        }

        int expAmount = DataBaseManager.Instance.MyDataBase.itemDataBase[_data.ExpCandyItemData];
        if(expAmount * 14 >= _maxExpValue)
        {
            DataBaseManager.Instance.AddItem(_data.ExpCandyItemData, -(int)Mathf.Ceil((float)_maxExpValue / 14));
        }
        else
        {
            GuideDisplayer.Instance.ShowGuide("별사탕이 부족합니다!");
            return;
        }

        if (_cookieLevel < 60)
        {
            _cookieLevel++;
            _controller.CharacterStat.LevelUP();
        }
        else
        {
            GuideDisplayer.Instance.ShowGuide("최대 레벨입니다!");
        }
    }

    public void Evolution()
    {
        if (!_isHave)
        {
            GuideDisplayer.Instance.ShowGuide("해당 쿠키를 소유하고 있지 않습니다!");
            return;
        }

        if (_evolutionGauge < _evolutionMaxGague)
        {
            GuideDisplayer.Instance.ShowGuide("소울이 부족합니다!");
            return;
        }

        if(_evolutionCount < 5)
        {
            _evolutionGauge -= _evolutionMaxGague;

            _evolutionCount++;
            _controller.CharacterStat.Evolution();
        }
        else
        {
            GuideDisplayer.Instance.ShowGuide("최대 등급입니다!");
        }
    }
}
