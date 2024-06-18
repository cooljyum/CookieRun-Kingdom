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
        _star = 1; //�⺻ 1��

        // ���� ��Ű�� 1���� -> ��Ÿ �߰�
        if (BattleManager.Instance.KilledCookies <= 1) 
        {
            _star++;
        }

        // �����ð��� 40�� �̻� -> ��Ÿ �߰�
        if (BattleUIManager.Instance.BattleTime > 40f)
        {
            _star++;
        }
    }

    private void SetStarUI() 
    {

    }
}
