using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    private List<List<GameObject>> _battleCookies;
    private List<BattleObjectSpawnManager.TeamData> _enemiesTeamList;

    private void Start()
    {
        BattleObjectSpawnManager.Instance.Init();
        _battleCookies = BattleObjectSpawnManager.Instance.GetBattleCookies();
        _enemiesTeamList = BattleObjectSpawnManager.Instance.GetEnemiesTeamList();

        AssignTargetsToCookies();
        AssignTargetsToEnemies();
    }

    private void AssignTargetsToCookies()
    {
        foreach (var cookieList in _battleCookies)
        {
            foreach (var cookie in cookieList)
            {
                BattleObject cookieObject = cookie.GetComponent<BattleObject>();
                GameObject closestEnemy = FindClosestEnemy(cookieObject.transform);
                if (closestEnemy != null)
                {
                    cookieObject.SetTarget(closestEnemy.transform);
                }
            }
        }
    }

    private void AssignTargetsToEnemies()
    {
        if (_enemiesTeamList.Count == 0)
            return;

        var firstEnemyTeam = _enemiesTeamList[0];
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(firstEnemyTeam.Front.BattleObj);
        enemies.AddRange(firstEnemyTeam.Middle.BattleObj);
        enemies.AddRange(firstEnemyTeam.Back.BattleObj);

        foreach (var enemy in enemies)
        {
            BattleObject enemyObject = enemy.GetComponent<BattleObject>();
            GameObject closestCookie = FindClosestCookie(enemyObject.transform);
            if (closestCookie != null)
            {
                enemyObject.SetTarget(closestCookie.transform);
            }
        }
    }

    private GameObject FindClosestEnemy(Transform cookieTransform)
    {
        GameObject closestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var team in _enemiesTeamList)
        {
            List<GameObject> enemies = new List<GameObject>();
            enemies.AddRange(team.Front.BattleObj);
            enemies.AddRange(team.Middle.BattleObj);
            enemies.AddRange(team.Back.BattleObj);

            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(cookieTransform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    private GameObject FindClosestCookie(Transform enemyTransform)
    {
        GameObject closestCookie = null;
        float minDistance = float.MaxValue;

        foreach (var cookieList in _battleCookies)
        {
            foreach (var cookie in cookieList)
            {
                float distance = Vector3.Distance(enemyTransform.position, cookie.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCookie = cookie;
                }
            }
        }

        return closestCookie;
    }
}
