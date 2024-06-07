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
}