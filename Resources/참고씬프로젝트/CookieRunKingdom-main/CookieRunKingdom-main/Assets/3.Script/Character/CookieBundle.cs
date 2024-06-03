using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CookieBundle : MonoBehaviour
{
    /// <summary>
    /// 2 3 1  전방
    /// 5 6 4  중앙
    /// 8 9 7  후방
    /// </summary>
    [SerializeField] private Transform[] cookiePositions;

    private List<CookieController> Cookies => BattleManager.instance.CookieList;
    private Camera _camera;
    private Tweener _cameraTween;
    private float _moveSpeed;

    private void Update()
    {
        float currentSpeed = GetMoveSpeed();
        if (currentSpeed == 0)
            _cameraTween.ChangeEndValue(5.0f, 1.5f, true).Restart();
        else
            _cameraTween.ChangeEndValue(6.5f, 1.5f, true).Restart();

        transform.position += Utils.Dir.normalized * currentSpeed * Time.deltaTime;
    }

    private float GetMoveSpeed()
    {
        /*
         모든 쿠키들이 뛰고 있거나 스킬을 쓰고 있을때
         다만 모든 쿠키가 스킬을 쓰면 안움직임
         */
        float currentSpeed = 0;
        int maxCookie = BattleManager.instance.CurrentCookieCount;
        int runCookie = 0;
        int skillCookie = 0;
        int knockbackCookie = 0;

        for (int i = 0; i < Cookies.Count; i++)
        {
            CharacterBattleController cookie = Cookies[i].CharacterBattleController;

            if (cookie.IsDead)
                continue;

            if (cookie.CheckState(EBattleState.BattleRunState))
                runCookie++;
            else if (cookie.CheckState(EBattleState.BattleSkillState))
                skillCookie++;
            else if (cookie.CheckState(EBattleState.BattleCrowdControlState) && cookie.CurrentCCType == ECCType.KnockBack)
                knockbackCookie++;
        }

        if (runCookie + skillCookie == maxCookie && skillCookie != maxCookie)
            currentSpeed = Cookies[0].Data.MoveSpeed;
        else if(knockbackCookie == maxCookie)
            currentSpeed = BattleManager.instance.KnockBackPower * 2;

        return currentSpeed;
    }

    public void Init()
    {
        _camera = Camera.main;
        _cameraTween = _camera.DOOrthoSize(5.5f, 1.5f).SetAutoKill(false);

        for (int i= 0; i < Cookies.Count; i++)
            MovePosition(Cookies[i]);

        foreach (BaseController cookie in Cookies)
        {
            cookie.gameObject.layer = LayerMask.NameToLayer("Cookie");
            cookie.CharacterBattleController.StartBattle(true);
            cookie.CharacterBattleController.SetEnemy(LayerMask.NameToLayer("Enemy"));
        }
    }

    private void MovePosition(CookieController cookie)
    {
        int battlePosition = cookie.CookieStat.BattlePosition;

        cookie.transform.SetParent(cookiePositions[battlePosition]);
        cookie.transform.localPosition = Vector3.zero;
        cookie.transform.SetParent(null);
        cookie.CharacterBattleController.SetPosition(this, cookiePositions[battlePosition]);
    }
}
