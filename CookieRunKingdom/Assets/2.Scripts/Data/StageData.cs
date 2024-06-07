using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/StageData", order = 1)]
public class StageData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; // 고유 식별자
    public string Name; // 이름

    [Header("Stage Info")]
    public List<int> MonsterGroupList; // 몬스터그룹 리스트
}