using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private Dictionary<int, Dictionary<int, float>> _totalTimer = new();

    private void Update()
    {
        List<(int buildingKey, int itemKey)> completedTimers = new List<(int, int)>();

        foreach (var buildingTimers in _totalTimer.ToList())
        {
            int buildingKey = buildingTimers.Key;
            foreach (var itemTimer in buildingTimers.Value.ToList())
            {
                int itemKey = itemTimer.Key;
                _totalTimer[buildingKey][itemKey] -= Time.deltaTime;

                if (_totalTimer[buildingKey][itemKey] <= 0.0f)
                {
                    completedTimers.Add((buildingKey, itemKey));
                }
            }
        }

        foreach (var completedTimer in completedTimers)
        {
            int buildingKey = completedTimer.buildingKey;
            int itemKey = completedTimer.itemKey;

            Debug.Log($"Timer completed for building {buildingKey}, item {itemKey}");
            _totalTimer[buildingKey].Remove(itemKey);

            if (_totalTimer[buildingKey].Count == 0)
            {
                _totalTimer.Remove(buildingKey);
            }
        }
    }

    public void AddTime(int buildingKey, CraftItemInfo craftItemInfo)
    {
        if (!_totalTimer.ContainsKey(buildingKey))
        {
            _totalTimer[buildingKey] = new Dictionary<int, float>();
        }

        _totalTimer[buildingKey][craftItemInfo.ResultItem.Key] = craftItemInfo.RequiredTime;
    }

    public float GetRemainingTime(int buildingKey, int itemKey)
    {
        if (_totalTimer.ContainsKey(buildingKey) && _totalTimer[buildingKey].ContainsKey(itemKey))
        {
            return _totalTimer[buildingKey][itemKey];
        }

        return 0.0f;
    }

    public static string ConvertTime(int totalSeconds)
    {
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        string result = "";
        if (hours > 0) result += $"{hours}시간 ";
        if (minutes > 0) result += $"{minutes}분 ";
        if (seconds > 0) result += $"{seconds}초";

        return result.Trim();
    }
}
