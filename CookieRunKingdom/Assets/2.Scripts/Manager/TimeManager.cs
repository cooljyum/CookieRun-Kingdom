using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private Dictionary<int, Dictionary<int, float>> _totalTimer = new();

    private void Update()
    {
        // �Ϸ�� Ÿ�̸Ӹ� ������ List
        List<(int buildingKey, int itemKey)> completedTimers = new List<(int, int)>();

        // �� Ÿ�̸� Dictionary�� ���纻�� �ݺ��Ͽ� ���� �� ���� ������ ����
        foreach (var buildingTimers in _totalTimer.ToList())
        {
            int buildingKey = buildingTimers.Key;
            foreach (var itemTimer in buildingTimers.Value.ToList())
            {
                int itemKey = itemTimer.Key;
                // ������ ������ ���� ����� �ð���ŭ ���� �������� Ÿ�̸Ӹ� ����
                _totalTimer[buildingKey][itemKey] -= Time.deltaTime;

                // Ÿ�̸Ӱ� �Ϸ�Ǿ����� �� �ǹ� �� ������ Ű ���� �Ϸ�� Ÿ�̸� ����Ʈ�� �߰�
                if (_totalTimer[buildingKey][itemKey] <= 0.0f)
                {
                    completedTimers.Add((buildingKey, itemKey));
                }
            }
        }
        // �Ϸ�� Ÿ�̸� ó��
        foreach (var completedTimer in completedTimers)
        {
            int buildingKey = completedTimer.buildingKey;
            int itemKey = completedTimer.itemKey;

            Debug.Log($"Timer completed for building {buildingKey}, item {itemKey}");

            // �Ϸ�� ������ Ÿ�̸Ӹ� Dictionary���� ����
            _totalTimer[buildingKey].Remove(itemKey);

            // �ش� �ǹ��� �� �̻� ������ Ÿ�̸Ӱ� ���� ���� ������, �ǹ� key�� Dictionary���� ����
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
        if (hours > 0) result += $"{hours}�ð� ";
        if (minutes > 0) result += $"{minutes}�� ";
        if (seconds > 0) result += $"{seconds}��";

        return result.Trim();
    }
}
