using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUIController : MonoBehaviour
{
    private int _star;

    [SerializeField]
    private GameObject _battleVictoryUI;

    [SerializeField]
    private GameObject _battleDefeatUI;



    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _battleVictoryUI.SetActive(false);
        _battleDefeatUI.SetActive(false);
    }

    public void SetResultUI(bool isWin)
    {
        if (isWin)
        {
            SetResult();
            _battleVictoryUI.SetActive(true);
        }
        else 
        {
            _battleDefeatUI.SetActive(false);
        }
    }

    private void SetResult() 
    {
        SetStar();
    }

    private void SetStar() 
    {
        _star = 1; //기본 1개

        // 죽은 쿠키가 1이하 -> 스타 추가
        if (BattleManager.Instance.KilledCookies <= 1) 
        {
            _star++;
        }

        // 남은시간이 40초 이상 -> 스타 추가
        if (BattleUIManager.Instance.BattleTime > 40f)
        {
            _star++;
        }
    }

    private void SetStarUI() 
    {

    }
}
