using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Pathfinding;

public class CookieBundleInAdventure : MonoBehaviour
{
    [SerializeField] private Transform[] _poses;
    [SerializeField] private CookiePathFinder[] _cookiePathFinder;
    [SerializeField] private Transform _cookieParent;

    private KingdomManager _manager;
    private Camera _cam;

    private Path _path;
    private Seeker _seeker;

    private bool _isInit = false;

    public Transform CookieParent => _cookieParent;

    public void Init()
    {
        if(!_isInit)
        {
            _manager = FindObjectOfType<KingdomManager>();
            _seeker = GetComponent<Seeker>();
            _cam = Camera.main;
            _isInit = true;
        }

        List<CookieController> allCookies = _manager.allCookies;

        for(int i = 0; i < _cookiePathFinder.Length; i++)
            _cookiePathFinder[i].Init(null, null);

        int cookieCount = 0;
        for(int i = 0; i < allCookies.Count; i++)
        {
            if (allCookies[i].CookieStat.IsBattleMember)
            {
                int positionIndex = allCookies[i].CookieStat.BattlePosition;
                _cookiePathFinder[cookieCount].gameObject.SetActive(true);
                _cookiePathFinder[cookieCount++].Init(allCookies[i].Data, _poses[positionIndex]);
            }
        }
    }

    public void OnMove()
    {
        Vector2 currentPos = Vector2.zero;
        if (Mouse.current != null && Mouse.current.enabled)
            currentPos = Mouse.current.position.ReadValue();
        else if (Touchscreen.current != null && Touchscreen.current.enabled)
            currentPos = Touchscreen.current.position.ReadValue();

        Vector3 targetPos = _cam.ScreenToWorldPoint(currentPos);
        _seeker.StartPath(transform.position, targetPos, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;

            Vector3 lastPos = _path.vectorPath[_path.vectorPath.Count - 1];
            Vector3 prevPos = _path.vectorPath[_path.vectorPath.Count - 2];

            float theta = Mathf.Atan((lastPos.y - prevPos.y) / (lastPos.x - prevPos.x)) * Mathf.Rad2Deg;
            if (lastPos.x == prevPos.x)
                theta = 90;

            transform.position = lastPos;
            GameManager.Game.battlePosition = transform.localPosition;

            transform.localEulerAngles = Vector3.forward * theta + Vector3.forward * 90;

            for(int i = 0; i < _cookiePathFinder.Length; i++)
                if(_cookiePathFinder[i].gameObject.activeSelf)
                    _cookiePathFinder[i].StartPathFinding();
        }
    }
}



