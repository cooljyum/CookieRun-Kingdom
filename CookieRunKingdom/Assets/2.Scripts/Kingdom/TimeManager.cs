using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private Dictionary<int, Dictionary<int, float>> _totalTimer = new();

    private void Update()
    {
        foreach(KeyValuePair<int, Dictionary<int, float>> timers in _totalTimer)
        {
            foreach (KeyValuePair<int, float> timer in timers.Value)
            {
                _totalTimer[timers.Key][timer.Key] -= Time.deltaTime;

                if(_totalTimer[timers.Key][timer.Key] <= 0.0f)
                {
                    //timers.Key : 해당건물
                    //timer.key : 아이템                    
                }
            }
        }
    }

    public void AddTime(int buildingKey, CraftItemInfo craftItemInfo)
    {
        _totalTimer.Add(buildingKey, null);
        _totalTimer[buildingKey].Add(craftItemInfo.ResultItem.Key, craftItemInfo.RequiredTime);
    }

    public float GetRemainTime(int buildingKey, int itemKey)
    {
        return _totalTimer[buildingKey][itemKey];
    }
}
