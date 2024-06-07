using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/MonsterGroupData", order = 1)]
public class MonsterGroupData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; // ���� �ĺ���
    public string Name; // �̸�

    [Header("MonsterGroup Info")]
    public List<int> FrontMonsterKeyList; // ���� ���� Key ����Ʈ
    public List<int> MiddleMonsterKeyList; // �߹� ���� Key ����Ʈ
    public List<int> BackMonsterKeyList; // �Ĺ� ���� Key ����Ʈ
}

