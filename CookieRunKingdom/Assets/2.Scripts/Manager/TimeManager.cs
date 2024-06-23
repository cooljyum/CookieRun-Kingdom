using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private Dictionary<int, Dictionary<int, float>> _totalTimer = new();

    private void Update()
    {
        // 완료된 타이머를 저장할 List
        List<(int buildingKey, int itemKey)> completedTimers = new List<(int, int)>();

        // 총 타이머 Dictionary의 복사본을 반복하여 열거 중 수정 문제를 피함
        foreach (var buildingTimers in _totalTimer.ToList())
        {
            int buildingKey = buildingTimers.Key;
            foreach (var itemTimer in buildingTimers.Value.ToList())
            {
                int itemKey = itemTimer.Key;
                // 마지막 프레임 이후 경과된 시간만큼 현재 아이템의 타이머를 감소
                _totalTimer[buildingKey][itemKey] -= Time.deltaTime;

                // 타이머가 완료되었으면 이 건물 및 아이템 키 쌍을 완료된 타이머 리스트에 추가
                if (_totalTimer[buildingKey][itemKey] <= 0.0f)
                {
                    completedTimers.Add((buildingKey, itemKey));
                }
            }
        }
        // 완료된 타이머 처리
        foreach (var completedTimer in completedTimers)
        {
            int buildingKey = completedTimer.buildingKey;
            int itemKey = completedTimer.itemKey;

            Debug.Log($"Timer completed for building {buildingKey}, item {itemKey}");

            // 완료된 아이템 타이머를 Dictionary에서 제거
            _totalTimer[buildingKey].Remove(itemKey);

            // 해당 건물에 더 이상 아이템 타이머가 남아 있지 않으면, 건물 key도 Dictionary에서 제거
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
