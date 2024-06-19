using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/StageData", order = 1)]
public class StageData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; // ���� �ĺ���
    public string Name; // �̸�

    [Header("Stage Info")]
    public List<int> MonsterGroupList; // ���ͱ׷� ����Ʈ
    public List<StageItem> StageItemList; // ������ ����Ʈ
}

[System.Serializable]
public class StageItem
{
    public int Key; // ������ Ű
    public float Value; // ������ ��� ��ġ
}
