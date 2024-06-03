using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieController : BaseController
{
    protected CookieCitizen _cookieCitizen;
    protected CookieStat _cookieStat;

    public CookieCitizen CookieCitizeon => _cookieCitizen;
    public CookieStat CookieStat => _cookieStat;

    protected override void Awake()
    {
        base.Awake();

        _cookieCitizen = GetComponent<CookieCitizen>();
        _cookieStat = GetComponent<CookieStat>();

        _cookieCitizen.Init(this);
        _cookieStat.Init(this);
    }
}
